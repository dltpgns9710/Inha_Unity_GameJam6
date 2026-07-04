using System.Collections.Generic;
using UnityEngine;

namespace JUNBEOM.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Constants

        private const float DEFAULT_WALK_SPEED = 2.0f;
        private const float DEFAULT_RUN_SPEED = 5.0f;
        private const float DEFAULT_JUMP_FORCE = 5.0f;
        private const float GROUND_NORMAL_THRESHOLD = 0.7f;

        #endregion

        #region Serialized Fields

        [Header("Movement Settings")]
        [SerializeField] private float _walkSpeed = DEFAULT_WALK_SPEED;
        [SerializeField] private float _runSpeed = DEFAULT_RUN_SPEED;
        [SerializeField] private float _jumpForce = DEFAULT_JUMP_FORCE;

        [Header("References")]
        [SerializeField] private PlayerInputManager _inputManager;

        #endregion

        #region Private Fields

        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Transform _cachedTransform;

        private float _moveInputX;
        private Vector3 _initialScale;
        private float _currentMoveSpeed;

        private bool _isJumping;
        private bool _isRunning;
        private bool _isGrounded;

        private HashSet<Collider2D> _groundColliders = new HashSet<Collider2D>();

        #endregion

        #region Animator Hash

        private static class AnimHash
        {
            public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
            public static readonly int Velocity = Animator.StringToHash("Velocity");
            public static readonly int IsRunning = Animator.StringToHash("IsRunning");
            public static readonly int IsJumping = Animator.StringToHash("IsJumping");
        }

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _cachedTransform = transform;

            _initialScale = _cachedTransform.localScale;
            _currentMoveSpeed = _walkSpeed;

            Debug.Assert(_inputManager != null, $"[{name}] PlayerInputManager 누락");
        }

        private void OnEnable()
        {
            _inputManager.OnMoveEvent += HandleMoveInput;
            _inputManager.OnRunEvent += HandleRunInput;
            _inputManager.OnJumpEvent += HandleJumpInput;
        }

        private void OnDisable()
        {
            _inputManager.OnMoveEvent -= HandleMoveInput;
            _inputManager.OnRunEvent -= HandleRunInput;
            _inputManager.OnJumpEvent -= HandleJumpInput;
        }

        private void Update()
        {
            UpdateAnimation();
        }

        private void FixedUpdate()
        {
            Move();
        }

        #endregion

        #region Input Event Handlers

        private void HandleMoveInput(float inputX)
        {
            _moveInputX = inputX;
            UpdateFacingDirection();
        }

        private void HandleRunInput(bool isRunning)
        {
            _isRunning = isRunning;
            _currentMoveSpeed = _isRunning ? _runSpeed : _walkSpeed;
        }

        private void HandleJumpInput()
        {
            bool canJump = _isGrounded && !_isJumping;
            if (!canJump) return;

            _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);

            _isGrounded = false;
            _isJumping = true;

            _animator.SetBool(AnimHash.IsJumping, true);
            _animator.SetBool(AnimHash.IsGrounded, false);
        }

        #endregion

        #region Private Methods

        private void Move()
        {
            Vector2 currentVelocity = _rigidbody.linearVelocity;
            currentVelocity.x = _moveInputX * _currentMoveSpeed;
            _rigidbody.linearVelocity = currentVelocity;
        }

        private void UpdateFacingDirection()
        {
            if (Mathf.Approximately(_moveInputX, 0.0f)) return;

            Vector3 scale = _initialScale;
            scale.x = _moveInputX < 0.0f ? -Mathf.Abs(_initialScale.x) : Mathf.Abs(_initialScale.x);
            _cachedTransform.localScale = scale;
        }

        private void UpdateAnimation()
        {
            float movementAmount = Mathf.Abs(_moveInputX);

            _animator.SetBool(AnimHash.IsGrounded, _isGrounded);
            _animator.SetFloat(AnimHash.Velocity, movementAmount);
            _animator.SetBool(AnimHash.IsRunning, _isRunning);
            _animator.SetBool(AnimHash.IsJumping, _isJumping);
        }

        private bool IsGroundCollision(Collision2D collision)
        {
            for (int i = 0; i < collision.contactCount; i++)
            {
                if (collision.GetContact(i).normal.y > GROUND_NORMAL_THRESHOLD)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Collision

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!IsGroundCollision(collision)) return;

            if (collision.collider != null)
            {
                _groundColliders.Add(collision.collider);
            }

            _isGrounded = _groundColliders.Count > 0;
            _isJumping = false;
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (!IsGroundCollision(collision)) return;

            if (collision.collider != null)
            {
                _groundColliders.Add(collision.collider);
            }

            _isGrounded = _groundColliders.Count > 0;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider != null && _groundColliders.Contains(collision.collider))
            {
                _groundColliders.Remove(collision.collider);
            }

            _isGrounded = _groundColliders.Count > 0;
        }

        #endregion
    }
}