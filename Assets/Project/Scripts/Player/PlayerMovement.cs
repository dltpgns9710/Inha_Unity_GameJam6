using System.Collections.Generic;
using UnityEngine;
using SEHOON.GameSystem;
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

        [Header("MAinCamera")]
        [SerializeField] private Camera.MainCameraController _mainCamera;

        [Header("AudioSetting")]
        [SerializeField] private AudioClip _moveClip;
        [SerializeField] private AudioClip _runClip;
        [SerializeField] private float _walkSoundInterval = 0.1f; 
        [SerializeField] private float _runSoundInterval = 0.05f;
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
        private bool _isDead = false;

        private HashSet<Collider2D> _groundColliders = new HashSet<Collider2D>();

        private float _footstepTimer = 0f; // 발소리 타이머
        private float _runstepTimer = 0f; // 달리기 타이머

        private bool _isFollowingPlayer;

        #endregion

        #region Private Fields
        public float ControlMultiplier = 1.0f;
        #endregion

        #region Animator Hash

        private static class AnimHash
        {
            public static readonly int IsGrounded = Animator.StringToHash("IsGrounded");
            public static readonly int Velocity = Animator.StringToHash("Velocity");
            public static readonly int IsRunning = Animator.StringToHash("IsRunning");
            public static readonly int IsJumping = Animator.StringToHash("IsJumping");
            public static readonly int IsDead = Animator.StringToHash("IsDead");
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
            HandleFootstepSound();
            HandleRunstepSound();

        }

        private void FixedUpdate()
        {
            if(_isDead) return;
            Move();
        }

        #endregion

        #region Input Event Handlers

        private void HandleMoveInput(float inputX)
        {
            if(!_mainCamera.GetPlayerFollowing())
            {
                _mainCamera.ToggleCameraMode();
            }
            _moveInputX = inputX * ControlMultiplier;
            UpdateFacingDirection();
        }

        private void HandleRunInput(bool isRunning)
        {
            if (!_mainCamera.GetPlayerFollowing())
            {
                _mainCamera.ToggleCameraMode();
            }
            _isRunning = isRunning;
            _currentMoveSpeed = _isRunning ? _runSpeed : _walkSpeed;
        }

        private void HandleJumpInput()
        {
            if (!_mainCamera.GetPlayerFollowing())
            {
                _mainCamera.ToggleCameraMode();
            }
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

        private void HandleFootstepSound()
        {
            // 땅에 닿아있고 && 좌우 이동 입력이 있을 때만 실행
            if (_isGrounded && Mathf.Abs(_moveInputX) > 0.1f && !_isRunning)
            {
                _footstepTimer -= Time.deltaTime;

                if (_footstepTimer <= 0f)
                {
                    SoundManager.Instance.PlaySfx(_moveClip);

                    _footstepTimer =_walkSoundInterval;
                }
            }
            else
            {
                _footstepTimer = 0f;
            }
        }
        private void HandleRunstepSound()
        {
            if (_isGrounded && Mathf.Abs(_moveInputX) > 0.1f&&_isRunning)
            {
                _runstepTimer -= Time.deltaTime;

                if (_runstepTimer <= 0f)
                {
                    SoundManager.Instance.PlaySfx(_runClip);

                    _runstepTimer = _runSoundInterval;
                }
            }
            else
            {
                _runstepTimer = 0f;
            }
        }

        private void Move()
        {
            Vector2 currentVelocity = _rigidbody.linearVelocity;
            currentVelocity.x = _moveInputX * _currentMoveSpeed;
            _rigidbody.linearVelocity = currentVelocity;
        }

        private void UpdateFacingDirection()
        {
            if (Mathf.Approximately(_moveInputX, 0.0f)) return;
            Vector2 scale = _initialScale;
            scale.x = _moveInputX < 0.0f ? -_initialScale.x : _initialScale.x;
            _cachedTransform.localScale = scale;
        }

        private void UpdateAnimation()
        {
            float movementAmount = Mathf.Abs(_moveInputX);

            _animator.SetBool(AnimHash.IsGrounded, _isGrounded);
            _animator.SetFloat(AnimHash.Velocity, movementAmount);
            _animator.SetBool(AnimHash.IsRunning, _isRunning);
            _animator.SetBool(AnimHash.IsJumping, _isJumping);
            _animator.SetBool(AnimHash.IsDead, _isDead);
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

        #region Public Methods
        public void SetSpeedMultiplier(float multiplier)
        {
            _walkSpeed = DEFAULT_WALK_SPEED * multiplier;
            _runSpeed = DEFAULT_RUN_SPEED * multiplier;

            _currentMoveSpeed = _isRunning ? _runSpeed : _walkSpeed;
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // 충돌한 Collider가 적(크리쳐)이면 사망처리
            if (collision.CompareTag("ENEMY"))
            {
                //_inputManager.DisablePlayerInput();
                _isDead = true;
            }
        }
        #endregion
    }
}