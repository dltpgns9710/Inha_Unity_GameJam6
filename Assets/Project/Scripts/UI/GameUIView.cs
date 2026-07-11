using UnityEngine;
using UnityEngine.UI;
using JUNBEOM.Player;

namespace SEHOON.UI
{
    public class GameUIView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Light Ratio")]
        [SerializeField] private Slider _lightRatioSlider;

        [Header("Key State")]
        [SerializeField] private GameObject _keyStateIcon;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            PlayerEventManager.Instance.OnLightRatioChanged += HandleLightRatioChanged;
            PlayerEventManager.Instance.OnKeyStateChanged += HandleKeyStateChanged;

            HandleLightRatioChanged(PlayerEventManager.Instance.CurrentLightRatio);
            PlayerEventManager.Instance.OnKeyStateRequested?.Invoke(HandleKeyStateChanged);
        }

        private void OnDisable()
        {
            PlayerEventManager.Instance.OnLightRatioChanged -= HandleLightRatioChanged;
            PlayerEventManager.Instance.OnKeyStateChanged -= HandleKeyStateChanged;
        }
        #endregion

        #region Private Methods
        private void HandleLightRatioChanged(float ratio)
        {
            if (_lightRatioSlider != null) _lightRatioSlider.value = ratio;
        }

        private void HandleKeyStateChanged(bool hasKey)
        {
            if (_keyStateIcon != null) _keyStateIcon.SetActive(hasKey);
        }
        #endregion
    }
}
