using UnityEngine;

namespace TAEWOOK.Helper.Data
{
    [CreateAssetMenu(fileName = "ProtectHelperConfig", menuName = "Helper/Protect Helper Config")]
    public class ProtectHelperConfig : HelperConfig
    {
        #region Serialized Fields
        [Header("Protection")]
        [SerializeField] private float _protectRange = 3.0f;
        [SerializeField] private float _protectCooldown = 10.0f;
        [SerializeField] private LayerMask _hazardAnomalyLayer;

        [Header("Feedback")]
        [SerializeField] private GameObject _blockEffectPrefab;
        [SerializeField] private AudioClip _blockSound;
        #endregion

        #region Properties
        public float ProtectRange => _protectRange;
        public float ProtectCooldown => _protectCooldown;
        public LayerMask HazardAnomalyLayer => _hazardAnomalyLayer;
        public GameObject BlockEffectPrefab => _blockEffectPrefab;
        public AudioClip BlockSound => _blockSound;
        #endregion
    }
}

