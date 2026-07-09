using UnityEngine;
using TAEWOOK.Helper.Core;
using TAEWOOK.Helper.Data;
using TAEWOOK.Helper.Detection;

namespace TAEWOOK.Helper.Ability
{
    [RequireComponent(typeof(HelperDetector))]
    public class DetectHelperAbility : HelperAbility
    {
        private enum EDetectAbilityState
        {
            None,
            MoveToSearchPosition,
            MoveToAnomaly,
            Alert,
            ReturnToPlayer,
        }    

        #region Private Fields
        private DetectHelperConfig _config;
        private HelperMovement _movement;      
        private HelperAnimation _helperAnimation;
        private HelperDetector _detector;
        private Transform _targetAnomaly;
        private Vector2 _commandSearchPosition;
        private Vector2 _commandMovePosition;
        private EDetectAbilityState _currentState;
        #endregion

        #region Public Methods
        public override bool IsActive => _currentState != EDetectAbilityState.None;

        public override void Initialize(HelperControllar helper)
        {
            base.Initialize(helper);

            if (_config == null)
            {
                _config = helper.Config as DetectHelperConfig;
            }

            _movement = GetComponent<HelperMovement>();
            _helperAnimation = GetComponent<HelperAnimation>();
            _detector = GetComponent<HelperDetector>();

            if (_detector == null)
            {
                _detector = gameObject.AddComponent<HelperDetector>();
            }

            if (_config != null)
            {
                _detector.Initialize(_config.CommandSearchDistance, _config.AnomalyLayer);
            }
        }

        public override bool CanUseAbility()
        {
            return Helper != null &&
                   _config != null &&
                   _movement != null &&
                   _helperAnimation != null &&
                   _detector != null &&
                   !IsActive;
        }

        public override void UseAbility(Vector2 targetPosition)
        {
            if (!CanUseAbility())
            {
                return;
            }

            _commandSearchPosition = targetPosition;
            _commandMovePosition = new Vector2(targetPosition.x, transform.position.y);
            _targetAnomaly = null;
            _currentState = EDetectAbilityState.MoveToSearchPosition;
        }

        public override void TickAbility()
        {
            switch (_currentState)
            {
                case EDetectAbilityState.MoveToSearchPosition:
                    UpdateMoveToSearchPosition();
                    break;
                case EDetectAbilityState.MoveToAnomaly:
                    UpdateMoveToAnomaly();
                    break;
                case EDetectAbilityState.Alert:
                    break;
                case EDetectAbilityState.ReturnToPlayer:
                    UpdateReturnToPlayer();
                    break;
            }
        }

        public override void OnBarkAnimationEnd()
        {
            if (_currentState != EDetectAbilityState.Alert)
            {
                return;
            }

            _targetAnomaly = null;
            _currentState = EDetectAbilityState.ReturnToPlayer;
        }
        #endregion

        #region Private Methods
        private void UpdateMoveToSearchPosition()
        {
            if (!_movement.MoveToTarget(_commandMovePosition, _config.CommandSearchArriveDistance))
            {
                return;
            }

            _targetAnomaly = _detector.FindNearestAnomaly(_commandSearchPosition);

            if (_targetAnomaly == null)
            {
                _currentState = EDetectAbilityState.ReturnToPlayer;
                return;
            }

            ShowExclamationIcon();
            _currentState = EDetectAbilityState.MoveToAnomaly;
        }

        private void UpdateMoveToAnomaly()
        {
            if (_targetAnomaly == null)
            {
                _currentState = EDetectAbilityState.ReturnToPlayer;
                return;
            }

            if (_movement.MoveToTarget(_targetAnomaly.position, _config.AnomalyArriveDistance))
            {
                _movement.Stop();
                _helperAnimation.PlayBark();
                _currentState = EDetectAbilityState.Alert;
            }
        }

        private void UpdateReturnToPlayer()
        {
            if (_movement.Follow(Helper.PlayerPosition, Helper.FollowDistance))
            {
                FinishAbility();
            }
        }

        private void FinishAbility()
        {
            _targetAnomaly = null;
            _currentState = EDetectAbilityState.None;
            Helper.ReturnToFollowState();
        }

        private void ShowExclamationIcon()
        {
            if (_config.ExclamationIconPrefab == null)
            {
                return;
            }

            GameObject icon = Instantiate(_config.ExclamationIconPrefab, transform);
            icon.transform.localPosition = _config.ExclamationIconOffset;
            icon.transform.localRotation = Quaternion.identity;

            Destroy(icon, _config.ExclamationIconDuration);
        }
        #endregion
    }
}
