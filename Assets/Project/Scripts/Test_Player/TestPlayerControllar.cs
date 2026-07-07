using UnityEngine;
using TAEWOOK.Helper;

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

    [Header("Helper Command")]
    [SerializeField] private HelperCommandBroadcaster _helperCommandBroadcaster;

    [Header("Detect Targeting")]
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _cameraFollowMouseDistance = 3.0f;
    [SerializeField] private float _detectTargetingCameraSize = 4.0f;
    #endregion

    #region Private Fields
    private Rigidbody2D _rigidbody2D;

    private float _moveInput;
    private bool _isGrounded;
    private bool _isDetectTargeting;
    private Camera _camera;
    private Vector3 _defaultCameraLocalPosition;
    private float _defaultCameraSize;
    
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        Debug.Assert(_rigidbody2D != null,
            $"[{name}] Rigidbody2D가 연결되지 않았습니다.");

        Debug.Assert(_groundCheck != null,
            $"[{name}] Ground Check가 연결되지 않았습니다.");

        Debug.Assert(_helperCommandBroadcaster != null,
            $"[{name}] HelperCommandBroadcaster가 연결되지 않았습니다.");

        _camera = Camera.main;

        if (_cameraTransform == null && _camera != null)
        {
            _cameraTransform = _camera.transform;
        }

        if (_cameraTransform != null)
        {
            _defaultCameraLocalPosition = _cameraTransform.localPosition;
        }

        if (_camera != null)
        {
            _defaultCameraSize = _camera.orthographicSize;
        }
    }

    private void Update()
    {
        HandleInput();
        HandleHelperCommandInput();
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

    private void HandleHelperCommandInput()
    {
        if (_helperCommandBroadcaster == null)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartDetectTargeting();
        }

        if (_isDetectTargeting)
        {
            UpdateDetectTargeting();
            HandleDetectTargetingInput();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _helperCommandBroadcaster.RequestWait();
        }
    }

    private void StartDetectTargeting()
    {
        if (_cameraTransform == null)
        {
            return;
        }

        _isDetectTargeting = true;

        if (_camera != null)
        {
            _camera.orthographicSize = _detectTargetingCameraSize;
        }
    }

    private void UpdateDetectTargeting()
    {
        if (_cameraTransform == null || _camera == null)
        {
            return;
        }

        Vector3 mouseViewportPosition = _camera.ScreenToViewportPoint(Input.mousePosition);

        Vector2 normalizedMouseOffset = new Vector2(
            mouseViewportPosition.x - 0.5f,
            mouseViewportPosition.y - 0.5f) * 2.0f;

        _cameraTransform.localPosition = _defaultCameraLocalPosition + new Vector3(
            normalizedMouseOffset.x * _cameraFollowMouseDistance,
            normalizedMouseOffset.y * _cameraFollowMouseDistance,
            0.0f);
    }

    private void HandleDetectTargetingInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RequestDetectAtMousePosition();
            EndDetectTargeting();
            return;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            EndDetectTargeting();
        }
    }

    private void RequestDetectAtMousePosition()
    {
        if (_camera == null)
        {
            return;
        }

        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = -_camera.transform.position.z;

        Vector2 mouseWorldPosition = _camera.ScreenToWorldPoint(mouseScreenPosition);
        _helperCommandBroadcaster.RequestDetectAnomaly(mouseWorldPosition);
    }

    private void EndDetectTargeting()
    {
        _isDetectTargeting = false;

        if (_cameraTransform != null)
        {
            _cameraTransform.localPosition = _defaultCameraLocalPosition;
        }

        if (_camera != null)
        {
            _camera.orthographicSize = _defaultCameraSize;
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
