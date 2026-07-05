using System;
using UnityEngine;

public class HelperCommandBroadcaster : MonoBehaviour
{
    public event Action DetectAnomalyRequested;
    public void RequestDetectAnomaly()
    {
        DetectAnomalyRequested?.Invoke();
    }
}
