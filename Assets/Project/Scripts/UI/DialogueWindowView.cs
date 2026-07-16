using System.Collections;
using System.Collections.Generic;
using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace SEHOON.UI
{
    public class DialogueWindowView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Dialogue Lines")]
        [SerializeField] private List<DialogueLine> _dialogueLines = new List<DialogueLine>();

        [Header("Setup")]
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private RectTransform _content;
        [SerializeField] private TextBoxItemView _textBoxItemPrefab;
        [SerializeField] private TMP_FontAsset _font;
        [SerializeField] private float _itemHeight = 175f;
        [SerializeField] private float _itemSpacing = 15f;

        [Header("Skip Button")]
        [SerializeField] private Button _skipButton;

        [Header("Layout")]
        [SerializeField, Range(0.1f, 1f)] private float _itemWidthRatio = 0.7f;

        [Header("Animation")]
        [SerializeField] private float _slideDistance = 60f;
        [SerializeField] private float _slideDuration = 0.25f;

        [Header("Events")]
        [SerializeField] private UnityEvent _onEnableEvent;
        [SerializeField] private UnityEvent _onDisableEvent;
        #endregion

        #region Private Fields
        private int _currentIndex = 0;
        private readonly List<RectTransform> _items = new List<RectTransform>();
        private readonly List<TextBoxItemView> _itemViews = new List<TextBoxItemView>();
        private Coroutine _revealCoroutine;
        private CanvasGroup _skipButtonGroup;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _currentIndex = 0;
            BuildItems();

            if (_content != null) _content.sizeDelta = new Vector2(_content.sizeDelta.x, 0f);
            if (_scrollRect != null) _scrollRect.enabled = false;

            if (_skipButton != null)
            {
                _skipButtonGroup = _skipButton.GetComponent<CanvasGroup>();
                if (_skipButtonGroup == null) _skipButtonGroup = _skipButton.gameObject.AddComponent<CanvasGroup>();
            }

            _skipButton?.onClick.AddListener(OnSkipButtonClicked);
        }

        private void OnEnable()
        {
            _currentIndex = 0;

            if (_revealCoroutine != null)
            {
                StopCoroutine(_revealCoroutine);
                _revealCoroutine = null;
            }

            foreach (RectTransform item in _items)
            {
                item.gameObject.SetActive(false);
            }

            if (_content != null) _content.sizeDelta = new Vector2(_content.sizeDelta.x, 0f);
            if (_scrollRect != null) _scrollRect.enabled = false;

            _onEnableEvent?.Invoke();
            ShowNext();
        }

        private void OnDisable()
        {
            SoundManager.Instance.StopLoopSfx();
            _onDisableEvent?.Invoke();
        }

        private void Update()
        {
            bool isPaused = Time.timeScale == 0f;

            if (_skipButtonGroup != null)
            {
                _skipButtonGroup.interactable = !isPaused;
                _skipButtonGroup.blocksRaycasts = !isPaused;
            }

            if (isPaused) return;

            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Mouse.current != null && 
                 (Mouse.current.leftButton.wasPressedThisFrame  || 
                 Mouse.current.rightButton.wasPressedThisFrame  || 
                 Mouse.current.middleButton.wasPressedThisFrame )))
            {
                TextBoxItemView currentItem = (_currentIndex > 0 && _currentIndex - 1 < _itemViews.Count)
                    ? _itemViews[_currentIndex - 1]
                    : null;

                if (currentItem != null && currentItem.IsRevealing)
                {
                    currentItem.CompleteText();
                }
                else
                {
                    ShowNext();
                }
            }
        }
        #endregion

        #region Public Methods
        public void OnSkipButtonClicked()
        {
            if (Time.timeScale == 0f) return;

            gameObject.SetActive(false);
        }

        public void ShowNext()
        {
            if (_currentIndex >= _items.Count)
            {
                gameObject.SetActive(false);
                return;
            }

            RectTransform rect = _items[_currentIndex];
            Vector2 itemTargetPosition = rect.anchoredPosition;

            float oldContentY = CalculateContentTargetY(_currentIndex * (_itemHeight + _itemSpacing));

            _currentIndex++;

            if (_content != null)
            {
                _content.sizeDelta = new Vector2(_content.sizeDelta.x, _currentIndex * (_itemHeight + _itemSpacing));
            }

            float newContentY = CalculateContentTargetY(_currentIndex * (_itemHeight + _itemSpacing));

            rect.gameObject.SetActive(true);

            if (_revealCoroutine != null) StopCoroutine(_revealCoroutine);
            _revealCoroutine = StartCoroutine(CoRevealAnimation(rect, itemTargetPosition, oldContentY, newContentY));
        }
        #endregion

        #region Private Methods
        private void BuildItems()
        {
            for (int i = 0; i < _dialogueLines.Count; i++)
            {
                DialogueLine line = _dialogueLines[i];

                TextBoxItemView item = Instantiate(_textBoxItemPrefab, _content);
                item.Text = line.Dialogue;
                item.SetFont(_font);
                item.SetAlignment(line.Alignment);

                float anchorMinX = line.Alignment == EDialogueBoxAlignment.Left ? 0f : 1f - _itemWidthRatio;
                float anchorMaxX = line.Alignment == EDialogueBoxAlignment.Left ? _itemWidthRatio : 1f;

                RectTransform rect = item.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(anchorMinX, 1f);
                rect.anchorMax = new Vector2(anchorMaxX, 1f);
                rect.pivot = new Vector2(0.5f, 1f);
                rect.sizeDelta = new Vector2(0f, _itemHeight);
                rect.anchoredPosition = new Vector2(0f, -(i * (_itemHeight + _itemSpacing)));

                item.gameObject.SetActive(false);
                _items.Add(rect);
                _itemViews.Add(item);
            }
        }

        private float CalculateContentTargetY(float height)
        {
            float viewportHeight = (_scrollRect != null && _scrollRect.viewport != null)
                ? _scrollRect.viewport.rect.height
                : height;

            return height - viewportHeight;
        }

        private void SetContentY(float y)
        {
            if (_content == null) return;

            if (_scrollRect != null)
            {
                _scrollRect.enabled = y > 0f;
            }

            _content.anchoredPosition = new Vector2(0f, y);
        }
        #endregion

        #region Coroutines
        private IEnumerator CoRevealAnimation(RectTransform rect, Vector2 itemTargetPosition, float oldContentY, float newContentY)
        {
            Vector2 itemStartPosition = itemTargetPosition + Vector2.down * _slideDistance;
            rect.anchoredPosition = itemStartPosition;

            float elapsed = 0f;
            while (elapsed < _slideDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / _slideDuration);

                rect.anchoredPosition = Vector2.Lerp(itemStartPosition, itemTargetPosition, t);
                SetContentY(Mathf.Lerp(oldContentY, newContentY, t));

                yield return null;
            }

            rect.anchoredPosition = itemTargetPosition;
            SetContentY(newContentY);
            _revealCoroutine = null;
        }
        #endregion
    }
}
