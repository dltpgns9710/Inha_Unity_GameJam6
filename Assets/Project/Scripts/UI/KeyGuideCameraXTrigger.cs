using System;
using System.Collections.Generic;
using UnityEngine;

namespace SEHOON.UI
{
    [Serializable]
    public class GuideAction
    {
        public EGuideTrigger Target;
        public KeyGuideInputTrigger.EAction Action;
    }

    public class KeyGuideCameraXTrigger : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Transform _camera;
        [SerializeField] private float _minX;

        [SerializeField] private KeyGuideView _keyGuideView;
        [SerializeField] private List<GuideAction> _guideActions;

        [SerializeField] private GameObject _activateTarget;
        [SerializeField] private GameObject _deactivateTarget;
        #endregion

        #region Unity Lifecycle
        private void Update()
        {
            if (_camera == null || _camera.position.x < _minX) return;

            Fire();
            enabled = false;
        }
        #endregion

        #region Private Methods
        private void Fire()
        {
            if (_keyGuideView != null && _guideActions != null)
            {
                foreach (GuideAction guideAction in _guideActions)
                {
                    if (guideAction.Action == KeyGuideInputTrigger.EAction.Show) _keyGuideView.Show(guideAction.Target);
                    else _keyGuideView.Hide(guideAction.Target);
                }
            }

            if (_activateTarget != null) _activateTarget.SetActive(true);
            if (_deactivateTarget != null) _deactivateTarget.SetActive(false);
        }
        #endregion
    }
}
