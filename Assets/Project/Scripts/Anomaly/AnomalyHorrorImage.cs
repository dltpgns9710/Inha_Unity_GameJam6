using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnomalyHorrorImage : AnomalyBase
{
    [Header("Trigger Image Settings")]
    [Tooltip("플레이어가 들어갈 감지 영역 (씬에 있는 Box Collider 2D 오브젝트를 드래그)")]
    [SerializeField] private Collider2D _triggerZone;
    [Tooltip("영역에 들어갔을 때 나타나게 할 이미지")]
    [SerializeField] private GameObject _targetImage;

    private Transform _playerTransform;
    private bool _isTriggerReady = false;
    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }
    public override void Apply()
    {
        _isTriggerReady = true;
    }
    public override void Remove()
    {
        _isTriggerReady = false; 

        if (_targetImage != null)
        {
            _targetImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isTriggerReady && _triggerZone != null && _playerTransform != null)
        {
            if (_triggerZone.OverlapPoint(_playerTransform.position))
            {
                if (_targetImage != null)
                {
                    _targetImage.SetActive(true);
                }
            }
        }
    }
}
