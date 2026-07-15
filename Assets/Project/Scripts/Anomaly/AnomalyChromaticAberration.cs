using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;

public class ChromaticAberrationAnomaly : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private ChromaticAberration chromaticAberration;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out chromaticAberration);
        }
    }

    public override void Apply()
    {
        if (chromaticAberration != null)
        {
            chromaticAberration.intensity.value = 1f; // 0~1 사이. 수치가 높을수록 어지러움 심화
        }
    }

    public override void Remove()
    {
        if (chromaticAberration != null) 
        {
            chromaticAberration.intensity.value = 0f;
        }
    }
}