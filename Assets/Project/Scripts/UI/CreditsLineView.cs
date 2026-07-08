using UnityEngine;
using TMPro;

namespace SEHOON.UI
{
    public class CreditsLineView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textWidget;

        public string Text
        {
            get => _textWidget.text;
            set => _textWidget.text = value;
        }

        public void SetFont(TMP_FontAsset font)
        {
            if (font != null && _textWidget != null)
            {
                _textWidget.font = font;
            }
        }
    }
}
