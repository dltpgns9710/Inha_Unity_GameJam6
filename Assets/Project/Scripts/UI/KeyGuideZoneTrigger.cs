using UnityEngine;

namespace SEHOON.UI
{
    public class KeyGuideZoneTrigger : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private string _playerTag = "Player";

        [SerializeField] private KeyGuideView _keyGuideView;
        [SerializeField] private EGuideTrigger _target;
        [SerializeField] private KeyGuideInputTrigger.EAction _action;

        [SerializeField] private GameObject _setActiveTarget;
        [SerializeField] private bool _setActiveValue = true;
        #endregion

        #region Unity Lifecycle
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(_playerTag)) return;

            if (_keyGuideView != null)
            {
                if (_action == KeyGuideInputTrigger.EAction.Show) _keyGuideView.Show(_target);
                else _keyGuideView.Hide(_target);
            }

            if (_setActiveTarget != null) _setActiveTarget.SetActive(_setActiveValue);
        }
        #endregion
    }
}
