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
        if (!_anomalyEnabled || !_isRunning || _player == null)
        {
            return;
        }

        RecycleDistantEyes();
        FillActiveEyes();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        _playerColliders.Add(other);
        _player = other.attachedRigidbody != null
            ? other.attachedRigidbody.transform
            : other.transform;

        if (_anomalyEnabled)
        {
            _isRunning = true;
            FillActiveEyes();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        _playerColliders.Remove(other);

        if (_playerColliders.Count == 0)
        {
            StopSpawning();
            _player = null;
        }
    }

    private void OnDisable()
    {
        StopSpawning();
        _player = null;
        _playerColliders.Clear();
    }

    public void SetAnomalyActive(bool isActive)
    {
        _anomalyEnabled = isActive;

        if (!isActive)
        {
            StopSpawning();
            return;
        }

        if (_player != null && _playerColliders.Count > 0)
        {
            _isRunning = true;
            FillActiveEyes();
        }
    }

    private void PrewarmPool()
    {
        if (_eyePrefab == null)
        {            
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

    private void FillActiveEyes()
    {
        int targetCount = Mathf.Min(_activeEyeCount, _activeEyes.Count + _pool.Count);
        bool shouldPlaySound = true;

        while (_activeEyes.Count < targetCount && _pool.Count > 0)
        {
            Eye eye = _pool.Dequeue();
            eye.Activate(FindSpawnPosition(), shouldPlaySound);
            _activeEyes.Add(eye);
            shouldPlaySound = false;
        }
    }

    private void RecycleDistantEyes()
    {
        float despawnDistanceSqr = _despawnDistance * _despawnDistance;

        for (int i = _activeEyes.Count - 1; i >= 0; i--)
        {
            Eye eye = _activeEyes[i];
            if ((eye.transform.position - _player.position).sqrMagnitude <= despawnDistanceSqr)
            {
                continue;
            }

            _activeEyes.RemoveAt(i);
            ReturnToPool(eye);
        }
    }

    private Vector3 FindSpawnPosition()
    {
        float minDistance = Mathf.Min(_minSpawnDistance, _maxSpawnDistance);
        float maxDistance = Mathf.Max(_minSpawnDistance, _maxSpawnDistance);
        Vector3 fallbackPosition = _player.position;

        for (int i = 0; i < _spawnPositionAttempts; i++)
        {
            Vector2 direction = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);
            Vector3 candidate = _player.position + (Vector3)(direction * distance);
            fallbackPosition = candidate;

            if (_blockedLayers.value == 0 ||
                !Physics2D.OverlapCircle(candidate, _blockedCheckRadius, _blockedLayers))
            {
                return candidate;
            }
        }

        return fallbackPosition;
    }

    private void StopSpawning()
    {
        _isRunning = false;

        for (int i = _activeEyes.Count - 1; i >= 0; i--)
        {
            ReturnToPool(_activeEyes[i]);
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
