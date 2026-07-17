using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JUNBEOM.Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        #region Private Fields

        private PlayerInput _playerInput;

        #endregion

        #region Events

        public event Action<float> OnMoveEvent;
        public event Action<bool> OnRunEvent;
        public event Action OnJumpEvent;
        public event Action OnEquipFlashlightEvent;
        public event Action OnToggleFlashlightEvent;
        public event Action OnInteractEvent;
        public event Action OnReturnCameraEvent;
        public event Action OnRequestDetectAnomalyEvent;
        public event Action OnRequestWaitEvent;
        public event Action OnConfirmEvent;
        public event Action OnCancelEvent;
        public event Action OnPauseMenuEvent;

        #endregion

        #region Unity Lifecycle

        private InputActionAsset _inputActions;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _inputActions = _playerInput.actions;

            _inputActions["Move"].performed += OnMove;
            _inputActions["Move"].canceled += OnMove;

            _inputActions["Run"].performed += OnRun;
            _inputActions["Run"].canceled += OnRun;

            _inputActions["Jump"].started += OnJump;
            _inputActions["EquipFlashlight"].started += OnEquipFlashlight;
            _inputActions["ToggleFlashlight"].started += OnToggleFlashlight;

            _inputActions["Interact"].started += OnInteract;

            _inputActions["ReturnCamera"].started += OnReturnCamera;

            _inputActions["RequestDetectAnomaly"].started += OnRequestDetectAnomaly;
            _inputActions["RequestWait"].started += OnRequestWait;
            _inputActions["Confirm"].started += OnConfirm;
            _inputActions["Cancel"].started += OnCancel; 
            _inputActions["PauseMenu"].started += OnPauseMenu; 
        }

        private void OnDestroy()
        {
            if (_inputActions == null) return;
            _inputActions["Move"].performed -= OnMove;
            _inputActions["Move"].canceled -= OnMove;
            _inputActions["Run"].performed -= OnRun;
            _inputActions["Run"].canceled -= OnRun;
            _inputActions["Jump"].started -= OnJump;
            _inputActions["EquipFlashlight"].started -= OnEquipFlashlight;
            _inputActions["ToggleFlashlight"].started -= OnToggleFlashlight;
            _inputActions["Interact"].started -= OnInteract;
            _inputActions["ReturnCamera"].started -= OnReturnCamera;
            _inputActions["RequestDetectAnomaly"].started -= OnRequestDetectAnomaly;
            _inputActions["RequestWait"].started -= OnRequestWait;
            _inputActions["Confirm"].started -= OnConfirm;
            _inputActions["Cancel"].started -= OnCancel;
            _inputActions["PauseMenu"].started -= OnPauseMenu;
        }

        private void OnEnable()
        {
            EnablePlayerInput();
        }

        private void OnDisable()
        {
            DisablePlayerInput(); 
        }

        #endregion

        #region Public Methods

        public void EnablePlayerInput()
        {
            _playerInput.actions.Enable();
        }

        public void DisablePlayerInput()
        {
            if (_playerInput == null) return;
            _playerInput.actions.Disable();
            OnMoveEvent?.Invoke(0f);
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

        private void OnInteract(InputAction.CallbackContext context)
        {
            OnInteractEvent?.Invoke();
        }

        private void OnEquipFlashlight(InputAction.CallbackContext context)
        {
            OnEquipFlashlightEvent?.Invoke();
        }

        private void OnToggleFlashlight(InputAction.CallbackContext context)
        {
            OnToggleFlashlightEvent?.Invoke();
        }
        private void OnReturnCamera(InputAction.CallbackContext context)
        {
            OnReturnCameraEvent?.Invoke();
        }
        private void OnRequestDetectAnomaly(InputAction.CallbackContext context)
        {
            OnRequestDetectAnomalyEvent?.Invoke();
        }
        private void OnRequestWait(InputAction.CallbackContext context)
        {
            OnRequestWaitEvent?.Invoke();
        }
        private void OnConfirm(InputAction.CallbackContext context)
        {
            OnConfirmEvent?.Invoke();
        }
        private void OnCancel(InputAction.CallbackContext context)
        {
            OnCancelEvent?.Invoke();
        }
        private void OnPauseMenu(InputAction.CallbackContext context)
        {
            OnPauseMenuEvent?.Invoke();
        }


        #endregion
    }
}