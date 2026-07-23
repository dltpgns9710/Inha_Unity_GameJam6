using UnityEngine;
using SEHOON.GameSystem;

public class PlayBGM : MonoBehaviour
{
    [SerializeField] private AudioClip _bgmClip;
    [SerializeField] private GameObject _trigger;
    private bool _triggered = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_triggered)
        {
            SoundManager.Instance.PlayBgm(_bgmClip);
            _trigger.SetActive(true);
            _triggered = true;
        }
        return;
    }
}
