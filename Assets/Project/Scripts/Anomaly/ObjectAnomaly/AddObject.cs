using SEHOON.GameSystem;
using System.Collections.Generic;
using UnityEngine;

public class AddObject : AnomalyBase
{
    [System.Serializable]
    public struct GameObjectPlacement
    {
        public GameObject targetObject;
        public float xValue;
        public float yValue;
    }

    [SerializeField] private List<GameObjectPlacement> _prefabPlacement;
    private int index = 0;
    private GameObject obj;

    public override void Apply()
    {
        index = Random.Range(0, _prefabPlacement.Count);
        obj = Instantiate(_prefabPlacement[index].targetObject, new Vector3(_prefabPlacement[index].xValue, _prefabPlacement[index].yValue, 0), Quaternion.identity);
        _detectData.AnomalyPos = obj.transform.position;
    }

    public override void Remove()
    {
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
