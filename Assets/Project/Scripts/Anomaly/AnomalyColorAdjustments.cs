using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;

public class ColorAdjustmentsAnomaly : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            globalVolume.profile.TryGet(out colorAdjustments);
        }
    }

    public override void Apply()
    {
        if (colorAdjustments != null)
        {
            colorAdjustments.saturation.value = -100f; // 채도를 -100으로 하면 완전 흑백 화면
            // colorAdjustments.colorFilter.value = new Color(1f, 0.5f, 0.5f); // 화면에 붉은 필터 적용
        }
    }

    public override void Remove()
    {
        if (colorAdjustments != null) 
        { 
            colorAdjustments.saturation.value = 0f; 
            colorAdjustments.colorFilter.value = Color.white; 
        }
    }
}