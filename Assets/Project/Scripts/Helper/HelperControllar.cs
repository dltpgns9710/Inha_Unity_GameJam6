using UnityEngine;

public class HelperControllar : MonoBehaviour
{

    #region Serialized Fields
    [Header("Helper Settings")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _followDistance = 1.5f;

    #endregion


    #region Private Fields
    private EHelperState _currentState;
    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private bool _isMoving;
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
                // Idle 상태에서의 동작
                break;
            case EHelperState.Follow:
                FollowPlayer();
                break;
            case EHelperState.DetectAnomaly:
                // DetectAnomaly 상태에서의 동작
                break;
        }
    }

   

    private void FollowPlayer()
    {
        float distance = Vector2.Distance(transform.position, _playerTransform.position);

        if(distance <= _followDistance)
        {
            _isMoving = false;
            _animator.SetBool("IsMoving", _isMoving);
            return;
        }

        Vector2 moveDirection = (_playerTransform.position - transform.position).normalized;

        if (moveDirection.x > 0.01f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (moveDirection.x < -0.01f)
        {
            _spriteRenderer.flipX = true;
        }

        _isMoving = true;
        _animator.SetBool("IsMoving", _isMoving);

        transform.position = Vector2.MoveTowards(
            transform.position,
            _playerTransform.position,
            _moveSpeed * Time.deltaTime);     
    }
    #endregion
}
