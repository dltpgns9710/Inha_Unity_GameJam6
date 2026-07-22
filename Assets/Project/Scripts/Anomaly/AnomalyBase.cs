using System;
using UnityEngine;

namespace SEHOON.GameSystem
{
    public enum EAnomalyType
    {
        Local,
        Global
    };
    public enum EHintType
    {
        Detected,
        instant,
    };

    [System.Serializable]
    public struct SDetectData
    {
        [SerializeField] private EAnomalyType _type;
        [SerializeField] private EHintType _hintType;
        [SerializeField] private Vector3 _anomalyPos;
        [SerializeField] private string _hintText;
        [SerializeField, Range(0, 60)] private float _detectRange;      

        public EAnomalyType Type => _type;
        public EHintType HintType => _hintType;
        public Vector3 AnomalyPos
        {
            get => _anomalyPos;
            set => _anomalyPos = value;
        }
        public float DetectRange => _detectRange;
        public string HintText => _hintText;        
    };
    
    public abstract class AnomalyBase : MonoBehaviour
    {
        [SerializeField] protected SDetectData _detectData;
        [SerializeField] private float _weight = 1f;
        [SerializeField] private bool _shouldGoBackAnomaly = true;
        
        public float Weight => _weight;
        public SDetectData DetectData => _detectData;
  
        public bool ShouldGoBackAnomaly => _shouldGoBackAnomaly;
           
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