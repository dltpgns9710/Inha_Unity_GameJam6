using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyEye : AnomalyBase
{
    [SerializeField] private EyeSpawner _eyeSpawner;

    private void Awake()
    {
        if (_eyeSpawner == null)
        {
            _eyeSpawner = GetComponentInChildren<EyeSpawner>(true);
        }

        _eyeSpawner?.SetAnomalyActive(false);
    }

    public override void Apply()
    {
        if (_eyeSpawner == null)
        {            
            return;
        }

        _eyeSpawner.SetAnomalyActive(true);
    }

    public override void Remove()
    {
        _eyeSpawner?.SetAnomalyActive(false);
    }
}
