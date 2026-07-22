using SEHOON.GameSystem;
using System;
using UnityEngine;

public class AnomalyRoom : AnomalyBase
{
    [SerializeField] private GameObject _Room;
    private GameObject obj;

    public override void Apply()
    {
        GameObject obj = Instantiate(_Room, new Vector3(25, 30, 0), Quaternion.identity);
    }

    public override void Remove()
    {
        if (obj != null)
        {
            Destroy(obj);
        }
    }
}
