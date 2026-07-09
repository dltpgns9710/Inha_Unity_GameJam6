using UnityEngine;
using TAEWOOK.Helper.Core;

namespace TAEWOOK.Helper.Ability
{
    public abstract class HelperAbility : MonoBehaviour
    {
        #region Protected Fields
        protected HelperControllar Helper;
        #endregion

        #region Public Methods
        public virtual bool IsActive => false;

        public virtual void Initialize(HelperControllar helper)
        {
            Helper = helper;
        }

        public abstract bool CanUseAbility();
        public abstract void UseAbility(Vector2 targetPosition);

        public virtual void TickAbility()
        {
        }

        public virtual void OnBarkAnimationEnd()
        {
        }
        #endregion
    }
}
