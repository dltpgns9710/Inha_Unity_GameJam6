using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace SEHOON.UI
{
    public class FadeTextView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI _textWidget;

        [Header("Fade")]
        [SerializeField] private float _fadeDuration = 1.5f;

        [Header("Events")]
        [SerializeField] private UnityEvent _onDisableEvent;
        #endregion

        #region Private Fields
        private Coroutine _fadeCoroutine;
        #endregion

        #region Public Methods
        public void SetText(string text)
        {
            if (_textWidget != null) _textWidget.text = text;
        }
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(CoFadeIn());
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

            gameObject.SetActive(false);
        }
        #endregion
    }
}
