using System.Runtime.CompilerServices;
using UnityEngine;

public class HelperControllar : MonoBehaviour
{

    #region Serialized Fields
    [Header("Helper Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _walkSpeed = 3.0f;
    [SerializeField] private float _runSpeed = 6.0f;
    [SerializeField] private float _runDistance = 4.0f;
    [SerializeField] private float _followDistance = 1.5f;

    #endregion


    #region Private Fields
    private EHelperState _currentState;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isRunning;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _currentState = EHelperState.Follow;

        Debug.Assert(_playerTransform != null, "Player Transform이 연결되지 않았습니다.");
        Debug.Assert(_spriteRenderer != null, "SpriteRenderer가 연결되지 않았습니다.");
    }
    void Update()
    {
        UpdateState();
    }
    #endregion

    #region Private Methods
    private void UpdateState()
    {
        switch(_currentState)
        {
            case EHelperState.Idle:
                UpdateIdle();
                break;
            case EHelperState.Follow:
                UpdateFollow();
                break;
            case EHelperState.DetectAnomaly:
                UpdateDetectAnomaly();
                break;
        }
    }

    private void UpdateIdle()
    {
        _animator.SetBool("IsMoving", false);
        _animator.SetBool("IsRunning", false);
    }

    private void UpdateFollow()
    {
        FollowPlayer();
    }

    private void UpdateDetectAnomaly()
    {
        //이상현상 감지 시 행동 정의
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);  

        bool isMoving = distance > _followDistance;
        bool isRunning = distance >= _runDistance;

        UpdateRunState(distance);

        _animator.SetBool("IsMoving", distance > _followDistance);
        _animator.SetBool("IsRunning", _isRunning);

        if (!isMoving)
        {        
            return;
        }

        float moveSpeed = _isRunning ? _runSpeed : _walkSpeed;

        FlipByMoveDirection();

        transform.position = Vector2.MoveTowards(
            transform.position,
            _playerTransform.position,
            moveSpeed * Time.deltaTime);
    }

    private void UpdateRunState(float distance)
    {
        if (distance >= _runDistance)
        {
            _isRunning = true;
        }
        else if(distance <= _runDistance - 1.0)
        {
            _isRunning = false;
        }
    }

    private void FlipByMoveDirection()
    {
        Vector2 moveDirection = (_playerTransform.position - transform.position).normalized;

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
