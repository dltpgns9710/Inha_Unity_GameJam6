using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyExpandingImage : AnomalyBase
{
    [Header("Target Settings")]
    [Tooltip("플레이어가 들어갈 감지 영역 (씬에 있는 Box Collider 2D 오브젝트)")]
    [SerializeField] private Collider2D _triggerZone;
    [Tooltip("크기가 변할 이미지 오브젝트 (UI Image 또는 Sprite Renderer)")]
    [SerializeField] private GameObject _targetImage;

    [Header("Scale Settings")]
    [Tooltip("이미지의 원래(최소) 크기 배율")]
    [SerializeField] private Vector3 _minScale = Vector3.one;
    [Tooltip("이미지가 도달할 목표(최대) 크기 배율")]
    [SerializeField] private Vector3 _maxScale = new Vector3(5f, 5f, 1f);
    [Tooltip("초당 크기 변화 속도")]
    [SerializeField] private float _expandSpeed = 1f;

    [Header("Behavior Settings")]
    [Tooltip("체크하면 플레이어가 영역을 벗어났을 때 이미지가 다시 작아집니다.")]
    [SerializeField] private bool _shrinkWhenLeave = true;

    [Header("놀라는 사운드")]
    [SerializeField] private AudioClip _scream;

    // 상태 변수
    private Transform _playerTransform;
    private bool _isAnomalyApplied = false;
    private bool _isExpanding = false;     

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }

        // 초기 크기 설정
        if (_targetImage != null)
        {
            _targetImage.transform.localScale = _minScale;
            _targetImage.SetActive(false);
        }
    }

    public override void Apply()
    {
        _isAnomalyApplied = true;

        if (_targetImage != null)
        {
            _targetImage.transform.localScale = _minScale;
            _targetImage.SetActive(false);
        }
    }

    public override void Remove()
    {
        // 이상현상 해제
        _isAnomalyApplied = false;
        _isExpanding = false;

        // 이미지를 원래 크기로 되돌리고 끕니다.
        if (_targetImage != null)
        {
            _targetImage.transform.localScale = _minScale;
            _targetImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_isAnomalyApplied || _triggerZone == null || _targetImage == null || _playerTransform == null)
        {
            return;
        }
        int count = 0;
        bool isPlayerInside = _triggerZone.OverlapPoint(_playerTransform.position);

        if (isPlayerInside)
        {
            if (!_isExpanding)
            {
                _isExpanding = true;
                _targetImage.SetActive(true);
                if (count == 0)
                    SoundManager.Instance.PlaySfx(_scream);
                ++count;
            }

            ScaleImage(_maxScale);
        }
        else
        {
            if (_isExpanding)
            {
                _isExpanding = false;
            }
            

            if (_shrinkWhenLeave)
            {
                ScaleImage(_minScale);

                if (_targetImage.transform.localScale == _minScale)
                {
                    _targetImage.SetActive(false);
                }
            }
        }
    }


    private void ScaleImage(Vector3 targetScale)
    {
        _targetImage.transform.localScale = Vector3.MoveTowards(
            _targetImage.transform.localScale,
            targetScale,
            _expandSpeed * Time.deltaTime
        );
    }
}