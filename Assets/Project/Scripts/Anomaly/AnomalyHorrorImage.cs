using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AnomalyHorrorImage : AnomalyBase
{

    //[Header("Horror Image")]
    //[SerializeField] private GameObject _image;

    //private bool _canImageLoad = false;
    //public override void Apply()
    //{
    //    if (_image != null)
    //        _image.SetActive(true);
    //}

    //public override void Remove()
    //{
    //    if (_image != null)
    //        _image.SetActive(false);
    //}
    [Header("Trigger Image Settings")]
    [Tooltip("플레이어가 들어갈 감지 영역 (씬에 있는 Box Collider 2D 오브젝트를 드래그)")]
    [SerializeField] private Collider2D _triggerZone;
    [Tooltip("영역에 들어갔을 때 나타나게 할 이미지")]
    [SerializeField] private GameObject _targetImage;
    // 플레이어의 위치를 추적할 변수
    private Transform _playerTransform;
    private bool _isTriggerReady = false;
    private void Start()
    {
        // 게임 시작 시 "Player" 태그를 가진 오브젝트를 자동으로 찾아냅니다.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
    }
    public override void Apply()
    {
        _isTriggerReady = true; // 함정 장전
    }
    public override void Remove()
    {
        _isTriggerReady = false; // 함정 해제

        if (_targetImage != null)
        {
            _targetImage.SetActive(false);
        }
    }
    // 매 프레임마다 플레이어의 위치를 검사합니다.
    private void Update()
    {
        // 1. 이상현상이 켜져 있고
        // 2. 트리거 영역과 플레이어를 성공적으로 찾았다면
        if (_isTriggerReady && _triggerZone != null && _playerTransform != null)
        {
            // 플레이어의 현재 위치가 트리거 영역(_triggerZone) 안에 포함되어 있는지 수학적으로 검사
            if (_triggerZone.OverlapPoint(_playerTransform.position))
            {
                if (_targetImage != null)
                {
                    _targetImage.SetActive(true); // 이미지 나타나기
                }
            }
        }
    }
}
