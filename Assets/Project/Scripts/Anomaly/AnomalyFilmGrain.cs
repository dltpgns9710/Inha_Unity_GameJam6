using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;

public class AnomalyFilmGrain : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private FilmGrain filmGrain;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out filmGrain);
        }
    }

    public override void Apply()
    {
        if (filmGrain != null)
        {
            filmGrain.intensity.value = 1f; // 0~1 사이. 수치가 높을수록 모래알 노이즈가 심해짐
        }
    }

    public override void Remove()
    {
        if (filmGrain != null) 
        {
            filmGrain.intensity.value = 0f;
        }
    }
}