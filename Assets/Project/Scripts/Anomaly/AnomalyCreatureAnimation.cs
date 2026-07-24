using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyCreatureAnimation : AnomalyBase
{
    [Header("Creature")]
    public CreatureTrigger targetCreature;

    private Animator _creatureAnimator;

    private void Start()
    {
        if (targetCreature != null)
        {
            _creatureAnimator = targetCreature.GetComponent<Animator>();
        }
    }

    public override void Apply()
    {
        if (_creatureAnimator != null)
        {
            _creatureAnimator.enabled = false;
        }
    }

    public override void Remove()
    {
        if (_creatureAnimator != null)
        {
            _creatureAnimator.enabled = true;

        }
    }
}
