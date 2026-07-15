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

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();

            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMove;

            _playerInput.actions["Run"].performed += OnRun;
            _playerInput.actions["Run"].canceled += OnRun;

            _playerInput.actions["Jump"].started += OnJump;
            _playerInput.actions["EquipFlashlight"].started += OnEquipFlashlight;
            _playerInput.actions["ToggleFlashlight"].started += OnToggleFlashlight;

            _playerInput.actions["Interact"].started += OnInteract;

            _playerInput.actions["ReturnCamera"].started += OnReturnCamera;

            _playerInput.actions["RequestDetectAnomaly"].started += OnRequestDetectAnomaly;
            _playerInput.actions["RequestWait"].started += OnRequestWait;
            _playerInput.actions["Confirm"].started += OnConfirm;
            _playerInput.actions["Cancel"].started += OnCancel; 
            _playerInput.actions["PauseMenu"].started += OnPauseMenu; 
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