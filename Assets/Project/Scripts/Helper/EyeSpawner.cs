using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EyeSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Eye _eyePrefab;
    [SerializeField] private Transform _poolRoot;

    [Header("Pool")]
    [SerializeField, Min(1)] private int _poolSize = 30;
    [SerializeField, Min(1)] private int _activeEyeCount = 20;

    [Header("Spawn")]
    [SerializeField, Min(0f)] private float _minSpawnDistance = 4f;
    [SerializeField, Min(0.1f)] private float _maxSpawnDistance = 10f;
    [SerializeField, Min(0.1f)] private float _despawnDistance = 13f;
    [SerializeField] private LayerMask _blockedLayers;
    [SerializeField, Min(0f)] private float _blockedCheckRadius = 0.3f;
    [SerializeField, Min(1)] private int _spawnPositionAttempts = 10;

    private readonly Queue<Eye> _pool = new Queue<Eye>();
    private readonly List<Eye> _activeEyes = new List<Eye>();
    private readonly HashSet<Collider2D> _playerColliders = new HashSet<Collider2D>();

    private Transform _player;
    private bool _anomalyEnabled;
    private bool _isRunning;

    private void Awake()
    {
        Collider2D trigger = GetComponent<Collider2D>();
        trigger.isTrigger = true;

        if (_poolRoot == null)
        {
            _poolRoot = transform;
        }

        PrewarmPool();
    }

    private void Update()
    {
        if (_player == null)
        {
            ClearPlayerState();
            return;
        }

        if (!_anomalyEnabled || !_isRunning)
        {
            return;
        }

        RecycleDistantEyes();
        FillActiveEyes(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!TryGetPlayerTransform(other, out Transform player))
        {
            return;
        }

        _playerColliders.Add(other);
        _player = player;

        if (!_anomalyEnabled || _isRunning)
        {
            return;
        }

        _isRunning = true;
        FillActiveEyes(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_playerColliders.Remove(other))
        {
            return;
        }

        RemoveMissingPlayerColliders();

        if (_playerColliders.Count == 0)
        {
            ClearPlayerState();
        }
    }

    private void OnDisable()
    {
        ClearPlayerState();
    }

    public void SetAnomalyActive(bool isActive)
    {
        _anomalyEnabled = isActive;

        if (!isActive)
        {
            StopSpawning();
            return;
        }

        RemoveMissingPlayerColliders();

        if (_player == null || _playerColliders.Count == 0)
        {
            return;
        }

        _isRunning = true;
        FillActiveEyes(true);
    }

    private void PrewarmPool()
    {
        if (_eyePrefab == null)
        {
            Debug.LogError($"{nameof(EyeSpawner)} requires an Eye prefab.", this);
            enabled = false;
            return;
        }

        int count = Mathf.Max(_poolSize, _activeEyeCount);

        for (int i = 0; i < count; i++)
        {
            Eye eye = Instantiate(_eyePrefab, _poolRoot);
            eye.Deactivate();
            _pool.Enqueue(eye);
        }
    }

    private void FillActiveEyes(bool playSpawnSound)
    {
        int targetCount = Mathf.Min(_activeEyeCount, _activeEyes.Count + _pool.Count);

        while (_activeEyes.Count < targetCount && _pool.Count > 0)
        {
            if (!TryFindSpawnPosition(out Vector3 spawnPosition))
            {
                break;
            }

            Eye eye = _pool.Dequeue();
            eye.Activate(spawnPosition, playSpawnSound);
            _activeEyes.Add(eye);
            playSpawnSound = false;
        }
    }

    private void RecycleDistantEyes()
    {
        float despawnDistanceSqr = _despawnDistance * _despawnDistance;

        for (int i = _activeEyes.Count - 1; i >= 0; i--)
        {
            Eye eye = _activeEyes[i];

            if (eye != null &&
                (eye.transform.position - _player.position).sqrMagnitude <= despawnDistanceSqr)
            {
                continue;
            }

            _activeEyes.RemoveAt(i);

            if (eye != null)
            {
                ReturnToPool(eye);
            }
        }
    }

    private bool TryFindSpawnPosition(out Vector3 spawnPosition)
    {
        float minDistance = Mathf.Min(_minSpawnDistance, _maxSpawnDistance);
        float maxDistance = Mathf.Max(_minSpawnDistance, _maxSpawnDistance);

        for (int i = 0; i < _spawnPositionAttempts; i++)
        {
            float angle = Random.Range(0f, Mathf.PI * 2f);
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 candidate = _player.position + (Vector3)(direction * distance);

            if (!IsSpawnPositionBlocked(candidate))
            {
                spawnPosition = candidate;
                return true;
            }
        }

        spawnPosition = default;
        return false;
    }

    private bool IsSpawnPositionBlocked(Vector3 position)
    {
        return _blockedLayers.value != 0 &&
               Physics2D.OverlapCircle(position, _blockedCheckRadius, _blockedLayers) != null;
    }

    private static bool TryGetPlayerTransform(Collider2D other, out Transform player)
    {
        Rigidbody2D attachedBody = other.attachedRigidbody;

        if (other.CompareTag("Player"))
        {
            player = attachedBody != null ? attachedBody.transform : other.transform;
            return true;
        }

        if (attachedBody != null && attachedBody.CompareTag("Player"))
        {
            player = attachedBody.transform;
            return true;
        }

        player = null;
        return false;
    }

    private void RemoveMissingPlayerColliders()
    {
        _playerColliders.RemoveWhere(collider => collider == null);
    }

    private void ClearPlayerState()
    {
        StopSpawning();
        _player = null;
        _playerColliders.Clear();
    }

    private void StopSpawning()
    {
        _isRunning = false;

        for (int i = _activeEyes.Count - 1; i >= 0; i--)
        {
            Eye eye = _activeEyes[i];

            if (eye != null)
            {
                ReturnToPool(eye);
            }
        }

        _activeEyes.Clear();
    }

    private void ReturnToPool(Eye eye)
    {
        eye.Deactivate();
        eye.transform.SetParent(_poolRoot);
        _pool.Enqueue(eye);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _activeEyeCount = Mathf.Min(_activeEyeCount, _poolSize);
        _maxSpawnDistance = Mathf.Max(_maxSpawnDistance, _minSpawnDistance);
        _despawnDistance = Mathf.Max(_despawnDistance, _maxSpawnDistance);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = Application.isPlaying && _player != null
            ? _player.position
            : transform.position;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, _minSpawnDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center, _maxSpawnDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, _despawnDistance);
    }
#endif
}
