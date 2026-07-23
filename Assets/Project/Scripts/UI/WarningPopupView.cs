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
        [SerializeField] private string _messageText = "콘텐츠 경고\n\n이 게임에는 다음과 같은 요소가 포함되어 있습니다.\n\n예고 없이 갑자기 나타나는 놀라는 장면\n큰 소리로 갑작스럽게 재생되는 효과음\n혐오감이나 불쾌감을 줄 수 있는 기괴하고 그로테스크한 이미지\n긴장감과 공포감을 조성하는 연출\n\n심장 질환이 있거나 큰 소리와 갑작스러운 자극에 민감하신 분, 임산부, 어린이는 플레이에 유의하시기 바랍니다. 플레이 중 불편함이나 스트레스를 느끼신다면 즉시 게임을 중단하시길 권장합니다.";
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
