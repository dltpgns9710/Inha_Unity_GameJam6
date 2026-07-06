using UnityEngine;
using TMPro;

namespace SEHOON.UI
{
    public class TextBoxItemView : MonoBehaviour
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

        public void SetAlignment(DialogueBoxAlignment alignment)
        {
            if (_textWidget == null) return;

            _textWidget.alignment = alignment == DialogueBoxAlignment.Right
                ? TextAlignmentOptions.TopRight
                : TextAlignmentOptions.TopLeft;
        }
    }
}
