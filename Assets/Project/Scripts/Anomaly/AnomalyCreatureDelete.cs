using SEHOON.GameSystem;
using UnityEngine;

public class AnomalyCreatureDelete : AnomalyBase
{
    [Header("Creature")]
    [SerializeField] private GameObject _creature;

    public override void Apply()
    {
        if( _creature != null )
            _creature.SetActive(false);
    }

    public override void Remove()
    {
        if(_creature  != null)
            _creature.SetActive(true);
    }
}
