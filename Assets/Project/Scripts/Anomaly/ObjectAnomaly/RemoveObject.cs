using SEHOON.GameSystem;
using UnityEngine;

public class RemoveObject : AnomalyBase
{
    [SerializeField] private GameObject[] _objectToRemove;
    private int index = 0;

    public override void Apply()
    {
        index = Random.Range(0, _objectToRemove.Length);
        _detectData.AnomalyPos = _objectToRemove[index].transform.position;
        _objectToRemove[index].SetActive(false);
    }

    public override void Remove()
    {
        _objectToRemove[index].SetActive(true);
    }
}
