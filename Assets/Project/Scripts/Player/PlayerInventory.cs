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
            PlayerEventManager.Instance.OnKeyCollected += HandleKeyCollected;
            PlayerEventManager.Instance.OnKeyStateRequested += HandleKeyStateRequested;
        }

        private void Start()
        {
            PlayerEventManager.Instance.OnKeyStateChanged?.Invoke(_hasKey);
        }

        private void OnDisable()
        {
            PlayerEventManager.Instance.OnKeyCollected -= HandleKeyCollected;
            PlayerEventManager.Instance.OnKeyStateRequested -= HandleKeyStateRequested;
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
            PlayerEventManager.Instance.OnKeyStateChanged?.Invoke(_hasKey);
        }

        private void HandleKeyStateRequested(Action<bool> response)
        {
            response?.Invoke(_hasKey);
        }

        #endregion
    }
}