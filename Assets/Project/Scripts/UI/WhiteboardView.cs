using TMPro;
using UnityEngine;

namespace SEHOON.UI
{
    public class WhiteboardView : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private TextMeshProUGUI _hintText;

        [Header("Default State")]
        [SerializeField, TextArea(3, 10)] private string _defaultText;
        [SerializeField] private Color _defaultColor = Color.black;

        [Header("Global Anomaly Hint")]
        [SerializeField] private Color _hintColor = new Color32(0xE6, 0x00, 0x00, 0xFF);
        [SerializeField] private float _hintTiltAngle = -10f;
        [SerializeField] private float _hintFontSize = 12f;
        #endregion

        #region Private Fields
        private float _defaultFontSize;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (_hintText != null) _defaultFontSize = _hintText.fontSize;

            ClearHint();
        }
        #endregion

        #region Public Methods
        public void ShowHint(string hintText)
        {
            if (_hintText == null) return;
            if (string.IsNullOrEmpty(hintText)) return;

            _hintText.text = hintText;
            _hintText.color = _hintColor;
            _hintText.fontSize = _hintFontSize;
            _hintText.rectTransform.localRotation = Quaternion.Euler(0f, 0f, _hintTiltAngle);
        }

        public void ClearHint()
        {
            if (_hintText == null) return;

            _hintText.text = _defaultText;
            _hintText.color = _defaultColor;
            _hintText.fontSize = _defaultFontSize;
            _hintText.rectTransform.localRotation = Quaternion.identity;
        }
        #endregion
    }
}
