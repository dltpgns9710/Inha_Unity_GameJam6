using SEHOON.GameSystem;
//using Unity.AppUI.MVVM;
using UnityEngine;

public class AnomalyCreature_1 : AnomalyBase
{
    [SerializeField] private GameObject _attackRange;

    [Header("크리쳐")]
    [SerializeField] private CreatureTrigger _creature;

    public override void Apply()
    {
        _creature.SetAnomalyState();
        _attackRange.SetActive(true);

    }
    public override void Remove()
    {
        if (_attackRange != null)
            _attackRange.SetActive(false);
    }
}
