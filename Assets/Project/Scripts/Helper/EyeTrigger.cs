using UnityEngine;
using SEHOON.GameSystem;
public class Eye : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private string _playerTag = "Player";
    private bool _hasTriggered = false;
    private SpriteRenderer _eyeSprite;
    [SerializeField] private AudioClip _eyeClip;

    private void Awake()
    {
        _eyeSprite = GetComponent<SpriteRenderer>();
        SetAlpha(0f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(_hasTriggered || !other.CompareTag(_playerTag))
        {
            return;
        }
        _hasTriggered = true;
        SetAlpha(1f);
        SoundManager.Instance.PlaySfx(_eyeClip);
        _animator.SetTrigger("isOpen");        
    }

    private void SetAlpha(float alpha)
    {
        Color color = _eyeSprite.color;
        color.a = alpha;
        _eyeSprite.color = color;
    }
}
