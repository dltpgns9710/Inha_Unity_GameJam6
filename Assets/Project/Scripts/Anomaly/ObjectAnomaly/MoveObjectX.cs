using SEHOON.GameSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveObject : AnomalyBase
{
    [System.Serializable]
    public struct GameObjectFloatPair
    {
        public GameObject targetObject;
        public float value;
    }

    [SerializeField] private List<GameObjectFloatPair> pairList;
    private int index = 0;

    public override void Apply()
    {
        index = Random.Range(0, pairList.Count);
        pairList[index].targetObject.transform.position += new Vector3(pairList[index].value, 0, 0);
        _detectData.AnomalyPos = pairList[index].targetObject.transform.position;
    }

    public override void Remove()
    {
        if (pairList != null)
        {
            pairList[index].targetObject.transform.position -= new Vector3(pairList[index].value, 0, 0);
        }
    }
}
