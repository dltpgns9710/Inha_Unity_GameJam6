using UnityEngine;

public class CreatureTrigger : MonoBehaviour
{
    private const float DEFAULT_WALK_SPEED = 1.5f;

    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = DEFAULT_WALK_SPEED;

    private bool _detect = false;
    private bool _move = false;
    private bool _isAnomaly = false;

    private Animator _animator;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        UpdateAnimation();
        if (_move)
        {
            Vector2 currentVelocity = _rigidbody.linearVelocity;
            currentVelocity.x = _walkSpeed;
            _rigidbody.linearVelocity = currentVelocity;
        }
    }

    public void SetAnomalyState()
    {
        _isAnomaly = true;
        if (_detect && !_move)
        {
            StartMoving();
        }
    }
    private void StartMoving()
    {
        _move = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_move == false)
            {
                _detect = true;
                if (_isAnomaly)
                {
                    StartMoving();
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_move == false)
                _detect = false;
        }
    }
    private void UpdateAnimation()
    {
        _animator.SetBool("IsOpenMouth", _detect);
        _animator.SetBool("IsMoving", _move);
    }
}
