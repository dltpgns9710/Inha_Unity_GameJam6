using System.Collections.Generic;
using SEHOON.GameSystem;
using UnityEngine;

public sealed class AnomalyPetClone : AnomalyBase
{
    [Header("Visual Source")]
    [SerializeField] private GameObject _sourcePetPrefab;

    [Header("Pool")]
    [SerializeField, Range(1, 60)] private int _poolSize = 30;

    [Header("Corridor Bounds")]
    [SerializeField] private float _minX = -14f;
    [SerializeField] private float _maxX = 14f;
    [SerializeField] private float _minY = -2.2f;
    [SerializeField] private float _maxY = 2.2f;

    [Header("Wandering")]
    [SerializeField] private float _minMoveSpeed = 0.7f;
    [SerializeField] private float _maxMoveSpeed = 1.4f;
    [SerializeField] private float _minIdleTime = 0.4f;
    [SerializeField] private float _maxIdleTime = 2.2f;
    [SerializeField] private float _minimumMoveDistance = 1.5f;

    private readonly List<PooledWanderingPet> _pool = new List<PooledWanderingPet>();
    private Transform _poolRoot;

    private void Awake()
    {
        CreatePets();
    }

    public override void Apply()
    {
        CreatePets();

        int triggerCount = PetCloneTrigger.OpenAll(this);
        Debug.Assert(triggerCount > 0, "PetCloneTrigger 인스턴스가 활성화 되지 않았습니다.", this);
    }

    internal void ShowPets()
    {
        CreatePets();

        for (int i = 0; i < _pool.Count; i++)
        {
            PooledWanderingPet pet = _pool[i];
            Vector3 position = new Vector3(
                Random.Range(Mathf.Min(_minX, _maxX), Mathf.Max(_minX, _maxX)),
                Random.Range(Mathf.Min(_minY, _maxY), Mathf.Max(_minY, _maxY)),
                0f);

            pet.transform.position = position;
            pet.gameObject.SetActive(true);
            pet.BeginWandering(
                _minX,
                _maxX,
                _minMoveSpeed,
                _maxMoveSpeed,
                _minIdleTime,
                _maxIdleTime,
                _minimumMoveDistance);
        }
    }

    public override void Remove()
    {
        PetCloneTrigger.CloseAll(this);
        foreach (PooledWanderingPet pet in _pool)
        {
            if (pet == null) continue;
            pet.StopWandering();
            pet.gameObject.SetActive(false);
        }
    }

    private void CreatePets()
    {
        if (_pool.Count >= _poolSize) return;
        if (_sourcePetPrefab == null)
        {
            Debug.Assert(false, "source pet prefab이 필요합니다.", this);
            return;
        }

        SpriteRenderer sourceRenderer = _sourcePetPrefab.GetComponent<SpriteRenderer>();
        Animator sourceAnimator = _sourcePetPrefab.GetComponent<Animator>();
        if (sourceRenderer == null || sourceAnimator == null || sourceAnimator.runtimeAnimatorController == null)
        {
            Debug.Assert(false, "SpriteRenderer, Animator Controller가 필요합니다.", this);
            return;
        }

        if (_poolRoot == null)
        {
            var rootObject = new GameObject("Pet Clone Pool");
            rootObject.transform.SetParent(transform, false);
            _poolRoot = rootObject.transform;
        }

        while (_pool.Count < _poolSize)
        {
            var clone = new GameObject($"Pooled Pet {_pool.Count + 1:00}");
            clone.transform.SetParent(_poolRoot, false);
            clone.transform.localScale = _sourcePetPrefab.transform.localScale;
            clone.layer = _sourcePetPrefab.layer;

            SpriteRenderer renderer = clone.AddComponent<SpriteRenderer>();
            renderer.sprite = sourceRenderer.sprite;
            renderer.sharedMaterial = sourceRenderer.sharedMaterial;
            renderer.color = sourceRenderer.color;
            renderer.flipY = sourceRenderer.flipY;
            renderer.maskInteraction = sourceRenderer.maskInteraction;
            renderer.spriteSortPoint = sourceRenderer.spriteSortPoint;
            renderer.sortingLayerID = sourceRenderer.sortingLayerID;
            renderer.sortingOrder = sourceRenderer.sortingOrder;

            Animator animator = clone.AddComponent<Animator>();
            animator.runtimeAnimatorController = sourceAnimator.runtimeAnimatorController;
            animator.avatar = sourceAnimator.avatar;
            animator.applyRootMotion = false;
            animator.updateMode = sourceAnimator.updateMode;
            animator.cullingMode = AnimatorCullingMode.CullUpdateTransforms;

            PooledWanderingPet wanderingPet = clone.AddComponent<PooledWanderingPet>();
            wanderingPet.CacheComponents(renderer, animator, sourceRenderer.sortingOrder);
            _pool.Add(wanderingPet);
            clone.SetActive(false);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector3 center = new Vector3((_minX + _maxX) * 0.5f, (_minY + _maxY) * 0.5f, 0f);
        Vector3 size = new Vector3(Mathf.Abs(_maxX - _minX), Mathf.Abs(_maxY - _minY), 0.1f);
        Gizmos.color = new Color(0.25f, 0.9f, 1f, 0.8f);
        Gizmos.DrawWireCube(center, size);
    }
#endif
}

