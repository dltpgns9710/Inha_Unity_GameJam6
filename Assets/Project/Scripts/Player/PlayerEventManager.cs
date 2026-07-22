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
    }
}
