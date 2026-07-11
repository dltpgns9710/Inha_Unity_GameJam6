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

        [Header("Camera Boundary")]
        [FormerlySerializedAs("minCameraBoundary")]
        [SerializeField] private Vector2 _minCameraBoundary;

        [FormerlySerializedAs("maxCameraBoundary")]
        [SerializeField] private Vector2 _maxCameraBoundary;

        [Header("Initial State")]
        [SerializeField] private bool _startFollowingPlayer = true;

        #endregion

        #region Private Fields

        private Vector3 _moveVelocity;
        private bool _isFollowingPlayer;

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

            _isFollowingPlayer = _startFollowingPlayer;
        }

        private void OnEnable()
        {
            if (_inputManager == null) return;
            _inputManager.OnReturnCameraEvent += HandleToggleCameraMode;
            _inputManager.OnInteractEvent += HandlePlayerInterect;
        }

        private void OnDisable()
        {
            if (_inputManager == null) return;
            _inputManager.OnReturnCameraEvent -= HandleToggleCameraMode;
            _inputManager.OnInteractEvent -= HandlePlayerInterect;
        }

        private void LateUpdate()
        {
            if (_player == null) return;

            if (_isFollowingPlayer)
            {
                FollowPlayerImmediately();
                return;
            }

            MoveFreeCameraSmoothly();
        }

        private void OnValidate()
        {
            float minX = Mathf.Min(
                _minCameraBoundary.x,
                _maxCameraBoundary.x);

            float maxX = Mathf.Max(
                _minCameraBoundary.x,
                _maxCameraBoundary.x);

            float minY = Mathf.Min(
                _minCameraBoundary.y,
                _maxCameraBoundary.y);

            float maxY = Mathf.Max(
                _minCameraBoundary.y,
                _maxCameraBoundary.y);

            _minCameraBoundary = new Vector2(minX, minY);
            _maxCameraBoundary = new Vector2(maxX, maxY);

            _freeCameraSmoothTime = Mathf.Max(
                0.01f,
                _freeCameraSmoothTime);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 플레이어 추적 모드와 자유 카메라 모드를 전환
        /// </summary>
        public void ToggleCameraMode()
        {
            _isFollowingPlayer = !_isFollowingPlayer;

            _moveVelocity = Vector3.zero;

            if (_isFollowingPlayer && _player != null)
            {
                FollowPlayerImmediately();
            }
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

        /// <summary>
        /// Smooth Time을 사용하지 않고 카메라를 플레이어에게 즉시 고정
        /// </summary>
        private void FollowPlayerImmediately()
        {
            Vector3 targetPosition = GetPlayerTargetPosition();
            targetPosition = ClampToBoundary(targetPosition);

            transform.position = targetPosition;


            _moveVelocity = Vector3.zero;
        }

        /// <summary>
        /// 자유 카메라 상태에서만 Smooth Time을 사용
        /// </summary>
        private void MoveFreeCameraSmoothly()
        {
            Vector3 targetPosition = GetFreeCameraTargetPosition();
            targetPosition = ClampToBoundary(targetPosition);

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _moveVelocity, _freeCameraSmoothTime);
        }

        /// <summary>
        /// 플레이어의 현재 위치를 카메라 목표 위치로 반환.
        /// </summary>
        private Vector3 GetPlayerTargetPosition()
        {
            return new Vector3(_player.position.x, _player.position.y, transform.position.z);
        }

        /// <summary>
        /// 마우스의 화면 위치를 Camera Boundary 범위에 대응
        /// </summary>
        private Vector3 GetFreeCameraTargetPosition()
        {
            bool cannotReadMouse = (Mouse.current == null) || (Screen.width <= 0) ||(Screen.height <= 0);

            if (cannotReadMouse)
            {
                return transform.position;
            }

            Vector2 mousePosition = Mouse.current.position.ReadValue();

            float normalizedX = Mathf.Clamp01(mousePosition.x / Screen.width);

            float normalizedY = Mathf.Clamp01(mousePosition.y / Screen.height);

            float targetX = Mathf.Lerp(_minCameraBoundary.x, _maxCameraBoundary.x, normalizedX);

            float targetY = Mathf.Lerp(_minCameraBoundary.y, _maxCameraBoundary.y, normalizedY);

            return new Vector3(targetX, targetY, transform.position.z);
        }

        /// <summary>
        /// 카메라 중심 위치가 Camera Boundary를 벗어나지 않게 제한
        /// </summary>
        private Vector3 ClampToBoundary(
            Vector3 targetPosition)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, _minCameraBoundary.x, _maxCameraBoundary.x);

            //targetPosition.y = _player.position.y;
            targetPosition.y = Mathf.Clamp( targetPosition.y, _minCameraBoundary.y, _maxCameraBoundary.y);

            return targetPosition;
        }

        #endregion
    }
}
