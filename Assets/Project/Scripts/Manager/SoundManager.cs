using UnityEngine;
using USingleton;

namespace SEHOON.GameSystem
{
    public class SoundManager : Singleton<SoundManager>
    {
        #region Serialized Fields
        [Header("Audio Sources")]
        [SerializeField] private AudioSource _bgmSource;
        [SerializeField] private AudioSource _sfxSource;

        [Header("Default Values")]
        [SerializeField] private float _defaultMasterVolume = 1f;
        [SerializeField] private float _defaultSfxVolume = 1f;
        [SerializeField] private float _defaultBgmVolume = 1f;
        #endregion

        #region Private Fields
        private float _masterVolume = 1f;
        private float _sfxVolume = 1f;
        private float _bgmVolume = 1f;
        private bool _isMuted = false;

        private const string MasterVolumeKey = "SoundManager_MasterVolume";
        private const string SfxVolumeKey = "SoundManager_SfxVolume";
        private const string BgmVolumeKey = "SoundManager_BgmVolume";
        private const string MutedKey = "SoundManager_Muted";
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            if (Instance != this) return;

            SetupAudioSources();
            LoadSettings();
        }
        #endregion

        #region Public Methods - Playback
        public void PlaySfx(AudioClip clip, float volumeScale = 1f)
        {
            if (clip == null || _sfxSource == null || _isMuted) return;

            _sfxSource.PlayOneShot(clip, _masterVolume * _sfxVolume * volumeScale);
        }

        public void PlayBgm(AudioClip clip, bool loop = true)
        {
            if (clip == null || _bgmSource == null) return;
            if (_bgmSource.clip == clip && _bgmSource.isPlaying) return;

            _bgmSource.clip = clip;
            _bgmSource.loop = loop;
            ApplyBgmVolume();
            _bgmSource.Play();
        }

        public void StopBgm()
        {
            if (_bgmSource != null) _bgmSource.Stop();
        }

        public void PauseBgm()
        {
            if (_bgmSource != null) _bgmSource.Pause();
        }

        public void ResumeBgm()
        {
            if (_bgmSource != null) _bgmSource.UnPause();
        }
        #endregion

        #region Public Methods - Settings
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(MasterVolumeKey, _masterVolume);
            PlayerPrefs.Save();
            ApplyBgmVolume();
        }

        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(SfxVolumeKey, _sfxVolume);
            PlayerPrefs.Save();
        }

        public void SetBgmVolume(float volume)
        {
            _bgmVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat(BgmVolumeKey, _bgmVolume);
            PlayerPrefs.Save();
            ApplyBgmVolume();
        }

        public void SetMuted(bool muted)
        {
            _isMuted = muted;
            PlayerPrefs.SetInt(MutedKey, muted ? 1 : 0);
            PlayerPrefs.Save();
            ApplyBgmVolume();
        }

        public float GetMasterVolume() => _masterVolume;
        public float GetSfxVolume() => _sfxVolume;
        public float GetBgmVolume() => _bgmVolume;
        public bool IsMuted() => _isMuted;
        #endregion

        #region Private Methods
        private void SetupAudioSources()
        {
            if (_bgmSource == null) _bgmSource = gameObject.AddComponent<AudioSource>();
            if (_sfxSource == null) _sfxSource = gameObject.AddComponent<AudioSource>();

            _bgmSource.playOnAwake = false;
            _bgmSource.loop = true;

            _sfxSource.playOnAwake = false;
            _sfxSource.loop = false;
        }

        private void LoadSettings()
        {
            _masterVolume = PlayerPrefs.GetFloat(MasterVolumeKey, _defaultMasterVolume);
            _sfxVolume = PlayerPrefs.GetFloat(SfxVolumeKey, _defaultSfxVolume);
            _bgmVolume = PlayerPrefs.GetFloat(BgmVolumeKey, _defaultBgmVolume);
            _isMuted = PlayerPrefs.GetInt(MutedKey, 0) == 1;

            ApplyBgmVolume();
        }

        private void ApplyBgmVolume()
        {
            if (_bgmSource != null)
            {
                _bgmSource.volume = _isMuted ? 0f : _masterVolume * _bgmVolume;
            }
        }
        #endregion
    }
}
