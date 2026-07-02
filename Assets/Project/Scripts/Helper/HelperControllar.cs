using Unity.VisualScripting;
using UnityEngine;

public class HelperControllar : MonoBehaviour
{

    #region Serialized Fields
    [Header("Follow")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _followDistance = 1.5f;
    #endregion

    #region Private Fields
    private EHelperState _currentState;
    #endregion

    #region Unity Lifecycle
    private void Awake()
    {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _currentState = EHelperState.Follow;

        Debug.Assert(_playerTransform != null,
            "Player Transform이 연결되지 않았습니다.");
    }
    void Start()
    {
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
            return;
        }

        transform.position = Vector2.MoveTowards(
            transform.position,
            _playerTransform.position,
            _moveSpeed * Time.deltaTime);
    }
    #endregion
}
