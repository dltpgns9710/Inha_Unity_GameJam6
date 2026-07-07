using System;
using UnityEngine;

namespace JUNBEOM.Player
{
    public static class PlayerEventChannel
    {
        #region Properties

        public static float CurrentLightRatio { get; private set; } = 1.0f;

        #endregion

        #region Events

        public static event Action<GameObject, GameObject> OnInteractionRequested;
        public static event Action OnKeyCollected;
        public static event Action<bool> OnKeyStateChanged;
        public static event Action<Action<bool>> OnKeyStateRequested;
        public static event Action<float> OnLightRatioChanged;

        #endregion

        #region Public Methods

        public static void BroadcastInteraction(
            GameObject target,
            GameObject interactor)
        {
            OnInteractionRequested?.Invoke(target, interactor);
        }

        public static void NotifyKeyCollected()
        {
            OnKeyCollected?.Invoke();
        }

        public static void BroadcastKeyState(bool hasKey)
        {
            OnKeyStateChanged?.Invoke(hasKey);
        }

        public static void RequestKeyState(Action<bool> response)
        {
            OnKeyStateRequested?.Invoke(response);
        }

        public static void BroadcastLightRatio(float lightRatio)
        {
            CurrentLightRatio = Mathf.Clamp01(lightRatio);
            OnLightRatioChanged?.Invoke(CurrentLightRatio);
        }

        #endregion
    }
}
