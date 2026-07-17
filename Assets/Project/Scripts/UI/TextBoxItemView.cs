using System.Collections;
using SEHOON.GameSystem;
using UnityEngine;
using TMPro;

namespace SEHOON.UI
{
    public class TextBoxItemView : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private TextMeshProUGUI _textWidget;

        [Header("Typewriter")]
        [SerializeField] private float _charInterval = 0.03f;
        
        [Header("Sound")]
        [SerializeField] private AudioClip _typingSound;
        #endregion

        #region Private Fields
        private string _fullText = string.Empty;
        private Coroutine _typewriterCoroutine;
        #endregion

        #region Properties
        public string Text
        {
            get => _textWidget != null ? _textWidget.text : string.Empty;
            set => SetText(value);
        }

        public bool IsRevealing => _typewriterCoroutine != null;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (Application.isPlaying && !string.IsNullOrEmpty(_fullText))
            {
                PlayTypewriter();
            }
        }

        private void OnDisable()
        {
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }
        }
        #endregion

        #region Public Methods
        public void SetText(string text)
        {
            _fullText = text ?? string.Empty;

            if (Application.isPlaying && gameObject.activeInHierarchy)
            {
                PlayTypewriter();
            }
            else if (_textWidget != null)
            {
                _textWidget.text = _fullText;
            }
        }

        public void SetFont(TMP_FontAsset font)
        {
            if (font != null && _textWidget != null)
            {
                _textWidget.font = font;
            }
        }

        public void SetAlignment(EDialogueBoxAlignment alignment)
        {
            if (_textWidget == null) return;

            _textWidget.alignment = alignment == EDialogueBoxAlignment.Right
                ? TextAlignmentOptions.TopRight
                : TextAlignmentOptions.TopLeft;
        }

        public void CompleteText()
        {
            SoundManager.Instance.StopLoopSfx();
            if (_typewriterCoroutine != null)
            {
                StopCoroutine(_typewriterCoroutine);
                _typewriterCoroutine = null;
            }

            if (_textWidget != null) _textWidget.text = _fullText;
        }
        #endregion

        #region Private Methods
        private void PlayTypewriter()
        {
            if (_typewriterCoroutine != null) StopCoroutine(_typewriterCoroutine);
            
            _typewriterCoroutine = StartCoroutine(CoRevealText());
        }
        #endregion

        #region Coroutines
        private IEnumerator CoRevealText()
        {
            if (_textWidget == null) yield break;

            _textWidget.text = string.Empty;
            SoundManager.Instance.PlayLoopSfx(_typingSound);
            for (int i = 1; i <= _fullText.Length; i++)
            {
                _textWidget.text = _fullText.Substring(0, i);
                yield return new WaitForSeconds(_charInterval);
            }
            
            SoundManager.Instance.StopLoopSfx();
            _typewriterCoroutine = null;
        }
        #endregion
    }
}
