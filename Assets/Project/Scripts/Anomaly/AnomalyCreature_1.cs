using SEHOON.GameSystem;
using Unity.AppUI.MVVM;
using UnityEngine;

public class AnomalyCreature_1 : AnomalyBase
{


    [Header("크리쳐")]
    [SerializeField] private CreatureTrigger _creature;

    public override void Apply()
    {
        _creature.SetAnomalyState();
    }
    public override void Remove()
    {

    }
}
