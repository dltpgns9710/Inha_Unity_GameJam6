using System;
using UnityEngine;
using USingleton;

namespace JUNBEOM.Player
{
    public class PlayerEventManager : Singleton<PlayerEventManager>
    {
        #region Properties

        public float CurrentLightRatio { get; private set; } = 1.0f;

        #endregion

        #region Events
        public Action<GameObject, GameObject> OnInteractionRequested;

        public Action OnKeyCollected;
        public Action<bool> OnKeyStateChanged;
        public Action<Action<bool>> OnKeyStateRequested;

        public Action<float> OnLightRatioChanged;
        #endregion

        #region Interaction Events

        //public void BroadcastInteraction(
        //    GameObject target,
        //    GameObject interactor)
        //{
        //    OnInteractionRequested?.Invoke(
        //        target,
        //        interactor);
        //}

        #endregion

        #region Key Events

        //public void NotifyKeyCollected()
        //{
        //    OnKeyCollected?.Invoke();
        //}

        //public void BroadcastKeyState(bool hasKey)
        //{
        //    OnKeyStateChanged?.Invoke(hasKey);
        //}

        //public void RequestKeyState(Action<bool> response)
        //{
        //    OnKeyStateRequested?.Invoke(response);
        //}

        #endregion

        #region Flashlight Events

        //public void BroadcastLightRatio(float lightRatio)
        //{
        //    CurrentLightRatio =
        //        Mathf.Clamp01(lightRatio);
        //    OnLightRatioChanged?.Invoke(
        //        CurrentLightRatio);
        //}

        #endregion
    }
}
