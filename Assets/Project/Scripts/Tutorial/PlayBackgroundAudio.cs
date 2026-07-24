using UnityEngine;
using SEHOON.GameSystem;

public class PlayBackgroundAudio : MonoBehaviour
{
    [SerializeField] private AudioClip _backgroundMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.PlayBgm(_backgroundMusic, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
