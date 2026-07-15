using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;

public class AnomalyVignette : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private Vignette vignette;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out vignette);
        }
    }

    public override void Apply()
    {
        if (vignette != null)
        {
            vignette.intensity.value = 0.6f; // 0~1 사이. 수치가 높을수록 테두리가 좁게 닫힘
            vignette.color.value = Color.black; // 핏빛(Color.red)으로 바꾸면 치명상을 입은 느낌을 줍니다.
        }
    }

    public override void Remove()
    {
        if (vignette != null) 
        { 
            vignette.intensity.value = 0f; 
            vignette.color.value = Color.black; 
        }
    }
}