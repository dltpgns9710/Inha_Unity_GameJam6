using System;
using System.Collections.Generic;
using TAEWOOK.Helper.Core;
using UnityEngine;
using UnityEngine.UI;

namespace SEHOON.UI
{
    public class DetecteChanceCountView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("References")]
        [SerializeField] private RectTransform _iconContainer;
        [SerializeField] private Image _iconPrefab;
        [SerializeField] private GameObject _chanceCountSource;

        [Header("Icon")]
        [SerializeField] private Sprite _iconSprite;
        [SerializeField] private Vector2 _iconSize = new Vector2(32f, 32f);
        #endregion

        #region Private Fields
        private readonly List<Image> _icons = new List<Image>();
        private HelperControllar _helperComponent;
        private int _currentCount;
        #endregion

        #region Unity Lifecycle
        private void Start()
        {
            //todo : _chanceCountSource 에서 갯수 가져와 초기화
            _helperComponent = _chanceCountSource.GetComponent<HelperControllar>();
            if (_helperComponent == null) return;

            Initialize(_helperComponent.MaxCount);
            _helperComponent.HasDected += TryDetect;
        }

        private void OnDestroy()
        {
            if (_helperComponent == null) return;

            _helperComponent.HasDected -= TryDetect;
        }

        #endregion

        #region Public Methods
        public void Initialize(int maxCount)
        {
            ClearIcons();

            for (int i = 0; i < maxCount; i++)
            {
                Image icon = Instantiate(_iconPrefab, _iconContainer);
                icon.sprite = _iconSprite;
                icon.rectTransform.sizeDelta = _iconSize;
                _icons.Add(icon);
            }
            
            _currentCount = maxCount;
            
            //todo : _chanceCountSource 에서 갯수 변화 구독
        }
        #endregion

        #region Private Methods
        private void TryDetect()
        {
            if (_currentCount <= 0)
            {
                return;
            }

            _currentCount--;
            _icons[_currentCount].gameObject.SetActive(false);
        }
        private void ClearIcons()
        {
            foreach (Image icon in _icons)
            {
                if (icon != null)
                {
                    Destroy(icon.gameObject);
                }
            }

            _icons.Clear();
        }
        #endregion
    }
}
