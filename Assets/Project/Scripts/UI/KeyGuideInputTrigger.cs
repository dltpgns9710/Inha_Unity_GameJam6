using System;
using UnityEngine;
using JUNBEOM.Player;

namespace SEHOON.UI
{
    public class KeyGuideInputTrigger : MonoBehaviour
    {
        public enum EAction
        {
            Show,
            Hide,
        }

        #region Serialized Fields
        [SerializeField] private PlayerInputManager _inputManager;
        [SerializeField] private EGuideTrigger _listenTo;

        [SerializeField] private KeyGuideView _keyGuideView;
        [SerializeField] private EGuideTrigger _target;
        [SerializeField] private EAction _action;

        [SerializeField] private GameObject _activateTarget;
        [SerializeField] private GameObject _deactivateTarget;
        #endregion

        #region Private Fields
        private Action<float> _moveHandler;
        private Action<bool> _runHandler;
        private Action _actionHandler;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (_inputManager == null) return;

            switch (_listenTo)
            {
                case EGuideTrigger.Move:
                    _moveHandler = value => { if (value != 0f) Fire(); };
                    _inputManager.OnMoveEvent += _moveHandler;
                    break;
                case EGuideTrigger.Run:
                    _runHandler = value => { if (value) Fire(); };
                    _inputManager.OnRunEvent += _runHandler;
                    break;
                case EGuideTrigger.Jump:
                    _actionHandler = Fire;
                    _inputManager.OnJumpEvent += _actionHandler;
                    break;
                case EGuideTrigger.Interact:
                    _actionHandler = Fire;
                    _inputManager.OnInteractEvent += _actionHandler;
                    break;
                case EGuideTrigger.EquipFlashlight:
                    _actionHandler = Fire;
                    _inputManager.OnEquipFlashlightEvent += _actionHandler;
                    break;
                case EGuideTrigger.ToggleFlashlight:
                    _actionHandler = Fire;
                    _inputManager.OnToggleFlashlightEvent += _actionHandler;
                    break;
                case EGuideTrigger.ReturnCamera:
                    _actionHandler = Fire;
                    _inputManager.OnReturnCameraEvent += _actionHandler;
                    break;
                case EGuideTrigger.RequestDetectAnomaly:
                    _actionHandler = Fire;
                    _inputManager.OnRequestDetectAnomalyEvent += _actionHandler;
                    break;
                case EGuideTrigger.RequestWait:
                    _actionHandler = Fire;
                    _inputManager.OnRequestWaitEvent += _actionHandler;
                    break;
            }
        }

        private void OnDisable()
        {
            if (_inputManager == null) return;

            if (_moveHandler != null) _inputManager.OnMoveEvent -= _moveHandler;
            if (_runHandler != null) _inputManager.OnRunEvent -= _runHandler;
            if (_actionHandler != null)
            {
                _inputManager.OnJumpEvent -= _actionHandler;
                _inputManager.OnInteractEvent -= _actionHandler;
                _inputManager.OnEquipFlashlightEvent -= _actionHandler;
                _inputManager.OnToggleFlashlightEvent -= _actionHandler;
                _inputManager.OnReturnCameraEvent -= _actionHandler;
                _inputManager.OnRequestDetectAnomalyEvent -= _actionHandler;
                _inputManager.OnRequestWaitEvent -= _actionHandler;
            }
        }
        #endregion

        #region Private Methods
        private void Fire()
        {
            if (_keyGuideView != null)
            {
                if (_action == EAction.Show) _keyGuideView.Show(_target);
                else _keyGuideView.Hide(_target);
            }

            if (_activateTarget != null) _activateTarget.SetActive(true);
            if (_deactivateTarget != null) _deactivateTarget.SetActive(false);
        }
        #endregion
    }
}
