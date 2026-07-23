using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace SEHOON.UI
{
    public class WarningPopupView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Message")]
        [SerializeField] private string _messageText;
        [SerializeField] private float _displayDuration = 3f;

        [Header("Font")]
        [SerializeField] private TMP_FontAsset _font;

        [Header("Text Widgets")]
        [SerializeField] private TextMeshProUGUI _messageTextWidget;

        [Header("Events")]
        [SerializeField] private UnityEvent _onDisableEvent;
        #endregion

        #region Properties
        public UnityEvent OnDisableEvent => _onDisableEvent;
        public float DisplayDuration => _displayDuration;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            ApplyText();
            ApplyFont();
        }

        private void OnValidate()
        {
            ApplyText();
            ApplyFont();
        }

        private void OnDisable()
        {
            _onDisableEvent?.Invoke();
        }
        #endregion

        #region Private Methods
        private void ApplyText()
        {
            if (_messageTextWidget != null) _messageTextWidget.text = _messageText;
        }

        private void ApplyFont()
        {
            if (_font != null && _messageTextWidget != null) _messageTextWidget.font = _font;
        }
        #endregion
    }
}
