using UnityEngine;

namespace JUNBEOM.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerFlashlight : MonoBehaviour
    {
        #region Serialized Fields

        [Header("References")]
        [SerializeField] private PlayerInputManager _inputManager;

        [Tooltip("실제 빛을 뿜는 Light 2D 또는 손전등 게임 오브젝트")]
        [SerializeField] private GameObject _flashlightObject;

        #endregion

        #region Private Fields

        private Animator _animator;
        private bool _isEquipped;
        private bool _isTurnedOn;

        #endregion

        #region Animator Hash

        private static class AnimHash
        {
            // 플래시를 들고 있는 자세(모션)를 위한 bool
            public static readonly int IsFlashEquipped = Animator.StringToHash("IsFlashEquipped");
        }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            Debug.Assert(_inputManager != null, $"[{name}] PlayerInputManager 누락");
            Debug.Assert(_flashlightObject != null, $"[{name}] _flashlightObject 누락");

            // 초기 상태: 플래시를 들지 않음 & 꺼짐
            _flashlightObject.SetActive(false);
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

        #endregion

        #region Input Event Handlers

        private void HandleEquipFlashlight()
        {
            _isEquipped = !_isEquipped;

            // 애니메이터에 플래시 장착 상태 전달 (꺼내는 모션 재생)
            _animator.SetBool(AnimHash.IsFlashEquipped, _isEquipped);

            // 플래시를 주머니에 넣을 때는 빛도 강제로 끕니다.
            if (!_isEquipped && _isTurnedOn)
            {
                _isTurnedOn = false;
                _flashlightObject.SetActive(false);
            }
        }

        private void HandleToggleFlashlight()
        {
            // 플래시를 꺼내지 않은 상태면 켤 수 없음
            bool canToggle = _isEquipped;
            if (!canToggle) return;

            _isTurnedOn = !_isTurnedOn;
            _flashlightObject.SetActive(_isTurnedOn);
        }

        #endregion
    }
}