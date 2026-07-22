using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyPostInteract : AnomalyBase
{
    [Header("가구 대화")]
    [SerializeField] private GameObject _furnitureObject;

    [Header("표시 프리팹")]
    [SerializeField] private GameObject _spawnPrefab;

    private FurnitureDialogue _furnitureDialogue;
    private GameObject _spawnedInstance;

    private void Awake()
    {
        if (_furnitureObject != null)
        {
            _furnitureDialogue = _furnitureObject.GetComponent<FurnitureDialogue>();
        }
    }

    public override void Apply()
    {
        if (_furnitureDialogue != null)
        {
            _furnitureDialogue.InteractEndEvent.AddListener(OnInteractEnd);
        }

        SpawnInstance();
    }

    public override void Remove()
    {
        if (_furnitureDialogue != null)
        {
            _furnitureDialogue.InteractEndEvent.RemoveListener(OnInteractEnd);
        }

        DespawnInstance();
    }

    private void OnInteractEnd()
    {
        if (_spawnedInstance != null)
        {
            _spawnedInstance.SetActive(true);
        }
    }

    private void SpawnInstance()
    {
        if (_spawnPrefab == null || _furnitureObject == null)
        {
            return;
        }

        _spawnedInstance = Instantiate(_spawnPrefab, _furnitureObject.transform.position, Quaternion.identity);
        _spawnedInstance.SetActive(false);
    }

    private void DespawnInstance()
    {
        if (_spawnedInstance != null)
        {
            Destroy(_spawnedInstance);
            _spawnedInstance = null;
        }
    }
}
