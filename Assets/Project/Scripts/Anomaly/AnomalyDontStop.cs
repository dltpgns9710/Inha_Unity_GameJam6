using System.Collections;
using UnityEngine;
using TMPro;
using SEHOON.GameSystem;
using SEHOON.UI;

public class AnomalyDontStop : AnomalyBase
{
    [Header("일시정지 메뉴 참조")]
    [SerializeField] private GameObject _pauseMenuObject;

    [Header("경고 텍스트")]
    [SerializeField] private GameObject _warningTextPrefab;
    [SerializeField] private string _warningMessage = "계속 해";

    [Header("강제 재개")]
    [SerializeField] private float _forceResumeDelay = 0.3f;

    [Header("사운드")]
    [SerializeField] private AudioClip _warningSound;

    [Header("등장 연출")]
    [SerializeField] private float _punchScale = 2f;
    [SerializeField] private float _punchDuration = 0.05f;

    private PauseMenuView _pauseMenuView;
    private GameObject _warningTextInstance;
    private TextMeshProUGUI _warningTextWidget;
    private Coroutine _forceResumeCoroutine;
    private Coroutine _showCoroutine;

    private void Awake()
    {
        if (_pauseMenuObject != null)
        {
            _pauseMenuView = _pauseMenuObject.GetComponent<PauseMenuView>();
        }
    }

    public override void Apply()
    {
        if (_pauseMenuView != null)
        {
            _pauseMenuView.OnEnableEvent.AddListener(OnPauseMenuOpened);
        }

        SpawnWarningText();
    }

    public override void Remove()
    {
        if (_pauseMenuView != null)
        {
            _pauseMenuView.OnEnableEvent.RemoveListener(OnPauseMenuOpened);
        }

        if (_forceResumeCoroutine != null)
        {
            StopCoroutine(_forceResumeCoroutine);
            _forceResumeCoroutine = null;
        }

        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
            _showCoroutine = null;
        }

        DespawnWarningText();
    }

    private void OnPauseMenuOpened()
    {
        if (_warningTextWidget != null)
        {
            _warningTextWidget.text = _warningMessage;
        }

        if (_warningSound != null)
        {
            SoundManager.Instance.PlaySfx(_warningSound);
        }

        if (_showCoroutine != null)
        {
            StopCoroutine(_showCoroutine);
        }

        _showCoroutine = StartCoroutine(CoShowWarningText());

        if (_forceResumeCoroutine != null)
        {
            StopCoroutine(_forceResumeCoroutine);
        }

        _forceResumeCoroutine = StartCoroutine(CoForceResume());
    }

    private IEnumerator CoShowWarningText()
    {
        if (_warningTextInstance == null)
        {
            yield break;
        }

        _warningTextInstance.transform.localScale = Vector3.one * _punchScale;
        _warningTextInstance.SetActive(true);

        float elapsed = 0f;
        while (elapsed < _punchDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / _punchDuration);
            _warningTextInstance.transform.localScale = Vector3.Lerp(Vector3.one * _punchScale, Vector3.one, t);
            yield return null;
        }

        _warningTextInstance.transform.localScale = Vector3.one;
        _showCoroutine = null;
    }

    private IEnumerator CoForceResume()
    {
        yield return new WaitForSecondsRealtime(_forceResumeDelay);

        _forceResumeCoroutine = null;
        ForceResume();
    }

    private void ForceResume()
    {
        if (_warningTextInstance != null)
        {
            _warningTextInstance.SetActive(false);
            _warningTextInstance.transform.localScale = Vector3.one;
        }

        if (_pauseMenuObject != null)
        {
            _pauseMenuObject.SetActive(false);
        }
    }

    private void SpawnWarningText()
    {
        if (_warningTextPrefab == null || _warningTextInstance != null)
        {
            return;
        }

        Transform parent = _pauseMenuObject != null ? _pauseMenuObject.transform.parent : null;
        _warningTextInstance = Instantiate(_warningTextPrefab, parent);
        _warningTextWidget = _warningTextInstance.GetComponentInChildren<TextMeshProUGUI>(true);
        _warningTextInstance.SetActive(false);
    }

    private void DespawnWarningText()
    {
        if (_warningTextInstance != null)
        {
            Destroy(_warningTextInstance);
            _warningTextInstance = null;
            _warningTextWidget = null;
        }
    }
}
