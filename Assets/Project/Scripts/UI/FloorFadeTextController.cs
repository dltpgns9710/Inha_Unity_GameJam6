using SEHOON.GameSystem;
using UnityEngine;

namespace SEHOON.UI
{
    [RequireComponent(typeof(FadeTextView))]
    public class FloorFadeTextController : MonoBehaviour
    {
        #region Private Fields
        private FadeTextView _fadeTextView;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _fadeTextView = GetComponent<FadeTextView>();
        }

        private void OnEnable()
        {
            _fadeTextView.SetText($"Floor {DataManager.Instance.Floor}");
        }
        #endregion
    }
}
