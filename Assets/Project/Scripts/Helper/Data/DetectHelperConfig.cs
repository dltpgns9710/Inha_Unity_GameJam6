using UnityEngine;

namespace TAEWOOK.Helper.Data
{
    [CreateAssetMenu(fileName = "DetectHelperConfig", menuName = "Helper/Detect Helper Config")]
    public class DetectHelperConfig : HelperConfig
    {
        #region Serialized Fields
        [Header("Detection")]
        [SerializeField] private float _commandSearchDistance = 6.0f;
        [SerializeField] private float _commandSearchArriveDistance = 0.5f;
        [SerializeField] private float _anomalyArriveDistance = 1.5f;
        [SerializeField] private LayerMask _anomalyLayer;

        [Header("Feedback")]
        [SerializeField] private GameObject _exclamationIconPrefab;
        [SerializeField] private Vector3 _exclamationIconOffset = new Vector3(0, 1.5f, 0);
        [SerializeField] private float _exclamationIconDuration = 0.75f;
        [SerializeField] private AudioClip _barkSound;
        #endregion

        #region Properties
        public float CommandSearchDistance => _commandSearchDistance;
        public float CommandSearchArriveDistance => _commandSearchArriveDistance;
        public float AnomalyArriveDistance => _anomalyArriveDistance;
        public LayerMask AnomalyLayer => _anomalyLayer;
        public GameObject ExclamationIconPrefab => _exclamationIconPrefab;
        public Vector3 ExclamationIconOffset => _exclamationIconOffset;
        public float ExclamationIconDuration => _exclamationIconDuration;
        public AudioClip BarkSound => _barkSound;
        #endregion
    }
}

