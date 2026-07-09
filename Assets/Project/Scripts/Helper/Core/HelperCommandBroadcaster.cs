using System;
using UnityEngine;

namespace TAEWOOK.Helper.Core
{
    public class HelperCommandBroadcaster : MonoBehaviour
    {
        public event Action<Vector2> DetectAnomalyRequested;
        public event Action OnWaitRequested;
        public void RequestDetectAnomaly(Vector2 searchPosition)
        {
            DetectAnomalyRequested?.Invoke(searchPosition);
        }

        public void RequestWait()
        {
            OnWaitRequested?.Invoke();
        }
    }
}

