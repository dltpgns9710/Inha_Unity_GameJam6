using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace JUNBEOM.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerFlashlight : MonoBehaviour
    {
        #region Constants

        private const float MINIMUM_DECREASE_DURATION = 0.1f;

        #endregion

        #region Serialized Fields

        [Header("References")]
        [SerializeField] private PlayerInputManager _inputManager;

        [SerializeField] private GameObject _flashlightObject;

        [SerializeField] private Light2D _flashlightLight;

        [Header("Light Test Settings")]
        [SerializeField] private float _maximumLightIntensity = 1.0f;

        [SerializeField] private float _minimumLightIntensity = 0.0f;

        [SerializeField] private float _lightDecreaseDuration = 10.0f;

        #endregion

        #region Private Fields

        private Animator _animator;

        private float _remainingLightTime;

        private bool _isEquipped;
        private bool _isTurnedOn;
        #endregion

        #region Properties

        public bool IsEquipped => _isEquipped;
        public bool IsTurnedOn => _isTurnedOn;
        public float CurrentLightIntensity => _flashlightLight.intensity;

        public float CurrentLightRatio { get; private set; } = 1.0f;

        /// <summary>
        /// 현재 손전등의 남은 빛 비율
        /// </summary>
        public float RemainingLightRatio
        {
            get
            {
                if (_lightDecreaseDuration <= 0.0f)
                {
                    return 0.0f;
                }

                return Mathf.Clamp01(
                    _remainingLightTime / _lightDecreaseDuration);
            }
        }

        #endregion

        #region Animator Hash

        private static class AnimHash
        {
            public static readonly int IsFlashEquipped =
                Animator.StringToHash("IsFlashEquipped");
        }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            if ((_flashlightLight == null) &&
                (_flashlightObject != null))
            {
                _flashlightLight =
                    _flashlightObject.GetComponentInChildren<Light2D>(true);
            }

            _lightDecreaseDuration = Mathf.Max(
                MINIMUM_DECREASE_DURATION,
                _lightDecreaseDuration);

            _remainingLightTime = _lightDecreaseDuration;
            _flashlightLight.intensity = _maximumLightIntensity;

            PlayerEventManager.Instance.OnLightRatioChanged?.Invoke(
                CurrentLightRatio);

            _isEquipped = false;
            SetFlashlightActive(false);
        }

        private void OnEnable()
        {
            _inputManager.OnEquipFlashlightEvent += HandleEquipFlashlight;
            _inputManager.OnToggleFlashlightEvent += HandleToggleFlashlight;
        }

        private void OnDisable()
        {
            _inputManager.OnEquipFlashlightEvent -= HandleEquipFlashlight;
            _inputManager.OnToggleFlashlightEvent -= HandleToggleFlashlight;
        }

        private void Update()
        {
            if (!_isTurnedOn)
            {
                return;
            }

            DecreaseLight();
        }

        #endregion

        #region Input Event Handlers

        private void HandleEquipFlashlight()
        {
            _isEquipped = !_isEquipped;

            _animator.SetBool(
                AnimHash.IsFlashEquipped,
                _isEquipped);

            if (!_isEquipped)
            {
                SetFlashlightActive(false);
            }
        }

        private void HandleToggleFlashlight()
        {
            if (!_isEquipped)
            {
                return;
            }

            SetFlashlightActive(!_isTurnedOn);
        }

        #endregion

        #region Private Methods

        private void DecreaseLight()
        {
            _remainingLightTime -= Time.deltaTime;
            _remainingLightTime = Mathf.Max(
                0.0f,
                _remainingLightTime);

            float lightRatio = RemainingLightRatio;

            _flashlightLight.intensity = Mathf.Lerp(
                _minimumLightIntensity,
                _maximumLightIntensity,
                lightRatio);

            CurrentLightRatio = Mathf.Clamp01(lightRatio);
            PlayerEventManager.Instance.OnLightRatioChanged?.Invoke(CurrentLightRatio);

            if (_remainingLightTime <= 0.0f)
            {
                Debug.Log($"[{name}] 손전등 빛이 모두 감소했습니다.");
            }
        }

        private void SetFlashlightActive(bool isActive)
        {
            _isTurnedOn = isActive;
            _flashlightObject.SetActive(isActive);
        }

        #endregion
    }
}
