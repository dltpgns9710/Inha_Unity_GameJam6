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

    [Header("Detect")]
    [SerializeField] private float _detectDistance = 3.0f; 
    [SerializeField] private LayerMask _anomalyLayer;
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
        ChangeState(EHelperState.Follow);

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
            case EHelperState.Alert:
                UpdateAlert();
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
            case EHelperState.Alert:
                _animator.SetBool("IsMoving", false);
                _animator.SetBool("IsRunning", false);
                _animator.SetTrigger("Bark");
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
            case EHelperState.Alert:
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

        if(CanDetectAnomaly())
        {
            ChangeState(EHelperState.DetectAnomaly);
        }
    }

    private void UpdateDetectAnomaly()
    {
        //이상현상 감지 시 행동 정의
        Collider2D detectCollider = Physics2D.OverlapCircle(
            transform.position,
            _detectDistance,
            _anomalyLayer);

        if (detectCollider == null)
        {
            ChangeState(EHelperState.Follow);
            return;
        }

        Debug.Assert(detectCollider != null, "이상현상 감지하지 못함");
        ChangeState(EHelperState.Alert);
    }

    private void UpdateAlert()
    {
       
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

    private bool CanDetectAnomaly()
    {
        return Physics2D.OverlapCircle(transform.position,
            _detectDistance,
            _anomalyLayer);
    }

    public void OnBarkAnimationEnd()
    {
        ChangeState(EHelperState.Follow);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(
            transform.position,
            _detectDistance);
    }
    #endregion
}
