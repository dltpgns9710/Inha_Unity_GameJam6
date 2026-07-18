using SEHOON.GameSystem;
using UnityEngine;

public class AnomalyPlayer_1 : AnomalyBase
{
    [Header("Player")]
    [SerializeField] private GameObject _player;

    public float blinkSpeed = 3;

    public override void Apply()
    {
        InvokeRepeating("ToggleBlink", 0f, blinkSpeed);
    }

    public override void Remove()
    {
        InvokeRepeating("ToggleBlink", 0f, blinkSpeed);
    }

    private void ToggleBlink()
    {
        if (_player != null)
        {
            _player.SetActive(!_player.activeSelf);
        }
    }
}
