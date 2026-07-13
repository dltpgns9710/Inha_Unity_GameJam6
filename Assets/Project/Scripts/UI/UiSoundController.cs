using System;
using SEHOON.GameSystem;
using UnityEngine;
using USingleton;

namespace SEHOON.UI
{
    public class UiSoundController : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Audio Sources")]
        [SerializeField] private AudioClip _bgmSource;
        #endregion

        #region Private Fields
        private AudioClip _previousBgmClip;
        #endregion

        private void OnEnable()
        {
            _previousBgmClip = SoundManager.Instance.GetCurrentBgmClip();
            SoundManager.Instance.PlayBgm(_bgmSource);
        }

        private void OnDisable()
        {
            if (_previousBgmClip != null)
            {
                SoundManager.Instance.PlayBgm(_previousBgmClip);
            }
            else
            {
                SoundManager.Instance.StopBgm();
            }
        }
    }
}