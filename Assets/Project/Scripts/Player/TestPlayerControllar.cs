using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    #region Serialized Fields
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5.0f;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 8.0f;

    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    #endregion

    #region Private Fields
    private Rigidbody2D _rigidbody2D;

    private float _moveInput;
    private bool _isGrounded;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Debug.Assert(_rigidbody2D != null,
            $"[{name}] Rigidbody2D가 연결되지 않았습니다.");

        Debug.Assert(_groundCheck != null,
            $"[{name}] Ground Check가 연결되지 않았습니다.");
    }

    private void Update()
    {
        HandleInput();
        CheckGround();
    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Private Methods
    private void HandleInput()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            Jump();
        }
    }

    private void Move()
    {
        _rigidbody2D.linearVelocity = new Vector2(
            _moveInput * _moveSpeed,
            _rigidbody2D.linearVelocity.y);
    }

    private void Jump()
    {
        _rigidbody2D.linearVelocity = new Vector2(
            _rigidbody2D.linearVelocity.x,
            _jumpForce);
    }

    private void CheckGround()
    {
        _isGrounded = Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundCheckRadius,
            _groundLayer);
    }
    #endregion
}