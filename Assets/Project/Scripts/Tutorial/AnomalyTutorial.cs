using UnityEngine;
using SEHOON.GameSystem;
public class AnomalyTutorial : AnomalyBase
{
    [Header("가짜 플레이어")]
    [SerializeField] private GameObject _fakePlayer;

    public override void Apply()
    {
        _fakePlayer.SetActive(true);
    }

    public override void Remove()
    {
        _fakePlayer.SetActive(false);
    }
    
}
