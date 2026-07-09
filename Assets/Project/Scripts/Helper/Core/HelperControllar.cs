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
        [SerializeField] private float _walkSpeed = 3.0f;
        [SerializeField] private float _runSpeed = 6.0f;
        [SerializeField] private float _runDistance = 4.0f;
        [SerializeField] private float _followDistance = 1.5f;
        [SerializeField] private float _sleepDelay = 5.0f;

        [Header("References")]
        [SerializeField] private HelperCommandBroadcaster _commandBroadcaster;
        [SerializeField] private HelperConfig _config;
        #endregion

        #region Private Fields
        private EHelperState _currentState;
        private bool _isWaitAnimationEnd;
        private HelperMovement _movement;
        private HelperAnimation _helperAnimation;
        private HelperAbility _ability;
        private float _waitElapsedTime;
        #endregion

        #region Properties
        public HelperConfig Config => _config;
        public Vector2 PlayerPosition => _playerTransform != null ? _playerTransform.position : transform.position;
        public float FollowDistance => _followDistance;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

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

            ApplyConfig();
            _movement.Initialize(_walkSpeed, _runSpeed, _runDistance);
            _ability?.Initialize(this);

            ChangeState(EHelperState.Follow);

            Debug.Assert(_playerTransform != null, "Player Transform is not connected.");
            Debug.Assert(_movement != null, "HelperMovement is not connected.");
            Debug.Assert(_helperAnimation != null, "HelperAnimation is not connected.");
        }

        private void OnEnable()
        {
            if (_commandBroadcaster == null)
            {
                return;
            }

            _commandBroadcaster.DetectAnomalyRequested += RequestUseAbility;
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

            _commandBroadcaster.DetectAnomalyRequested -= RequestUseAbility;
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
            RequestUseAbility(searchPosition);
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
        private void ApplyConfig()
        {
            if (_config == null)
            {
                return;
            }

            _walkSpeed = _config.WalkSpeed;
            _runSpeed = _config.RunSpeed;
            _runDistance = _config.RunDistance;
            _followDistance = _config.FollowDistance;
            _sleepDelay = _config.SleepDelay;
        }

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
                case EHelperState.Sleep:
                    UpdateSleep();
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
            _movement.Follow(PlayerPosition, _followDistance);
        }

        private void UpdateWait()
        {
            _waitElapsedTime += Time.deltaTime;

            if (_waitElapsedTime >= _sleepDelay)
            {
                ChangeState(EHelperState.Sleep);
            }
        }

        private void UpdateSleep()
        {
        }
        #endregion
    }
}
