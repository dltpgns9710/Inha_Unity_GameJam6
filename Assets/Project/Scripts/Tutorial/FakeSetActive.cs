using UnityEngine;
using SEHOON.GameSystem;

public class FakeSetActive : MonoBehaviour
{
    [SerializeField] private GameObject _entrance;
    [SerializeField] private GameObject _textBoxHitbox;

    [SerializeField] private AudioClip _disappearingSound;
    [SerializeField] private AudioClip _eventSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlaySfx(_disappearingSound);
        SoundManager.Instance.PlaySfx(_eventSound);
        SoundManager.Instance.StopBgm();
        gameObject.SetActive(false);
        _entrance.SetActive(true);
        _textBoxHitbox.SetActive(true);
    }
}
