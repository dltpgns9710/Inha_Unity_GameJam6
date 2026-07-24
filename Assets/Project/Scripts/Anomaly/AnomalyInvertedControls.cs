using UnityEngine;
using SEHOON.GameSystem;
using JUNBEOM.Player;

public class AnomalyInvertedControls : AnomalyBase
{
    [Header("플레이어")]
    [SerializeField] private PlayerMovement _playerMovement;

    public override void Apply()
    {
        if (_playerMovement == null)
        {
            _playerMovement = FindFirstObjectByType<PlayerMovement>();
        }

        if (_playerMovement != null)
        {
            _playerMovement.ControlMultiplier = -1.0f;
        }
    }

    public override void Remove()
    {
        if (_playerMovement != null)
        {
            _playerMovement.ControlMultiplier = 1.0f;
        }
    }
}