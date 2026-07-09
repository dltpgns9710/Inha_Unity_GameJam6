using UnityEngine;

namespace TAEWOOK.Helper.Data
{
    public abstract class HelperConfig : ScriptableObject
    {
        #region Serialized Fields
        [Header("Movement")]
        [SerializeField] private float _walkSpeed = 3.0f;
        [SerializeField] private float _runSpeed = 6.0f;
        [SerializeField] private float _runDistance = 4.0f;
        [SerializeField] private float _followDistance = 1.5f;

        [Header("Behavior")]
        [SerializeField] private float _sleepDelay = 5.0f;
        #endregion

        #region Properties
        public float WalkSpeed => _walkSpeed;
        public float RunSpeed => _runSpeed;
        public float RunDistance => _runDistance;
        public float FollowDistance => _followDistance;
        public float SleepDelay => _sleepDelay;
        #endregion
    }
}

