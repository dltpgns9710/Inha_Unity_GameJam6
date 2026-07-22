using System;
using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyPlayerAnimatorOff : AnomalyBase
{
    [Header("플레이어 애니메이터")]
    [SerializeField] private GameObject _playerObject;

    private Animator _playerAnimator;

    private void Awake()
    {
        if (_playerObject != null)
        {
            _playerAnimator = _playerObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if(_playerObject != null) _detectData.AnomalyPos = _playerObject.transform.position;
    }

    public override void Apply()
    {
        if (_playerAnimator != null)
        {
            _playerAnimator.enabled = false;
        }
    }
    
    public override void Remove()
    {
        if (_playerAnimator != null)
        {
            _playerAnimator.enabled = true;
        }
    }
}
