using SEHOON.GameSystem;
using UnityEngine;

public class AnomalyPet_2 : AnomalyBase
{
    [Header("Pet")]
    [SerializeField] private GameObject _pet;
    [SerializeField] private GameObject _horrorPet;

    public override void Apply()
    {
        if (_pet != null&&_horrorPet != null)
        {
            _pet.SetActive(false);
            _horrorPet.SetActive(true);
        }
    }

    public override void Remove()
    {
        if (_pet != null && _horrorPet != null)
        {
            _pet.SetActive(true);
            _horrorPet.SetActive(false);
        }
    }
}
