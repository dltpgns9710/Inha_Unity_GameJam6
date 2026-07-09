using UnityEngine;

namespace TAEWOOK.Helper.Core
{
    [RequireComponent(typeof(Animator))]
    public class HelperAnimation : MonoBehaviour
    {
        #region Private Fields
        private Animator _animator;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            _animator = GetComponent<Animator>();

            Debug.Assert(_animator != null, "Animator가 연결되지 않았습니다.");
        }
        #endregion

        #region Public Methods
        public void PlayBark()
        {
            _animator.SetTrigger("Bark");
        }

        public void SetWaiting(bool isWaiting)
        {
            _animator.SetBool("IsWaiting", isWaiting);
        }

        public void SetSleeping(bool isSleeping)
        {
            _animator.SetBool("IsSleeping", isSleeping);
        }
        #endregion
    }
}
