using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SEHOON.UI
{
    public class BlinkObjects : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private List<Image> _blinkSprites;
        [SerializeField] private float _blinkInterval = 0.4f;
        #endregion
        
        #region Unity Lifecycle

        private void OnEnable()
        {
            InvokeRepeating(nameof(ToggleVisibility), _blinkInterval, _blinkInterval);
        }

        #endregion

        #region Private Methods
        private void ToggleVisibility()
        {
            if (_blinkSprites == null || _blinkSprites.Count == 0) return;
            foreach(Image blinkSprites in _blinkSprites)
            {
                blinkSprites.enabled = !blinkSprites.enabled;
            }
        }
        #endregion
    }
}
