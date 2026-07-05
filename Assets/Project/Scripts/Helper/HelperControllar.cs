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

    [Header("Command")]
    [SerializeField] private float _commandSearchDistance = 6.0f;

    [Header("Detect")]
    [SerializeField] private float _anomalyArriveDistance = 1.0f;
    [SerializeField] private LayerMask _anomalyLayer;
    #endregion


    #region Private Fields
    private EHelperState _currentState;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isRunning;
    private Transform _targetAnomaly;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        ChangeState(EHelperState.Follow);

        Debug.Assert(_playerTransform != null, "Player Transform이 연결되지 않았습니다.");
        Debug.Assert(_spriteRenderer != null, "SpriteRenderer가 연결되지 않았습니다.");
        Debug.Assert(_animator != null, "Animator가 연결되지 않았습니다.");
    }

    private void Update()
    {
        UpdateState();
    }
    #endregion

    #region Public Methods
    public void RequestDetectAnomaly()
    {
        if (_currentState == EHelperState.MoveToAnomaly ||
            _currentState == EHelperState.Alert ||
            _currentState == EHelperState.ReturnToPlayer)
        {
            return;
        }

        ChangeState(EHelperState.DetectAnomaly);
    }

    public void OnBarkAnimationEnd()
    {
        _targetAnomaly = null;
        ChangeState(EHelperState.ReturnToPlayer);
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
            case EHelperState.MoveToAnomaly:
                UpdateMoveToAnomaly();
                break;
            case EHelperState.Alert:
                UpdateAlert();
                break;
            case EHelperState.ReturnToPlayer:
                UpdateReturnToPlayer();
                break;
        }
    }

    #region State Transition Logic
    private void ChangeState(EHelperState nextState)
    {
        if(_currentState == nextState)
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
            case EHelperState.Idle:
                break;
            case EHelperState.Follow:
                break;
            case EHelperState.DetectAnomaly:
                break;
            case EHelperState.MoveToAnomaly:
                break;
            case EHelperState.Alert:
                _animator.SetBool("IsMoving", false);
                _animator.SetBool("IsRunning", false);
                _animator.SetTrigger("Bark");
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
            case EHelperState.DetectAnomaly:
                break;
            case EHelperState.MoveToAnomaly:
                break;
            case EHelperState.Alert:
                break;
            case EHelperState.ReturnToPlayer:
                break;
        }
    }
    #endregion

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
        _targetAnomaly = FindNearestAnomaly();

        if (_targetAnomaly == null)
        {
            ChangeState(EHelperState.Follow);
            return;
        }

        ChangeState(EHelperState.MoveToAnomaly);
    }

    private void UpdateMoveToAnomaly()
    {
        if (_targetAnomaly == null)
        {
            ChangeState(EHelperState.ReturnToPlayer);
            return;
        }

        MoveToTarget(_targetAnomaly.position, _anomalyArriveDistance);

        if (Vector2.Distance(transform.position, _targetAnomaly.position) <= _anomalyArriveDistance)
        {
            ChangeState(EHelperState.Alert);
        }
    }

    private void UpdateAlert()
    {

    }

    private void UpdateReturnToPlayer()
    {
        FollowPlayer();

        if (Vector2.Distance(transform.position, _playerTransform.position) <= _followDistance)
        {
            ChangeState(EHelperState.Follow);
        }
    }

    private Transform FindNearestAnomaly()
    {
        Collider2D[] detectColliders = Physics2D.OverlapCircleAll(
            transform.position,
            _commandSearchDistance,
            _anomalyLayer);

        Transform nearestAnomaly = null;
        float nearestDistance = float.MaxValue;

        foreach (Collider2D detectCollider in detectColliders)
        {
            float distance = Vector2.Distance(transform.position, detectCollider.transform.position);

            if (distance >= nearestDistance)
            {
                continue;
            }

            nearestDistance = distance;
            nearestAnomaly = detectCollider.transform;
        }

        return nearestAnomaly;
    }

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);

        bool isMoving = distance > _followDistance;

        UpdateRunState(distance);

        _animator.SetBool("IsMoving", isMoving);
        _animator.SetBool("IsRunning", _isRunning);

        if (!isMoving)
        {
            return;
        }

        float moveSpeed = _isRunning ? _runSpeed : _walkSpeed;

        FlipByMoveDirection(_playerTransform.position);

        transform.position = Vector2.MoveTowards(
            transform.position,
            _playerTransform.position,
            moveSpeed * Time.deltaTime);
    }

    private void MoveToTarget(Vector2 targetPosition, float stopDistance)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        bool isMoving = distance > stopDistance;

        UpdateRunState(distance);

        _animator.SetBool("IsMoving", isMoving);
        _animator.SetBool("IsRunning", _isRunning);

        if (!isMoving)
        {
            return;
        }

        float moveSpeed = _isRunning ? _runSpeed : _walkSpeed;

        FlipByMoveDirection(targetPosition);

        transform.position = Vector2.MoveTowards(
            transform.position,
            targetPosition,
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            transform.position,
            _commandSearchDistance);
    }
    #endregion
}
