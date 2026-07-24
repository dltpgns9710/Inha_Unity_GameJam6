using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyBgm : AnomalyBase
{
    [Header("BGM")]
    [SerializeField] private AudioClip _bgmClip;

    public override void Apply()
    {
        SoundManager.Instance.PlayBgm(_bgmClip);
    }

    public override void Remove()
    {
        SoundManager.Instance.StopBgm();
    }
}
