using System;
using UnityEngine;

namespace JUNBEOM.Player
{
    public class PlayerInventory : MonoBehaviour
    {
        #region Private Fields

        private bool _hasKey;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _hasKey = false;
        }

        private void OnEnable()
        {
            PlayerEventChannel.OnKeyCollected += HandleKeyCollected;
            PlayerEventChannel.OnKeyStateRequested += HandleKeyStateRequested;
        }

        private void Start()
        {
            PlayerEventChannel.BroadcastKeyState(_hasKey);
        }

        private void OnDisable()
        {
            PlayerEventChannel.OnKeyCollected -= HandleKeyCollected;
            PlayerEventChannel.OnKeyStateRequested -= HandleKeyStateRequested;
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
            PlayerEventChannel.BroadcastKeyState(_hasKey);
        }

        private void HandleKeyStateRequested(Action<bool> response)
        {
            response?.Invoke(_hasKey);
        }

        #endregion
    }
}