using UnityEngine;

namespace TAEWOOK.Helper.Data
{
    [CreateAssetMenu(fileName = "DetectHelperConfig", menuName = "Helper/Detect Helper Config")]
    public class DetectHelperConfig : HelperConfig
    {
        #region Serialized Fields
        [Header("Detection")]       
        [SerializeField] private float _commandSearchArriveDistance = 0.5f;               

        [Header("Feedback")]
        [SerializeField] private GameObject _exclamationIconPrefab;
        [SerializeField] private Vector3 _exclamationIconOffset = new Vector3(0, 1.5f, 0);
        [SerializeField] private float _exclamationIconDuration = 0.75f;        
        #endregion

        #region Properties       
        public float CommandSearchArriveDistance => _commandSearchArriveDistance;          
        public GameObject ExclamationIconPrefab => _exclamationIconPrefab;
        public Vector3 ExclamationIconOffset => _exclamationIconOffset;
        public float ExclamationIconDuration => _exclamationIconDuration;        
        #endregion
    }
}

