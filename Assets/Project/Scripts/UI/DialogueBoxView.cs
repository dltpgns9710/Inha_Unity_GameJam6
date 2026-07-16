using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

namespace SEHOON.UI
{
    public class DialogueBoxView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Dialogue Lines")]
        [SerializeField] private List<DialogueLine> _dialogueLines = new List<DialogueLine>();

        [Header("Text Widgets")]
        [SerializeField] private TextMeshProUGUI _nameTextWidget;
        [SerializeField] private TextBoxItemView _textBoxItem;

        [Header("Font")]
        [SerializeField] private TMP_FontAsset _font;

        [Header("Skip Button")]
        [SerializeField] private Button _skipButton;

        [Header("Events")]
        [SerializeField] private UnityEvent _onEnableEvent = new UnityEvent();
        [SerializeField] private UnityEvent _onDisableEvent = new UnityEvent();
        #endregion

        #region Private Fields
        private int _currentIndex = 0;
        private CanvasGroup _skipButtonGroup;
        private bool _skipInputThisFrame;
        #endregion

        #region Property Fields
        public UnityEvent OnEnableEvent => _onEnableEvent;
        public UnityEvent OnDisableEvent => _onDisableEvent;

        public List<DialogueLine> DialogueLines
        {
            get => _dialogueLines;
            set => _dialogueLines = value;
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _currentIndex = 0;
            ApplyFont();
            ShowLine(_currentIndex);

            if (_skipButton != null)
            {
                _skipButtonGroup = _skipButton.GetComponent<CanvasGroup>();
                if (_skipButtonGroup == null) _skipButtonGroup = _skipButton.gameObject.AddComponent<CanvasGroup>();
            }

            _skipButton?.onClick.AddListener(OnSkipButtonClicked);
        }

        private void OnValidate()
        {
            ApplyFont();
            ShowLine(_currentIndex);
        }

        private void OnEnable()
        {
            _currentIndex = 0;
            ShowLine(_currentIndex);
            _skipInputThisFrame = true;

            _onEnableEvent?.Invoke();
        }

        private void OnDisable()
        {
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

            if (_skipInputThisFrame)
            {
                _skipInputThisFrame = false;
                return;
            }

            if ((Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
                (Mouse.current != null && 
                 (Mouse.current.leftButton.wasPressedThisFrame  || 
                  Mouse.current.rightButton.wasPressedThisFrame  || 
                  Mouse.current.middleButton.wasPressedThisFrame )))
            {
                if (_textBoxItem != null && _textBoxItem.IsRevealing)
                {
                    _textBoxItem.CompleteText();
                }
                else if (_currentIndex >= _dialogueLines.Count - 1)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    Next();
                }
            }
        }
        #endregion

        #region Public Methods
        public void ShowLine(int index)
        {
            if (_dialogueLines == null || _dialogueLines.Count == 0) return;

            _currentIndex = Mathf.Clamp(index, 0, _dialogueLines.Count - 1);
            DialogueLine line = _dialogueLines[_currentIndex];

            if (_nameTextWidget != null) _nameTextWidget.text = line.Name;
            if (_textBoxItem != null)
            {
                _textBoxItem.Text = line.Dialogue;
                _textBoxItem.SetAlignment(line.Alignment);
            }

            if (_skipButton != null) _skipButton.gameObject.SetActive(_currentIndex < _dialogueLines.Count - 1);
        }

        public void Next() => ShowLine(_currentIndex + 1);

        public void OnSkipButtonClicked()
        {
            if (Time.timeScale == 0f) return;

            gameObject.SetActive(false);
        }
        #endregion

        #region Private Methods
        private void ApplyFont()
        {
            if (_font == null) return;

            if (_nameTextWidget != null) _nameTextWidget.font = _font;
            if (_textBoxItem != null) _textBoxItem.SetFont(_font);
        }
        #endregion
    }
}
