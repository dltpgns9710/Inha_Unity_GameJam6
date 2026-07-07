using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SEHOON.GameSystem;

namespace SEHOON.UI
{
    public class ButtonSoundEffect : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
    {
        #region Serialized Fields
        [Header("Sound Effects")]
        [SerializeField] private AudioClip _hoverSound;
        [SerializeField] private AudioClip _clickSound;

        [Header("Settings")]
        [SerializeField] private bool _playHoverSound = true;
        [SerializeField] private bool _playClickSound = true;
        [SerializeField] private float _hoverVolumeScale = 0.5f;
        [SerializeField] private float _clickVolumeScale = 0.7f;
        #endregion

        #region Private Fields
        private Button _button;
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            _button = GetComponent<Button>();
        }
        #endregion

        #region Event Handlers
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_playHoverSound && _hoverSound != null && _button != null && _button.interactable)
            {
                SoundManager.Instance.PlaySfx(_hoverSound, _hoverVolumeScale);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_playClickSound && _clickSound != null && _button != null && _button.interactable)
            {
                SoundManager.Instance.PlaySfx(_clickSound, _clickVolumeScale);
            }
        }
        #endregion
    }
}
