using UnityEngine;
using SEHOON.GameSystem;
using JUNBEOM.Player;

public class AnomalyPlayerSpeed : AnomalyBase
{
    [Header("Speed Settings")]
    [Tooltip("플레이어 속도 배율 (0.5 = 절반 속도, 2.0 = 두 배 속도)")]
    [SerializeField] private float _speedMultiplier = 0.5f;

    [Header("Player Movement")]
    [SerializeField] private PlayerMovement _playerMovement;

    public override void Apply()
    {
        if (_playerMovement == null)
            _playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (_playerMovement != null)
            _playerMovement.SetSpeedMultiplier(_speedMultiplier);
    }

    public override void Remove()
    {
        if (_playerMovement != null)
            _playerMovement.SetSpeedMultiplier(1.0f);
    }
}