using UnityEngine;

namespace SEHOON.GameSystem
{
    public enum EAnomalyType
    {
        Local,
        Global
    };
    
    [System.Serializable]
    public struct SDetectData
    {
        [SerializeField] private EAnomalyType _type;
        [SerializeField] private Vector3 _anomalyPos;
        [SerializeField, Range(0, 60)] private float _detectRange;

        public EAnomalyType Type => _type;
        public Vector3 AnomalyPos => _anomalyPos;
        public float DetectRange => _detectRange;
    };
    
    public abstract class AnomalyBase : MonoBehaviour
    {
        [SerializeField] private SDetectData _detectData;
        [SerializeField] private float _weight = 1f;      

        public float Weight => _weight;
        public SDetectData DetectData => _detectData;
           
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (_detectData.Type == EAnomalyType.Global)
            {
                return;
            }

            Vector3 detectPosition = _detectData.AnomalyPos;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(detectPosition, _detectData.DetectRange);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(detectPosition, 0.15f);
        }
#endif

        public abstract void Apply();
        public abstract void Remove();
    }
}