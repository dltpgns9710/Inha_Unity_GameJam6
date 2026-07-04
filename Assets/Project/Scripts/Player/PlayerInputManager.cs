using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JUNBEOM.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        #region Private Fields

        private PlayerInputActions _inputActions;

        #endregion

        #region Events

        public event Action<float> OnMoveEvent;
        public event Action<bool> OnRunEvent;
        public event Action OnJumpEvent;
        public event Action OnEquipFlashlightEvent;
        public event Action OnToggleFlashlightEvent;
        public event Action OnToggleSettingsEvent;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _inputActions = new PlayerInputActions();

            // Player Action Map 구독
            _inputActions.Player.Move.performed += OnMove;
            _inputActions.Player.Move.canceled += OnMove;

            _inputActions.Player.Run.performed += OnRun;
            _inputActions.Player.Run.canceled += OnRun;

            _inputActions.Player.Jump.started += OnJump;
            _inputActions.Player.EquipFlashlight.started += OnEquipFlashlight;
            _inputActions.Player.ToggleFlashlight.started += OnToggleFlashlight;

            // UI Action Map 구독
            _inputActions.UI.ToggleSettings.started += OnToggleSettings;
        }

        private void OnEnable()
        {
            EnablePlayerInput();
            _inputActions.UI.Enable(); // UI 액션맵은 항상 활성화
        }

        private void OnDisable()
        {
            _inputActions.Disable();
        }

        #endregion

        #region Public Methods

        public void EnablePlayerInput()
        {
            _inputActions.Player.Enable();
        }

        public void DisablePlayerInput()
        {
            _inputActions.Player.Disable();
            OnMoveEvent?.Invoke(0f); // 입력 차단 시 이동 취소 보정
            OnRunEvent?.Invoke(false);
        }

        #endregion

        #region Input Callbacks

        private void OnMove(InputAction.CallbackContext context)
        {
            float inputX = context.ReadValue<float>();
            OnMoveEvent?.Invoke(inputX);
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            bool isRunning = context.ReadValueAsButton();
            OnRunEvent?.Invoke(isRunning);
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            OnJumpEvent?.Invoke();
        }

        private void OnEquipFlashlight(InputAction.CallbackContext context)
        {
            OnEquipFlashlightEvent?.Invoke();
        }

        private void OnToggleFlashlight(InputAction.CallbackContext context)
        {
            OnToggleFlashlightEvent?.Invoke();
        }

        private void OnToggleSettings(InputAction.CallbackContext context)
        {
            OnToggleSettingsEvent?.Invoke();
        }

        #endregion
    }
}