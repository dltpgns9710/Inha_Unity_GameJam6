using System;
using TAEWOOK.Helper.Core;
using UnityEngine;
using JUNBEOM.Player;
    public class PlayerAIInteract : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputManager _inputManager;
        [SerializeField] private HelperCommandBroadcaster _HelperInteract;

        #region Private Fields
        private bool _canDetect;
        #endregion

        private void Awake()
        {
            _canDetect = false;
        }

        private void OnEnable()
        {
            _inputManager.OnRequestDetectAnomalyEvent += HandleRequestDetectAnomaly;
            _inputManager.OnRequestWaitEvent += HandleRequestWait;
            _inputManager.OnConfirmEvent += HandleConfirm;
            _inputManager.OnCancelEvent += HandleCancel;
        }

        private void OnDisable()
        {
            _inputManager.OnRequestDetectAnomalyEvent -= HandleRequestDetectAnomaly;
            _inputManager.OnRequestWaitEvent -= HandleRequestWait;
            _inputManager.OnConfirmEvent -= HandleConfirm;
            _inputManager.OnCancelEvent -= HandleCancel;
        }


        private void HandleRequestDetectAnomaly()
        {
            _canDetect = true;
        }

        private void HandleRequestWait()
        {
            _HelperInteract.RequestWait();
        }

        private void HandleConfirm()
        {
            if (!_canDetect)
            {
                return;
            }
            //Vector2 searchPosition = Input.mousePosition;

            Vector2 searchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _HelperInteract.RequestDetectAnomaly(searchPosition);
        }

        private void HandleCancel()
        {
            _canDetect = false;
        }
    }

