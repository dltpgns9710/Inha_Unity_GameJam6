using System;
using System.Collections.Generic;
using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SEHOON.UI
{
    public class StorySceneController : MonoBehaviour
    {
        [Serializable]
        public struct StoryTypeEntry
        {
            public EStoryType StoryType;
            public GameObject StoryInstance;
            public string SceneName;
            public AudioClip AudioClip;
        }

        #region Serialized Fields
        [Header("Story Type Entries")]
        [SerializeField] private List<StoryTypeEntry> _storyTypeEntries = new List<StoryTypeEntry>();

        [Header("Fallback")]
        [SerializeField] private string _fallbackScene;
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            EStoryType currentStoryType = DataManager.Instance.StoryType;

            foreach (StoryTypeEntry entry in _storyTypeEntries)
            {
                entry.StoryInstance?.SetActive(entry.StoryType == currentStoryType);
            }
        }

        private void OnEnable()
        {
            AudioClip clip = GetCurrentAudioClip();
            if (clip == null) return;

            SoundManager.Instance.PlayBgm(clip);
        }

        private void OnDisable()
        {
            SoundManager.Instance.StopBgm();
        }
        #endregion

        #region Public Methods
        public void ChangeScene()
        {
            SceneManager.LoadScene(GetSceneToLoad());
        }
        #endregion

        #region Private Methods
        private string GetSceneToLoad()
        {
            EStoryType currentStoryType = DataManager.Instance.StoryType;

            foreach (StoryTypeEntry entry in _storyTypeEntries)
            {
                if (entry.StoryType == currentStoryType)
                {
                    return entry.SceneName;
                }
            }

            return _fallbackScene;
        }

        private AudioClip GetCurrentAudioClip()
        {
            EStoryType currentStoryType = DataManager.Instance.StoryType;

            foreach (StoryTypeEntry entry in _storyTypeEntries)
            {
                if (entry.StoryType == currentStoryType)
                {
                    return entry.AudioClip;
                }
            }

            return null;
        }
        #endregion
    }
}
