using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;
using System.Collections;

public class ColorAdjustmentsAnomaly : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private ColorAdjustments colorAdjustments;

    [Header("효과 변화 설정")]
    public float targetIntensity = -100f;
    public float transitionDuration = 10f;

    private Coroutine transitionCoroutine;
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
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionIntensity(targetIntensity));
            //colorAdjustments.colorFilter.value = new Color(1f, 0.5f, 0.5f); // 화면에 붉은 필터 적용
        }
    }

    public override void Remove()
    {
        if (colorAdjustments != null) 
        {
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            colorAdjustments.saturation.value = 0f; 
            //colorAdjustments.colorFilter.value = Color.white; 
        }
    }
    private IEnumerator TransitionIntensity(float targetValue)
    {
        float startValue = colorAdjustments.saturation.value;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            colorAdjustments.saturation.value = Mathf.Lerp(startValue, targetValue, elapsedTime / transitionDuration);
            yield return null;
        }
        colorAdjustments.saturation.value = targetValue;
    }
}