using SEHOON.GameSystem;

public class AnomalyMuteVolume : AnomalyBase
{
    private float _originalMasterVolume;
    private float _originalSfxVolume;
    private float _originalBgmVolume;

    public override void Apply()
    {
        _originalMasterVolume = SoundManager.Instance.GetMasterVolume();
        _originalSfxVolume = SoundManager.Instance.GetSfxVolume();
        _originalBgmVolume = SoundManager.Instance.GetBgmVolume();

        SoundManager.Instance.SetMasterVolume(0f);
        SoundManager.Instance.SetSfxVolume(0f);
        SoundManager.Instance.SetBgmVolume(0f);
    }

    public override void Remove()
    {
        SoundManager.Instance.SetMasterVolume(_originalMasterVolume);
        SoundManager.Instance.SetSfxVolume(_originalSfxVolume);
        SoundManager.Instance.SetBgmVolume(_originalBgmVolume);
    }
}
