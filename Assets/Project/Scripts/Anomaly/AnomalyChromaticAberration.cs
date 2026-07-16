using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using SEHOON.GameSystem;
using System.Collections; // Coroutine을 사용하기 위해 필요합니다.

public class ChromaticAberrationAnomaly : AnomalyBase
{
    [Header("에디터에서 Global Volume 오브젝트를 넣어주세요")]
    public Volume globalVolume;
    private ChromaticAberration chromaticAberration;

    [Header("효과 변화 설정")]
    public float targetIntensity = 1f;       // 도달할 최대 어지러움 수치
    public float transitionDuration = 10f;   // 목표 수치까지 도달하는 데 걸리는 시간 (예: 10초)

    private Coroutine transitionCoroutine; // 실행 중인 코루틴을 추적

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
            // 이미 변화 중이라면 정지시키고 새로 시작 (부드러운 전환을 위해)
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            transitionCoroutine = StartCoroutine(TransitionIntensity(targetIntensity));
        }
    }

    public override void Remove()
    {
        if (chromaticAberration != null)
        {
            // 이상현상이 해제될 때도 원래 수치(0)로 서서히 돌아가게 합니다.
            if (transitionCoroutine != null) StopCoroutine(transitionCoroutine);
            chromaticAberration.intensity.value = 0f;
        }
    }

    // 시간에 따라 수치를 서서히 변경하는 코루틴
    private IEnumerator TransitionIntensity(float targetValue)
    {
        float startValue = chromaticAberration.intensity.value;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            // Mathf.Lerp를 이용해 현재 값에서 목표 값으로 시간에 맞춰 보간합니다.
            chromaticAberration.intensity.value = Mathf.Lerp(startValue, targetValue, elapsedTime / transitionDuration);
            yield return null; // 다음 프레임까지 대기
        }

        // 마지막에 정확한 목표치를 강제로 맞춰줍니다.
        chromaticAberration.intensity.value = targetValue;
    }
}