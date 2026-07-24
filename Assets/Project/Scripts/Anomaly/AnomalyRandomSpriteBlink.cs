using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SEHOON.GameSystem;

public class AnomalyRandomSpriteBlink : AnomalyBase
{
    [Header("스프라이트")]
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Vector2 _spriteSize = new Vector2(200f, 200f);

    [Header("타이밍 (Visible: 스폰 주기 / Hidden: 개별 유지 시간)")]
    [SerializeField] private float _visibleDuration = 0.2f;
    [SerializeField] private float _hiddenDuration = 1f;

    [Header("페이드")]
    [SerializeField] private float _fadeInDuration = 0.2f;
    [SerializeField] private float _fadeOutDuration = 0.2f;

    [Header("회전")]
    [SerializeField, Range(0f, 180f)] private float _rotationRange = 15f;

    [Header("효과음")]
    [SerializeField] private AudioClip _appearSound;

    private RectTransform _canvasRect;
    private Coroutine _spawnCoroutine;
    private readonly List<GameObject> _activeInstances = new List<GameObject>();

    private void Awake()
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas != null)
        {
            _canvasRect = canvas.GetComponent<RectTransform>();
        }
    }

    public override void Apply()
    {
        if (_spawnCoroutine != null)
        {
            StopCoroutine(_spawnCoroutine);
        }

        _spawnCoroutine = StartCoroutine(CoSpawnLoop());
    }

    public override void Remove()
    {
        StopAllCoroutines();
        _spawnCoroutine = null;

        DespawnAll();
    }

    private IEnumerator CoSpawnLoop()
    {
        while (true)
        {
            StartCoroutine(CoSpriteLifecycle());
            yield return new WaitForSeconds(_visibleDuration);
        }
    }

    private IEnumerator CoSpriteLifecycle()
    {
        GameObject instance = SpawnAtRandomPosition();
        if (instance == null)
        {
            yield break;
        }

        _activeInstances.Add(instance);

        Image image = instance.GetComponent<Image>();
        yield return StartCoroutine(CoFade(image, 0f, 1f, _fadeInDuration));

        yield return new WaitForSeconds(_hiddenDuration);

        yield return StartCoroutine(CoFade(image, 1f, 0f, _fadeOutDuration));

        _activeInstances.Remove(instance);
        Destroy(instance);
    }

    private IEnumerator CoFade(Image image, float from, float to, float duration)
    {
        if (image == null)
        {
            yield break;
        }

        Color color = image.color;
        color.a = from;
        image.color = color;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(from, to, elapsed / duration);
            image.color = color;
            yield return null;
        }

        color.a = to;
        image.color = color;
    }

    private GameObject SpawnAtRandomPosition()
    {
        if (_canvasRect == null)
        {
            return null;
        }

        GameObject instance = new GameObject("AnomalyRandomSprite", typeof(RectTransform), typeof(Image));
        instance.transform.SetParent(_canvasRect, false);

        RectTransform instanceRect = instance.GetComponent<RectTransform>();
        instanceRect.sizeDelta = _spriteSize;
        instanceRect.anchoredPosition = GetRandomPosition();
        float rotationSign = Random.value < 0.5f ? 1f : -1f;
        float rotationZ = Random.Range(0f, _rotationRange) * rotationSign;
        instanceRect.localRotation = Quaternion.Euler(0f, 0f, rotationZ);

        Image image = instance.GetComponent<Image>();
        image.sprite = _sprite;
        image.raycastTarget = false;
        image.preserveAspect = true;

        if (_appearSound != null)
        {
            SoundManager.Instance.PlaySfx(_appearSound);
        }

        return instance;
    }

    private Vector2 GetRandomPosition()
    {
        Rect rect = _canvasRect.rect;
        float halfWidth = _spriteSize.x * 0.5f;
        float halfHeight = _spriteSize.y * 0.5f;

        float x = Random.Range(rect.xMin + halfWidth, rect.xMax - halfWidth);
        float y = Random.Range(rect.yMin + halfHeight, rect.yMax - halfHeight);
        return new Vector2(x, y);
    }

    private void DespawnAll()
    {
        foreach (GameObject instance in _activeInstances)
        {
            if (instance != null)
            {
                Destroy(instance);
            }
        }

        _activeInstances.Clear();
    }
}
