using UnityEngine;
using TAEWOOK.Helper.Data;

namespace TAEWOOK.Helper.Ability
{
    public class ProtectHelperAbility : HelperAbility
    {
       
        #region Private Fields
        private ProtectHelperConfig _config;
        private float _lastProtectTime = float.NegativeInfinity;
        #endregion

        #region Public Methods
        public override void Initialize(Core.HelperControllar helper)
        {
            base.Initialize(helper);

            if (_config == null)
            {
                _config = helper.Config as ProtectHelperConfig;
            }
        }

        public override bool CanUseAbility()
        {
            if (Helper == null || _config == null)
            {
                return false;
            }

            return !IsActive && Time.time >= _lastProtectTime + _config.ProtectCooldown;
        }

        public override void UseAbility(Vector2 targetPosition)
        {
            if (!CanUseAbility())
            {
                return;
            }

            _lastProtectTime = Time.time;
        }
        #endregion
    }
}
