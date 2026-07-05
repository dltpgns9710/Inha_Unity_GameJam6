using System;
using UnityEngine;

public class HelperCommandBroadcaster : MonoBehaviour
{
    public event Action DetectAnomalyRequested;
    public event Action OnWaitRequested;
    public void RequestDetectAnomaly()
    {
        DetectAnomalyRequested?.Invoke();
    }
    
    public void RequestWait()
    {
        OnWaitRequested?.Invoke();
    }
}
