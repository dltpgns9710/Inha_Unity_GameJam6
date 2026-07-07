using UnityEngine;

namespace TAEWOOK.Helper
{
    [RequireComponent(typeof(HelperMovement))]
    [RequireComponent(typeof(HelperDetector))]
    [RequireComponent(typeof(HelperAnimation))]
    public class HelperControllar : MonoBehaviour
    {

        #region Serialized Fields
        [Header("Helper Settings")]
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private float _walkSpeed = 3.0f;
        [SerializeField] private float _runSpeed = 6.0f;
        [SerializeField] private float _runDistance = 4.0f;
        [SerializeField] private float _followDistance = 1.5f;
        [SerializeField] private float _SleepDelay = 5.0f;

        [Header("Detect")]
        [SerializeField] private float _commandSearchDistance = 6.0f;
        [SerializeField] private float _commandSearchArriveDistance = 0.5f;
        [SerializeField] private float _anomalyArriveDistance = 1.5f;
        [SerializeField] private LayerMask _anomalyLayer;

        [Header("Detect Feedback")]
        [SerializeField] private GameObject _exclamationIconPrefab;
        [SerializeField] private Vector3 _exclamationIconOffset = new Vector3(0, 1.5f, 0);
        [SerializeField] private float _exclamationIconDuration = 1.0f;

        [Header("References")]
        [SerializeField] private HelperCommandBroadcaster _commandBroadcaster;
        #endregion


        #region Private Fields
        private EHelperState _currentState;
        private bool _isWaitAnimationEnd;
        private HelperMovement _movement;
        private HelperDetector _detector;
        private HelperAnimation _helperAnimation;
        private Transform _targetAnomaly;
        private Vector2 _commandSearchPosition;
        private Vector2 _commandMovePosition;
        private float _waitElapsedTime;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            _movement = GetComponent<HelperMovement>();
            _detector = GetComponent<HelperDetector>();
            _helperAnimation = GetComponent<HelperAnimation>();

            if (_movement == null)
            {
                _movement = gameObject.AddComponent<HelperMovement>();
            }

            if (_detector == null)
            {
                _detector = gameObject.AddComponent<HelperDetector>();
            }

            if (_helperAnimation == null)
            {
                _helperAnimation = gameObject.AddComponent<HelperAnimation>();
            }

            _movement.Initialize(_walkSpeed, _runSpeed, _runDistance);
            _detector.Initialize(_commandSearchDistance, _anomalyLayer);
            ChangeState(EHelperState.Follow);

            Debug.Assert(_playerTransform != null, "Player Transform이 연결되지 않았습니다.");
            Debug.Assert(_movement != null, "HelperMovement가 연결되지 않았습니다.");
            Debug.Assert(_detector != null, "HelperDetector가 연결되지 않았습니다.");
            Debug.Assert(_helperAnimation != null, "HelperAnimation이 연결되지 않았습니다.");
        }

        private void OnEnable()
        {
            _commandBroadcaster.DetectAnomalyRequested += RequestDetectAnomaly;
            _commandBroadcaster.OnWaitRequested += RequestWait;
        }

        private void Update()
        {
            UpdateState();
        }

        private void OnDisable()
        {
            _commandBroadcaster.DetectAnomalyRequested -= RequestDetectAnomaly;
            _commandBroadcaster.OnWaitRequested -= RequestWait;
        }
        #endregion

        #region Public Methods
        public void RequestDetectAnomaly(Vector2 searchPosition)
        {
            if (_currentState == EHelperState.DetectAnomaly ||
                _currentState == EHelperState.MoveToAnomaly ||
                _currentState == EHelperState.Alert ||
                _currentState == EHelperState.ReturnToPlayer)
            {
                return;
            }

            _commandSearchPosition = searchPosition;
            _commandMovePosition = new Vector2(searchPosition.x, transform.position.y);
            ChangeState(EHelperState.DetectAnomaly);
        }

        public void OnBarkAnimationEnd()
        {
            _targetAnomaly = null;
            ChangeState(EHelperState.ReturnToPlayer);
        }

        public void RequestWait()
        {
            if (_currentState == EHelperState.Sleep)
            {
                ChangeState(EHelperState.Follow);
                return;
            }

            if (_currentState == EHelperState.Wait)
            {
                if (!_isWaitAnimationEnd)
                {
                    return;
                }
                ChangeState(EHelperState.Follow);
                return;
            }
            if (_currentState == EHelperState.Follow)
            {
                _isWaitAnimationEnd = false;
                ChangeState(EHelperState.Wait);
            }

        }
        public void OnWaitAnimationEnd()
        {
            _isWaitAnimationEnd = true;
        }
        #endregion

