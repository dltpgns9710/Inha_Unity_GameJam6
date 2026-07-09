using System;
using UnityEngine;

namespace JUNBEOM.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        #region Private Fields

        private bool _hasKey;

        private PlayerEventManager _eventManager;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _hasKey = false;
            _eventManager = PlayerEventManager.Instance;
        }

        private void OnEnable()
        {
            _eventManager.OnKeyCollected += HandleKeyCollected;
            _eventManager.OnKeyStateRequested += HandleKeyStateRequested;
        }

        private void Start()
        {
            _eventManager.BroadcastKeyState(_hasKey);
        }

        private void OnDisable()
        {
            _eventManager.OnKeyCollected -= HandleKeyCollected;
            _eventManager.OnKeyStateRequested -= HandleKeyStateRequested;
        }

        #endregion

        #region Event Handlers

        private void HandleKeyCollected()
        {
            if (_hasKey)
            {
                return;
            }

            _hasKey = true;
            _eventManager.BroadcastKeyState(_hasKey);
        }

        private void HandleKeyStateRequested(Action<bool> response)
        {
            response?.Invoke(_hasKey);
        }

        #endregion
    }
}