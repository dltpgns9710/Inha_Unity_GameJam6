using JUNBEOM.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace JUNBEOM.Camera
{
    [RequireComponent(typeof(UnityEngine.Camera))]
    public class MainCameraController : MonoBehaviour
    {
        #region Serialized Fields

        [Header("References")]
        [FormerlySerializedAs("player")]
        [SerializeField] private Transform _player;

        [SerializeField]
        private JUNBEOM.Player.PlayerInputManager _inputManager;

        [Header("Free Camera Settings")]
        [FormerlySerializedAs("_smoothTime")]
        [FormerlySerializedAs("smoothing")]
        [SerializeField, Min(0.01f)]
        private float _freeCameraSmoothTime = 0.15f;

        [Header("Camera Boundary (Global)")]
        [FormerlySerializedAs("minCameraBoundary")]
        [SerializeField] private Vector2 _minCameraBoundary;

        [FormerlySerializedAs("maxCameraBoundary")]
        [SerializeField] private Vector2 _maxCameraBoundary;

        [Header("Edge Scroll Settings (Virtual Box)")]
        [SerializeField, Range(0f, 0.5f)] private float _edgeScrollWidthRatio = 0.1f;
        [SerializeField] private float _edgeScrollSpeed = 15f;

        [Header("Fixed Settings")]
        [SerializeField] private float _yOffset = 2f;

        [Header("Initial State")]
        [SerializeField] private bool _startFollowingPlayer = true;

        #endregion

        #region Private Fields

        private Vector3 _moveVelocity;
        private bool _isFollowingPlayer;

        private UnityEngine.Camera _cam;
        private Vector3 _freeCameraTargetPosition;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            Debug.Assert(
                _player != null,
                $"[{name}] Player Transform 누락");

            Debug.Assert(
                _inputManager != null,
                $"[{name}] PlayerInputManager 누락");

            _cam = GetComponent<UnityEngine.Camera>();
            _isFollowingPlayer = _startFollowingPlayer;
            _freeCameraTargetPosition = transform.position;
        }

        private void OnEnable()
        {
            if (_inputManager == null) return;
            _inputManager.OnReturnCameraEvent += HandleToggleCameraMode;
            _inputManager.OnInteractEvent += HandlePlayerInterect;
        }

        private void OnDisable()
        {
            if (!ReferenceEquals(_inputManager, null))
            {
                _inputManager.OnReturnCameraEvent -= HandleToggleCameraMode;
                _inputManager.OnInteractEvent -= HandlePlayerInterect;
            }
        }

        private void LateUpdate()
        {
            if (_player == null) return;

            if (_isFollowingPlayer)
            {
                FollowPlayerImmediately();
                return;
            }
            UpdateFreeCameraTargetPosition();
            MoveFreeCameraSmoothly();
        }

        private void OnValidate()
        {
            float minX = Mathf.Min(_minCameraBoundary.x, _maxCameraBoundary.x);
            float maxX = Mathf.Max(_minCameraBoundary.x, _maxCameraBoundary.x);
            float minY = Mathf.Min(_minCameraBoundary.y, _maxCameraBoundary.y);
            float maxY = Mathf.Max(_minCameraBoundary.y, _maxCameraBoundary.y);

            _minCameraBoundary = new Vector2(minX, minY);
            _maxCameraBoundary = new Vector2(maxX, maxY);

            _freeCameraSmoothTime = Mathf.Max(0.01f, _freeCameraSmoothTime);
        }

        #endregion

        #region Public Methods
        public void ToggleCameraMode()
        {
            _isFollowingPlayer = !_isFollowingPlayer;

            _moveVelocity = Vector3.zero;

            if (_isFollowingPlayer && _player != null)
            {
                FollowPlayerImmediately();
            }
            else
            {
                _freeCameraTargetPosition = transform.position;
            }
        }

        public bool GetPlayerFollowing()
        {
            return _isFollowingPlayer;
        }

        #endregion

        #region Input Event Handlers

        private void HandleToggleCameraMode()
        {
            ToggleCameraMode();
        }

        private void HandlePlayerInterect()
        {

        }

        #endregion

        #region Private Methods

        private void FollowPlayerImmediately()
        {
            Vector3 targetPosition = GetPlayerTargetPosition();
            targetPosition = ClampToBoundary(targetPosition);

            transform.position = targetPosition;
            _moveVelocity = Vector3.zero;
        }


        private void MoveFreeCameraSmoothly()
        {
            transform.position = Vector3.SmoothDamp(transform.position, _freeCameraTargetPosition, ref _moveVelocity, _freeCameraSmoothTime);
        }

        private void UpdateFreeCameraTargetPosition()
        {
            bool cannotReadMouse = (Mouse.current == null) || (Screen.width <= 0) || (Screen.height <= 0);

            if (!cannotReadMouse)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                float normalizedX = mousePosition.x / Screen.width;

                if (normalizedX >= 0f && normalizedX <= _edgeScrollWidthRatio)
                {
                    _freeCameraTargetPosition.x -= _edgeScrollSpeed * Time.deltaTime;
                }
                else if (normalizedX >= (1f - _edgeScrollWidthRatio) && normalizedX <= 1f)
                {
                    _freeCameraTargetPosition.x += _edgeScrollSpeed * Time.deltaTime;
                }
            }


            _freeCameraTargetPosition.y = _player.position.y + _yOffset;
            _freeCameraTargetPosition.z = transform.position.z;


            _freeCameraTargetPosition = ClampToBoundary(_freeCameraTargetPosition);
        }

        private Vector3 GetPlayerTargetPosition()
        {
            return new Vector3(_player.position.x, _player.position.y + _yOffset, transform.position.z);
        }

        /// <summary>
        /// 카메라의 중심 위치가 전체 바운더리 및 플레이어 화면 이탈 방지 조건을 넘지 않게 제한
        /// </summary>
        private Vector3 ClampToBoundary(Vector3 targetPosition)
        {
            float minX = _minCameraBoundary.x;
            float maxX = _maxCameraBoundary.x;

            // 카메라 컴포넌트가 존재하고, 플레이어가 화면 내에 있어야 한다는 조건을 추가
            if (_cam != null && _cam.orthographic)
            {
                // 화면의 절반 너비를 계산 (직교 카메라 기준)
                float halfWidth = _cam.orthographicSize * _cam.aspect;

                // 카메라가 (플레이어X - 절반너비) 보다 왼쪽으로 가거나,
                // 카메라가 (플레이어X + 절반너비) 보다 오른쪽으로 가면 플레이어가 화면 밖으로 나감.
                // 약간의 여백을 원할 경우 halfWidth 에 0.9f 등을 곱해 조절 가능합니다.
                float limitMinX = _player.position.x - halfWidth;
                float limitMaxX = _player.position.x + halfWidth;

                // 글로벌 제한과 플레이어 유지 제한 중 더 좁은 범위를 선택
                minX = Mathf.Max(minX, limitMinX);
                maxX = Mathf.Min(maxX, limitMaxX);
            }

            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);

            // Y축은 항상 플레이어 위치 + 설정된 Offset(2f)으로 고정 (글로벌 바운더리보다 우선 적용)
            targetPosition.y = _player.position.y + _yOffset;

            return targetPosition;
        }

        #endregion
    }
}