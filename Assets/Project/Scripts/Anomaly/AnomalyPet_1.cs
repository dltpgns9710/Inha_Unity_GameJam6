using SEHOON.GameSystem;
using UnityEngine;

public class AnomalyPet_1 : AnomalyBase
{
    [Header("Pet")]
    [SerializeField] private GameObject _pet;
    [SerializeField] private Vector3 _scale;

    private Vector3 _originalScale;

    public override void Apply()
    {
        _originalScale = _pet.transform.localScale;
        if(_pet != null )
            _pet.transform.localScale = _scale;
    }

    public override void Remove()
    {
        if (_pet != null)
            _pet.transform.localScale = _originalScale;
    }
}
