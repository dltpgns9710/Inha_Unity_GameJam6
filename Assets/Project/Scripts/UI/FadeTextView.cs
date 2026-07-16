using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System;
using SEHOON.GameSystem;

namespace SEHOON.UI
{
    public class FadeTextView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _textWidget;

        [Header("Fade")]
        [SerializeField] private float _fadeDuration = 1.5f;

        [SerializeField] private bool _isDoFadeOut = true;
        
        [Header("Events")]
        [SerializeField] private UnityEvent _onDisableEvent = new UnityEvent();

        [SerializeField] private AudioClip _doorCloseSound;

        public event Action _fadeOutEnd;
        public event Action _fadeInEnd;
        #endregion

        #region Private Fields
        private Coroutine _fadeCoroutine;
        #endregion

        #region Public Methods
        public void SetText(string text)
        {
            if (_textWidget != null) _textWidget.text = text;
        }

        public UnityEvent OnDisableEvent => _onDisableEvent;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = _isDoFadeOut? StartCoroutine(CoFadeOut()) : StartCoroutine(CoFadeIn());
        }

        private void OnDisable()
        {
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
                _fadeCoroutine = null;
            }
            _onDisableEvent?.Invoke();
        }
        #endregion

        #region Coroutines
        private IEnumerator CoFadeIn()
        {
            if (_textWidget == null) yield break;

            Color color = _textWidget.color;
            color.a = 0f;
            _textWidget.color = color;

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = Mathf.Clamp01(elapsed / _fadeDuration);
                _textWidget.color = color;
                yield return null;
            }

            color.a = 1f;
            _textWidget.color = color;
            _fadeCoroutine = null;
            _isDoFadeOut = true;
            _fadeInEnd?.Invoke();
            //gameObject.SetActive(false);
        }
        
        private IEnumerator CoFadeOut()
        {
            if (_textWidget == null) yield break;

            SoundManager.Instance.PlaySfx(_doorCloseSound);
            Color color = _textWidget.color;
            color.a = 1f;
            _textWidget.color = color;

            float elapsed = 0f;
            while (elapsed < _fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = 1f - Mathf.Clamp01(elapsed / _fadeDuration);
                _textWidget.color = color;
                yield return null;
            }

            color.a = 0f;
            _textWidget.color = color;
            _fadeCoroutine = null;
            _isDoFadeOut = false;
            _fadeOutEnd?.Invoke();
            gameObject.SetActive(false);
        }
        #endregion
    }
}
