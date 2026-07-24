using UnityEngine;

public sealed class PooledWanderingPet : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int IsRunningHash = Animator.StringToHash("IsRunning");
    private static readonly int IsWaitingHash = Animator.StringToHash("IsWaiting");
    private static readonly int IsSleepingHash = Animator.StringToHash("IsSleeping");

    private SpriteRenderer _renderer;
    private Animator _animator;
    private int _baseSortingOrder;
    private float _minX;
    private float _maxX;
    private float _minSpeed;
    private float _maxSpeed;
    private float _minIdle;
    private float _maxIdle;
    private float _minimumMoveDistance;
    private float _targetX;
    private float _moveSpeed;
    private float _idleRemaining;
    private bool _walking;
    private bool _active;

    public void CacheComponents(SpriteRenderer spriteRenderer, Animator animator, int baseSortingOrder)
    {
        _renderer = spriteRenderer;
        _animator = animator;
        _baseSortingOrder = baseSortingOrder;
    }

    public void BeginWandering(
        float minX,
        float maxX,
        float minSpeed,
        float maxSpeed,
        float minIdle,
        float maxIdle,
        float minimumMoveDistance)
    {
        _minX = Mathf.Min(minX, maxX);
        _maxX = Mathf.Max(minX, maxX);
        _minSpeed = Mathf.Min(minSpeed, maxSpeed);
        _maxSpeed = Mathf.Max(minSpeed, maxSpeed);
        _minIdle = Mathf.Min(minIdle, maxIdle);
        _maxIdle = Mathf.Max(minIdle, maxIdle);
        _minimumMoveDistance = Mathf.Max(0.1f, minimumMoveDistance);
        _active = true;

        SetCommonAnimatorValues();
        if (Random.value < 0.7f)
            ChooseNewTarget();
        else
            BeginIdle();

        UpdateSortingOrder();
    }

    public void StopWandering()
    {
        _active = false;
        SetWalking(false);
    }

    private void Update()
    {
        if (!_active) return;

        if (_walking)
        {
            Vector3 position = transform.position;
            position.x = Mathf.MoveTowards(position.x, _targetX, _moveSpeed * Time.deltaTime);
            transform.position = position;

            if (Mathf.Abs(position.x - _targetX) <= 0.01f)
                BeginIdle();
        }
        else
        {
            _idleRemaining -= Time.deltaTime;
            if (_idleRemaining <= 0f)
                ChooseNewTarget();
        }
    }

    private void ChooseNewTarget()
    {
        float currentX = transform.position.x;
        _targetX = Random.Range(_minX, _maxX);

        if (Mathf.Abs(_targetX - currentX) < _minimumMoveDistance)
        {
            float leftDistance = currentX - _minX;
            float rightDistance = _maxX - currentX;
            _targetX = leftDistance > rightDistance
                ? Mathf.Max(_minX, currentX - _minimumMoveDistance)
                : Mathf.Min(_maxX, currentX + _minimumMoveDistance);
        }

        _moveSpeed = Random.Range(_minSpeed, _maxSpeed);
        _renderer.flipX = _targetX < currentX;
        SetWalking(true);
    }

    private void BeginIdle()
    {
        _idleRemaining = Random.Range(_minIdle, _maxIdle);
        SetWalking(false);
    }

    private void SetWalking(bool walking)
    {
        _walking = walking;
        if (_animator == null) return;
        _animator.SetBool(IsMovingHash, walking);
        _animator.SetBool(IsRunningHash, false);
    }

    private void SetCommonAnimatorValues()
    {
        _animator.SetBool(IsWaitingHash, false);
        _animator.SetBool(IsSleepingHash, false);
        _animator.SetBool(IsRunningHash, false);
    }

    private void UpdateSortingOrder()
    {
        _renderer.sortingOrder = _baseSortingOrder + Mathf.RoundToInt(-transform.position.y * 10f);
    }
}
