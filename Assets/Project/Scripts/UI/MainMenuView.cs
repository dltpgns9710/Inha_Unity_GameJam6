using System.Collections;
using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace SEHOON.UI
{
    public class MainMenuView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Button Actions")]
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private string _sceneToLoad;
        [SerializeField] private GameObject _creditsPrefab;
        [SerializeField] private Transform _creditsSpawnParent;

        [Header("Button Fade In")]
        [SerializeField] private CanvasGroup _startButtonGroup;
        [SerializeField] private CanvasGroup _creditsButtonGroup;
        [SerializeField] private CanvasGroup _quitButtonGroup;
        [SerializeField] private float _buttonFadeDuration = 0.6f;

        [Header("Text Widgets")]
        [SerializeField] private TextMeshProUGUI _titleTextWidget;
        [SerializeField] private TextMeshProUGUI _subtitleTextWidget;
        [SerializeField] private TextMeshProUGUI _startButtonTextWidget;
        [SerializeField] private TextMeshProUGUI _creditsButtonTextWidget;
        [SerializeField] private TextMeshProUGUI _quitButtonTextWidget;

        [Header("Font")]
        [SerializeField] private TMP_FontAsset _titleFont;
        [SerializeField] private TMP_FontAsset _subtitleFont;
        [SerializeField] private TMP_FontAsset _buttonFont;

        [Header("Text")]
        [SerializeField] private string _titleText;
        [SerializeField] private string _subtitleText;
        [SerializeField] private string _startButtonText;
        [SerializeField] private string _creditsButtonText;
        [SerializeField] private string _quitButtonText;
        #endregion

        #region Private Fields
        private GameObject _creditsInstance;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            ApplyFont();
            ApplyText();
            BindButtons();

            SetButtonsAlpha(0f);
            StartCoroutine(CoFadeInButtons());
        }

        private void OnValidate()
        {
            ApplyFont();
            ApplyText();
        }
        #endregion

        #region Public Methods
        public void OnStartButtonClicked()
        {
            DataManager.Instance.Init();
            SceneManager.LoadScene(_sceneToLoad);
        }

        public void OnCreditsButtonClicked()
        {
            if (_creditsPrefab == null) return;

            if (_creditsInstance == null)
            {
                Transform parent = _creditsSpawnParent != null ? _creditsSpawnParent : transform.parent;
                _creditsInstance = Instantiate(_creditsPrefab, parent);
            }
            else
            {
                _creditsInstance.SetActive(true);
            }
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        #endregion

        #region Private Methods
        private void BindButtons()
        {
            _startButton?.onClick.AddListener(OnStartButtonClicked);
            _creditsButton?.onClick.AddListener(OnCreditsButtonClicked);
            _quitButton?.onClick.AddListener(OnQuitButtonClicked);
        }

        private void SetButtonsAlpha(float alpha)
        {
            bool interactable = alpha >= 1f;

            if (_startButtonGroup != null)
            {
                _startButtonGroup.alpha = alpha;
                _startButtonGroup.interactable = interactable;
                _startButtonGroup.blocksRaycasts = interactable;
            }

            if (_creditsButtonGroup != null)
            {
                _creditsButtonGroup.alpha = alpha;
                _creditsButtonGroup.interactable = interactable;
                _creditsButtonGroup.blocksRaycasts = interactable;
            }

            if (_quitButtonGroup != null)
            {
                _quitButtonGroup.alpha = alpha;
                _quitButtonGroup.interactable = interactable;
                _quitButtonGroup.blocksRaycasts = interactable;
            }
        }

        private void ApplyFont()
        {
            if (_titleFont != null && _titleTextWidget != null) _titleTextWidget.font = _titleFont;
            if (_subtitleFont != null && _subtitleTextWidget != null) _subtitleTextWidget.font = _subtitleFont;

            if (_buttonFont != null)
            {
                if (_startButtonTextWidget != null) _startButtonTextWidget.font = _buttonFont;
                if (_creditsButtonTextWidget != null) _creditsButtonTextWidget.font = _buttonFont;
                if (_quitButtonTextWidget != null) _quitButtonTextWidget.font = _buttonFont;
            }
        }

        private void ApplyText()
        {
            if (!string.IsNullOrEmpty(_titleText) && _titleTextWidget != null) _titleTextWidget.text = _titleText;
            if (!string.IsNullOrEmpty(_subtitleText) && _subtitleTextWidget != null) _subtitleTextWidget.text = _subtitleText;
            if (!string.IsNullOrEmpty(_startButtonText) && _startButtonTextWidget != null) _startButtonTextWidget.text = _startButtonText;
            if (!string.IsNullOrEmpty(_creditsButtonText) && _creditsButtonTextWidget != null) _creditsButtonTextWidget.text = _creditsButtonText;
            if (!string.IsNullOrEmpty(_quitButtonText) && _quitButtonTextWidget != null) _quitButtonTextWidget.text = _quitButtonText;
        }
        #endregion

        #region Coroutines
        private IEnumerator CoFadeInButtons()
        {
            float elapsed = 0f;
            while (elapsed < _buttonFadeDuration)
            {
                elapsed += Time.deltaTime;
                SetButtonsAlpha(Mathf.Clamp01(elapsed / _buttonFadeDuration));
                yield return null;
            }

            SetButtonsAlpha(1f);
        }
        #endregion
    }
}
