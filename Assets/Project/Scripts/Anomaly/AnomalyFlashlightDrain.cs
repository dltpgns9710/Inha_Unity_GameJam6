using UnityEngine;
using SEHOON.GameSystem;
using JUNBEOM.Player;

public class AnomalyFlashlightDrain : AnomalyBase
{
    [Header("Flashlight Drain Settings")]
    [Tooltip("배터리 소모 속도 배율 (2.0 = 2배 빨리 닳음)")]
    [SerializeField] private float _drainMultiplier = 2.0f;

    [Header("플레이어")]
    [SerializeField] private PlayerFlashlight _playerFlashlight;

    public override void Apply()
    {
        if (_playerFlashlight == null)
            _playerFlashlight = FindFirstObjectByType<PlayerFlashlight>();

        if (_playerFlashlight != null)
            _playerFlashlight.DrainMultiplier = _drainMultiplier;
    }

    public override void Remove()
    {
        if (_playerFlashlight != null)
            _playerFlashlight.DrainMultiplier = 1.0f;
    }
}
