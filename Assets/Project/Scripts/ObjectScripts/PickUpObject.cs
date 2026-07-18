using Unity.VisualScripting;
using UnityEngine;
using JUNBEOM.Player;
using System.Collections;
using SEHOON.GameSystem;

public class PickUpObject : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _keyObject;

    [SerializeField] private AudioClip _collectSound;

    [SerializeField] private float _animationDuration = 0.5f;

    private bool _hasInteracted = false;

    void Start()
    {
        
    }

    void Update()
    {
       
    }

    public void Interact(GameObject interactor)
    {
        PlayerEventManager.Instance.OnKeyCollected?.Invoke();
        if (!_hasInteracted)
        {
            SoundManager.Instance.PlaySfx(_collectSound);
            StartCoroutine(AnimateKeyObject());
        }

        _hasInteracted = true;
    }

    private IEnumerator AnimateKeyObject()
    {
        Transform keyTransform = _keyObject.transform;
        SpriteRenderer spriteRenderer = _keyObject.GetComponent<SpriteRenderer>();

        Vector3 startPos = keyTransform.position;
        float elapsed = 0f;

        while (elapsed < _animationDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _animationDuration;

            float easedProgress = 1 - (1 - progress) * (1 - progress);

            keyTransform.Rotate(0, 360 * Time.deltaTime / _animationDuration, 0);

            keyTransform.position = startPos + Vector3.up * (easedProgress * 2f);

            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(1f, 0f, progress);
                spriteRenderer.color = color;
            }

            yield return null;
        }

        _keyObject.SetActive(false);
    }
}
