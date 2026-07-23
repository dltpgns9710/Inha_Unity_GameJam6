using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;

public class LensDistortionAnomaly : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private LensDistortion lensDistortion;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out lensDistortion);
        }
    }

    public override void Apply()
    {
        if (lensDistortion != null)
        {
            lensDistortion.intensity.value = -0.5f; //화면 왜곡 정도
        }
    }

    public override void Remove()
    {
        if (lensDistortion != null) 
        {
            lensDistortion.intensity.value = 0f;
        }
    }
}