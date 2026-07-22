using UnityEngine;

namespace SEHOON.GameSystem
{
    public class AnomalySliding : AnomalyBase
    {
        [Header("Anomaly Action Settings")]
        [Tooltip("숨어있다가 튀어나올 오브젝트")]
        [SerializeField] private Transform _hiddenObject;

        [Tooltip("플레이어 감지를 담당할 ObjectCollider 연결")]
        [SerializeField] private ObjectCollider _objectCollider;

        [Tooltip("오브젝트가 이동할 로컬 오프셋 (기본값: 좌측으로 1만큼 이동)")]
        [SerializeField] private Vector3 _slideOffset = new Vector3(-1f, 0f, 0f);

        [Tooltip("오브젝트가 이동하는 속도")]
        [SerializeField] private float _moveSpeed = 5f;

        [Header("놀라는 사운드")]
        [SerializeField] private AudioClip _scream;

        private Vector3 _originalPosition;
        private Vector3 _targetPosition;
        private Vector3 _currentDestination;

        private bool _isAnomalyActive = false;

        private int _count = 0;

        private void Awake()
        {
            if (_hiddenObject != null)
            {
                // 위치 계산 및 저장
                _originalPosition = _hiddenObject.localPosition;
                _targetPosition = _originalPosition + _slideOffset;
                _currentDestination = _originalPosition;
            }
        }

        public override void Apply()
        {
            _isAnomalyActive = true;

            if (_hiddenObject != null)
            {
                // 초기 위치로 되돌리고 활성화
                _hiddenObject.localPosition = _originalPosition;
                _currentDestination = _originalPosition;
                _hiddenObject.gameObject.SetActive(true);
            }
        }

        public override void Remove()
        {
            _isAnomalyActive = false;

            if (_hiddenObject != null)
            {
                // 비활성화(끄기)
                _hiddenObject.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (_hiddenObject == null || _objectCollider == null || !_isAnomalyActive)
            {
                return;
            }

            if(!_objectCollider.Detect())
                _count = 0;
            if (_objectCollider.Detect())
            {


                if (_count == 0)
                    SoundManager.Instance.PlaySfx(_scream);
                ++_count;
                _currentDestination = _targetPosition;
            }
            else
            {
                _currentDestination = _originalPosition;
            }

            _hiddenObject.localPosition = Vector3.Lerp(
                _hiddenObject.localPosition,
                _currentDestination,
                Time.deltaTime * _moveSpeed
            );
        }
    }
}