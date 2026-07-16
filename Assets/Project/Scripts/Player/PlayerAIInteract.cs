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
    private Vector2 hotSpot = Vector2.zero;
    #endregion


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
        Cursor.SetCursor(_findCursor, hotSpot, CursorMode.Auto);    
    }

    private void HandleRequestWait()
    {
        _HelperInteract.RequestWait();
    }

    private void HandleConfirm()
    {
        Vector2 searchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _HelperInteract.RequestDetectAnomaly(searchPosition);
        Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
    }

    private void HandleCancel()
    {
        Cursor.SetCursor(_normalCursor, hotSpot, CursorMode.Auto);
    }
}