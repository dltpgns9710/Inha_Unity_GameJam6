using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace SEHOON.UI
{
    public class PauseMenuView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Title")]
        [SerializeField] private string _titleText;

        [Header("Buttons")]
        [SerializeField] private string _resumeButtonText;
        [SerializeField] private string _quitButtonText;

        [Header("Button Actions")]
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _quitButton;

        [Header("Font")]
        [SerializeField] private TMP_FontAsset _font;

        [Header("Text Widgets")]
        [SerializeField] private TextMeshProUGUI _titleTextWidget;
        [SerializeField] private TextMeshProUGUI _resumeButtonTextWidget;
        [SerializeField] private TextMeshProUGUI _quitButtonTextWidget;

        [Header("Events")]
        [SerializeField] private UnityEvent _onEnableEvent;
        [SerializeField] private UnityEvent _onDisableEvent;
        #endregion

        #region Properties
        public string TitleText
        {
            get => _titleText;
            set { _titleText = value; ApplyTexts(); }
        }

        public string ResumeButtonText
        {
            get => _resumeButtonText;
            set { _resumeButtonText = value; ApplyTexts(); }
        }

        public string QuitButtonText
        {
            get => _quitButtonText;
            set { _quitButtonText = value; ApplyTexts(); }
        }

        public TMP_FontAsset Font
        {
            get => _font;
            set { _font = value; ApplyFont(); }
        }
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            ApplyTexts();
            ApplyFont();
            BindButtons();
        }

        private void OnValidate()
        {
            ApplyTexts();
            ApplyFont();
        }

        private void OnEnable()
        {
            Time.timeScale = 0f;

            _onEnableEvent?.Invoke();
        }

        private void OnDisable()
        {
            _onDisableEvent?.Invoke();

            Time.timeScale = 1f;
        }
        #endregion

        #region Public Methods
        public void OnResumeButtonClicked()
        {
            gameObject.SetActive(false);
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
            _resumeButton?.onClick.AddListener(OnResumeButtonClicked);
            _quitButton?.onClick.AddListener(OnQuitButtonClicked);
        }

        private void ApplyTexts()
        {
            if (_titleTextWidget != null) _titleTextWidget.text = _titleText;
            if (_resumeButtonTextWidget != null) _resumeButtonTextWidget.text = _resumeButtonText;
            if (_quitButtonTextWidget != null) _quitButtonTextWidget.text = _quitButtonText;
        }

        private void ApplyFont()
        {
            if (_font == null) return;

            if (_titleTextWidget != null) _titleTextWidget.font = _font;
            if (_resumeButtonTextWidget != null) _resumeButtonTextWidget.font = _font;
            if (_quitButtonTextWidget != null) _quitButtonTextWidget.font = _font;
        }
        #endregion
    }
}
