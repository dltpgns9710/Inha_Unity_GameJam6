using UnityEngine;
using TAEWOOK.Helper.Ability;
using TAEWOOK.Helper.Data;

namespace TAEWOOK.Helper.Core
{
    [RequireComponent(typeof(HelperMovement))]
    [RequireComponent(typeof(HelperAnimation))]
    public class HelperControllar : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Helper State Settings")]      
        [SerializeField] private Transform _playerTransform;                                   

        [Header("References")]
        [SerializeField] private HelperCommandBroadcaster _commandBroadcaster;
        [SerializeField] private HelperConfig _config;
        [SerializeField] private int _maxCount;
        #endregion

        #region Private Fields      
        private EHelperState _currentState;
        private bool _isWaitAnimationEnd;
        private HelperMovement _movement;
        private HelperAnimation _helperAnimation;
        private HelperAbility _ability;
        private float _waitElapsedTime;
        private Collider2D _playerCollider;
        private Collider2D _helperCollider;
        private int _detectChance;
        private bool hasDetectChance;
        #endregion

        #region Properties
        public HelperConfig Config => _config;
        public Vector2 PlayerPosition => _playerTransform != null ? _playerTransform.position : transform.position;
        public float FollowDistance => _config.FollowDistance;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            _playerCollider = _playerTransform.GetComponent<Collider2D>();
            _helperCollider = GetComponent<Collider2D>();

            if (_playerTransform == null && player != null)
            {
                _playerTransform = player.transform;
            }

            _movement = GetComponent<HelperMovement>();
            _helperAnimation = GetComponent<HelperAnimation>();
            _ability = GetComponent<HelperAbility>();            

            if (_movement == null)
            {
                _movement = gameObject.AddComponent<HelperMovement>();
            }

            if (_helperAnimation == null)
            {
                _helperAnimation = gameObject.AddComponent<HelperAnimation>();
            }
           
            _movement.Initialize(_config.WalkSpeed, _config.RunSpeed, _config.RunDistance);
            _ability?.Initialize(this);

            ChangeState(EHelperState.Follow);

            Debug.Assert(_playerTransform != null, "Player Transform is not connected.");
            Debug.Assert(_movement != null, "HelperMovement is not connected.");
            Debug.Assert(_helperAnimation != null, "HelperAnimation is not connected.");
        }
        private void Start()
        {
            _detectChance = 0;
        }

        private void OnEnable()
        {
            if (_commandBroadcaster == null)
            {
                return;
            }

            _commandBroadcaster.DetectAnomalyRequested += RequestDetectAnomaly;
            _commandBroadcaster.OnWaitRequested += RequestWait;
        }

        private void Update()
        {
            if (_ability != null && _ability.IsActive)
            {
                _ability.TickAbility();
                return;
            }

            UpdateState();
        }

        private void OnDisable()
        {
            if (_commandBroadcaster == null)
            {
                return;
            }

            _commandBroadcaster.DetectAnomalyRequested -= RequestDetectAnomaly;
            _commandBroadcaster.OnWaitRequested -= RequestWait;
        }
        #endregion

        #region Public Methods
        public void RequestUseAbility(Vector2 targetPosition)
        {
            if (_ability == null || !_ability.CanUseAbility())
            {
                return;
            }

            _ability.UseAbility(targetPosition);
        }

        public void RequestDetectAnomaly(Vector2 searchPosition)
        {
            if(_currentState == EHelperState.Wait || _currentState == EHelperState.Sleep)
            {
                return;
            }

            if(_detectChance >= _maxCount)
            {
                return;
            }

            int wallLayer = LayerMask.GetMask("Wall");

            Collider2D wall = Physics2D.OverlapPoint(
                searchPosition,
                wallLayer);

            if (wall != null)
            {
                return;
            }

            RequestUseAbility(searchPosition);
            _detectChance++;
        }

        public void RequestWait()
        {
            if (_ability != null && _ability.IsActive)
            {
                return;
            }

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

        public void OnBarkAnimationEnd()
        {
            _ability?.OnBarkAnimationEnd();
        }

        public void ReturnToFollowState()
        {
            ChangeState(EHelperState.Follow);
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
                case EHelperState.Wait:
                    UpdateWait();
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

        private void OnEnterState(EHelperState state)
        {
            switch (state)
            {
                case EHelperState.Wait:
                    _movement.Stop();
                    _helperAnimation.SetWaiting(true);
                    _waitElapsedTime = 0f;
                    break;
                case EHelperState.Sleep:
                    _helperAnimation.SetWaiting(false);
                    _helperAnimation.SetSleeping(true);
                    break;
            }
        }

        private void OnExitState(EHelperState state)
        {
            switch (state)
            {
                case EHelperState.Wait:
                    _helperAnimation.SetWaiting(false);
                    break;
                case EHelperState.Sleep:
                    _helperAnimation.SetSleeping(false);
                    break;
            }
        }

        private void UpdateIdle()
        {
            _movement.Stop();
        }

        private void UpdateFollow()
        {
            TeleportToPlayer();
            _movement.Follow(PlayerPosition, _config.FollowDistance);
        }

        private void UpdateWait()
        {
            _waitElapsedTime += Time.deltaTime;

            if (_waitElapsedTime >= _config.SleepDelay)
            {
                ChangeState(EHelperState.Sleep);
            }
        }        

        private void TeleportToPlayer()
        {
            float xDistance = Mathf.Abs(PlayerPosition.x - transform.position.x);
            float yDistance = Mathf.Abs(PlayerPosition.y - transform.position.y);
            
            if(xDistance <= 40.0f && yDistance <= 10.0f)
            {
                return;
            }

            float targetY = PlayerPosition.y;

            if(_playerCollider != null && _helperCollider != null)
            {
                float footDiff = _playerCollider.bounds.min.y - _helperCollider.bounds.min.y;
                targetY = transform.position.y + footDiff;
            }
            
            transform.position = new Vector3(
                PlayerPosition.x - _config.FollowDistance,
                targetY,
                transform.position.z);                 
        }
        #endregion
    }
}
