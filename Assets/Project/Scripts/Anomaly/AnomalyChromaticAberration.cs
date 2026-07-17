using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;
using System.Collections; 

public class ChromaticAberrationAnomaly : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private ChromaticAberration chromaticAberration;

    [Header("효과 변화 설정")]
    public float targetIntensity = 1f;       
    public float transitionDuration = 10f;   

    private Coroutine transitionCoroutine; 

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
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionIntensity(targetIntensity));
        }
    }

    public override void Remove()
    {
        if (chromaticAberration != null)
        {
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            chromaticAberration.intensity.value = 0f;
        }
    }
    private IEnumerator TransitionIntensity(float targetValue)
    {
        float startValue = chromaticAberration.intensity.value;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            chromaticAberration.intensity.value = Mathf.Lerp(startValue, targetValue, elapsedTime / transitionDuration);
            yield return null;
        }
        chromaticAberration.intensity.value = targetValue;
    }
}