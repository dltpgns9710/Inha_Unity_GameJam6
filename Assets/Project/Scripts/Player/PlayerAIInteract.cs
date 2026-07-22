using System;
using TAEWOOK.Helper.Core;
using UnityEngine;
using JUNBEOM.Player;
public class PlayerAIInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputManager _inputManager;
    [SerializeField] private HelperCommandBroadcaster _HelperInteract;
    [SerializeField] private HelperControllar _HelperController;
    public Texture2D _findCursor;
    public Texture2D _normalCursor;
    public Texture2D _cancleCursor;

    #region Private Fields
    private bool _canDetect;
    private Vector2 hotSpot = Vector2.zero;
    #endregion
    private void Start()
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
        if (_HelperController.HasDetectChance)
        {
            Cursor.SetCursor(_cancleCursor, hotSpot, CursorMode.Auto);
            _canDetect = false;
        }
        else
        {
            Cursor.SetCursor(_findCursor, hotSpot, CursorMode.Auto);
            _canDetect = true;
        }
    }

    private void HandleRequestWait()
    {
        _HelperInteract.RequestWait();
    }

    private void HandleConfirm()
    {
        if (!_canDetect)
        {
            Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
            return;
        }
        Vector2 searchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _HelperInteract.RequestDetectAnomaly(searchPosition);
        Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
        _canDetect = false;
    }

    private void HandleCancel()
    {
        Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
    }
}
