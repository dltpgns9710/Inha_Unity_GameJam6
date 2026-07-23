using UnityEngine;
using SEHOON.GameSystem;

public class StopBG : MonoBehaviour
{
    [SerializeField] private GameObject _door;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (_door != null)
        {
            _door.SetActive(false);
        }
        SoundManager.Instance.StopBgm();
    }
}