        #region Private Methods
        private void UpdateState()
        {
            switch (_currentState)
            {
                case EHelperState.Idle:
                    UpdateIdle();
                    break;
                case EHelperState.Follow:
                    UpdateFollow();
                    break;
                case EHelperState.Alert:
                    UpdateAlert();
                    break;
                case EHelperState.Wait:
                    UpdateWait();
                    break;
                case EHelperState.Sleep:
                    UpdateSleep();
                    break;
                case EHelperState.DetectAnomaly:
                    UpdateDetectAnomaly();
                    break;
                case EHelperState.MoveToAnomaly:
                    UpdateMoveToAnomaly();
                    break;
                case EHelperState.ReturnToPlayer:
                    UpdateReturnToPlayer();
                    break;
            }
        }

        #region State Transition Logic    
        private void OnEnterState(EHelperState state)
        {
            switch (state)
            {
                case EHelperState.Idle:
                    break;
                case EHelperState.Follow:
                    break;
                case EHelperState.Alert:
                    _movement.Stop();
                    _helperAnimation.PlayBark();
                    break;
                case EHelperState.Wait:
                    _movement.Stop();
                    _helperAnimation.SetWaiting(true);
                    _waitElapsedTime = 0f;
                    break;
                case EHelperState.Sleep:
                    _helperAnimation.SetWaiting(false);
                    _helperAnimation.SetSleeping(true);
                    break;
                case EHelperState.DetectAnomaly:
                    break;
                case EHelperState.MoveToAnomaly:
                    break;
                case EHelperState.ReturnToPlayer:
                    break;
            }
        }

        private void OnExitState(EHelperState state)
        {
            switch (state)
            {
                case EHelperState.Idle:
                    break;
                case EHelperState.Follow:
                    break;
                case EHelperState.Alert:
                    break;
                case EHelperState.Wait:
                    _helperAnimation.SetWaiting(false);
                    break;
                case EHelperState.Sleep:
                    _helperAnimation.SetSleeping(false);
                    break;
                case EHelperState.DetectAnomaly:
                    break;
                case EHelperState.MoveToAnomaly:
                    break;
                case EHelperState.ReturnToPlayer:
                    break;
            }
        }
        private void ChangeState(EHelperState nextState)
        {
            if (_currentState == nextState)
            {
                return;
            }

            OnExitState(_currentState);
            _currentState = nextState;
            OnEnterState(_currentState);
        }
        #endregion

        private void UpdateIdle()
        {
            _movement.Stop();
        }

        private void UpdateFollow()
        {
            _movement.Follow(_playerTransform.position, _followDistance);
        }

        private void UpdateDetectAnomaly()
        {
            if (!_movement.MoveToTarget(_commandMovePosition, _commandSearchArriveDistance))
            {
                return;
            }

            _targetAnomaly = _detector.FindNearestAnomaly(_commandSearchPosition);

            if (_targetAnomaly == null)
            {
                ChangeState(EHelperState.ReturnToPlayer);
                return;
            }
            ShowExclamtionIcon(transform.position);
            ChangeState(EHelperState.MoveToAnomaly);
        }

        private void UpdateMoveToAnomaly()
        {
            if (_targetAnomaly == null)
            {
                ChangeState(EHelperState.ReturnToPlayer);
                return;
            }

            if (_movement.MoveToTarget(_targetAnomaly.position, _anomalyArriveDistance))
            {
                ChangeState(EHelperState.Alert);
            }
        }

        private void UpdateAlert()
        {

        }

        private void UpdateReturnToPlayer()
        {
            if (_movement.Follow(_playerTransform.position, _followDistance))
            {
                ChangeState(EHelperState.Follow);
            }
        }

        private void UpdateWait()
        {
            _waitElapsedTime += Time.deltaTime;
            if (_waitElapsedTime >= _SleepDelay)
            {
                ChangeState(EHelperState.Sleep);
            }
        }

        private void UpdateSleep()
        {

        }

        private void ShowExclamtionIcon(Vector3 position)
        {
            if(_exclamationIconPrefab == null)
            {
                return;
            }

            GameObject icon = Instantiate(
                _exclamationIconPrefab,
                position + _exclamationIconOffset,
                Quaternion.identity);

            Destroy(icon, _exclamationIconDuration);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            Gizmos.DrawWireSphere(
                _commandSearchPosition,
                _commandSearchDistance);
        }
        #endregion
    }
}

