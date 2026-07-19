using System.Collections;
using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyPlaySfx : AnomalyBase
{
    [Header("효과음")]
    [SerializeField] private AudioClip _sfxClip;
    [SerializeField] private bool _isLoop;

    private Coroutine _loopCoroutine;

    public override void Apply()
    {
        if (_isLoop)
        {
            _loopCoroutine = StartCoroutine(CoLoopSfx());
        }
        else
        {
            SoundManager.Instance.PlaySfx(_sfxClip);
        }
    }

    public override void Remove()
    {
        if (_loopCoroutine != null)
        {
            StopCoroutine(_loopCoroutine);
            _loopCoroutine = null;
        }
    }

    private IEnumerator CoLoopSfx()
    {
        if (_sfxClip == null)
        {
            yield break;
        }

        while (true)
        {
            SoundManager.Instance.PlaySfx(_sfxClip);
            yield return new WaitForSeconds(_sfxClip.length);
        }
    }
}
