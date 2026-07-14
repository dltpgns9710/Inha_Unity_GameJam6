using System;
using TAEWOOK.Helper.Core;
using UnityEngine;
using JUNBEOM.Player;
    public class PlayerAIInteract : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInputManager _inputManager;
        [SerializeField] private HelperCommandBroadcaster _HelperInteract;
        public Texture2D _findCursor;
        public Texture2D _normalCursor;

    #region Private Fields
        private bool _canDetect;
        private bool _cantMove;
        private Vector2 hotSpot = Vector2.zero;
    #endregion

    private void Awake()
        {
            _cantMove = false;
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
            Cursor.SetCursor(_findCursor, hotSpot, CursorMode.Auto);
        }

        private void HandleRequestWait()
        {
            _HelperInteract.RequestWait();
            _cantMove = true;
        }

        private void HandleConfirm()
        {
            if (_canDetect == false|| _cantMove ==true)
            {
                return;
            }
            //Vector2 searchPosition = Input.mousePosition;

            Vector2 searchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _HelperInteract.RequestDetectAnomaly(searchPosition);
            _canDetect = false;
            Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
            _cantMove = false;
        }

        private void HandleCancel()
        {
            _canDetect = false;
            Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
        }
    }

