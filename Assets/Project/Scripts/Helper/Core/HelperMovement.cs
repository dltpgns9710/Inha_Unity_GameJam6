using UnityEngine;

namespace TAEWOOK.Helper.Core
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Animator))]
    public class HelperMovement : MonoBehaviour
    {
        #region Private Fields
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private float _walkSpeed;
        private float _runSpeed;
        private float _runDistance;
        private bool _isRunning;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            EnsureReferences();

            Debug.Assert(_spriteRenderer != null, "SpriteRenderer가 연결되지 않았습니다.");
            Debug.Assert(_animator != null, "Animator가 연결되지 않았습니다.");
        }
        #endregion

        #region Public Methods
        public void Initialize(float walkSpeed, float runSpeed, float runDistance)
        {
            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
            _runDistance = runDistance;
        }

        public bool Follow(Vector2 targetPosition, float followDistance)
        {
            return MoveToTarget(targetPosition, followDistance);
        }

        public bool MoveToTarget(Vector2 targetPosition, float stopDistance)
        {
            EnsureReferences();

            float distance = Vector2.Distance(transform.position, targetPosition);
            bool isMoving = distance > stopDistance;

            UpdateRunState(distance);
            SetMoveAnimation(isMoving);

            if (!isMoving)
            {
                return true;
            }

            float moveSpeed = _isRunning ? _runSpeed : _walkSpeed;

            FlipByMoveDirection(targetPosition);

            transform.position = Vector2.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime);

            return false;
        }

        public void Stop()
        {
            EnsureReferences();
            _animator.SetBool("IsMoving", false);
            _animator.SetBool("IsRunning", false);
        }
        #endregion

        #region Private Methods
        private void EnsureReferences()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            if (_animator == null)
            {
                _animator = GetComponent<Animator>();
            }
        }

        private void SetMoveAnimation(bool isMoving)
        {
            _animator.SetBool("IsMoving", isMoving);
            _animator.SetBool("IsRunning", _isRunning);
        }

        private void UpdateRunState(float distance)
        {
            if (distance >= _runDistance)
            {
                _isRunning = true;
            }
            else if (distance <= _runDistance - 1.0f)
            {
                _isRunning = false;
            }
        }

        private void FlipByMoveDirection(Vector2 targetPosition)
        {
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

            if (moveDirection.x > 0.01f)
            {
                _spriteRenderer.flipX = false;
            }
            else if (moveDirection.x < -0.01f)
            {
                _spriteRenderer.flipX = true;
            }
        }
        #endregion
    }
}

