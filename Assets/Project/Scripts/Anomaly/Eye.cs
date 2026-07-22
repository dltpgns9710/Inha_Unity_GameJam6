using UnityEngine;
using SEHOON.GameSystem;

public class Eye : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioClip _eyeClip;

    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    private static readonly int ClosedState = Animator.StringToHash("Closed");

    private SpriteRenderer _eyeSprite;

    private void Awake()
    {
        _eyeSprite = GetComponent<SpriteRenderer>();
        SetAlpha(0f);
    }

    public void Activate(Vector3 position, bool playSound)
    {
        transform.position = position;
        gameObject.SetActive(true);
        SetAlpha(1f);

        if (_animator != null)
        {
            _animator.Play(ClosedState, 0, 0f);
            _animator.ResetTrigger(IsOpen);
            _animator.SetTrigger(IsOpen);
        }

        if (playSound && _eyeClip != null)
        {
            SoundManager.Instance.PlaySfx(_eyeClip);
        }
    }

    public void Deactivate()
    {
        SetAlpha(0f);

        if (_animator != null)
        {
            _animator.ResetTrigger(IsOpen);
            _animator.Play(ClosedState, 0, 0f);
        }

        gameObject.SetActive(false);
    }

    private void SetAlpha(float alpha)
    {
        if (_eyeSprite == null)
        {
            return;
        }

        Color color = _eyeSprite.color;
        color.a = alpha;
        _eyeSprite.color = color;
    }
}