using SEHOON.GameSystem;
using UnityEngine;

public class FlipObjectX : AnomalyBase
{
    [SerializeField] private GameObject[] _objectToFlip;
    private int index = 0;

    public override void Apply()
    {
        index = Random.Range(0, _objectToFlip.Length);
        _detectData.AnomalyPos = _objectToFlip[index].transform.position;
        _objectToFlip[index].GetComponent<SpriteRenderer>().flipX = true;
    }

    public override void Remove()
    {
        _objectToFlip[index].GetComponent<SpriteRenderer>().flipX = false;
    }
}
