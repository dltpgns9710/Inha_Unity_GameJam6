using System.Collections;
using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyJumpscareOnce : AnomalyBase
{
    [Header("Jumpscare Settings")]
    [Tooltip("플레이어가 닿을 감지 영역 (씬에 있는 Collider 2D 오브젝트)")]
    [SerializeField] private Collider2D _triggerZone;
    [Tooltip("갑툭튀할 공포 이미지 오브젝트")]
    [SerializeField] private GameObject _targetImage;
    [Tooltip("이미지가 화면에 떠 있을 시간(초)")]
    [SerializeField] private float _displayDuration = 1.5f;

    [Header("놀라는 사운드")]
    [SerializeField] private AudioClip _scream;


    private Transform _playerTransform;
    private bool _isTriggerReady = false;
    private bool _hasTriggered = false; 

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
        _hasTriggered = false;
    }

    public override void Remove()
    {
        _isTriggerReady = false;
        _hasTriggered = false;

        if (_targetImage != null)
        {
            _targetImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (_isTriggerReady && !_hasTriggered && _triggerZone != null && _playerTransform != null)
        {
            if (_triggerZone.OverlapPoint(_playerTransform.position))
            {
                StartCoroutine(ShowJumpscareRoutine());
            }
        }
    }

    private IEnumerator ShowJumpscareRoutine()
    {
        _hasTriggered = true;
        int count = 0;
        if (_targetImage != null)
        {
            _targetImage.SetActive(true);
            if (count == 0)
                SoundManager.Instance.PlaySfx(_scream);
            ++count;
        }

        // 설정한 시간만큼 대기
        yield return new WaitForSeconds(_displayDuration);

        if (_targetImage != null)
        {
            _targetImage.SetActive(false);
        }
    }
}