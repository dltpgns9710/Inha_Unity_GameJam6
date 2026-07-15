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
        [SerializeField] EAnomalyType Type;
        [SerializeField] Vector3 AnomalyPos;
        [SerializeField, Range(0, 60)]float DetectRange;
    };
    
    public abstract class AnomalyBase : MonoBehaviour
    {
        [SerializeField] private SDetectData _detectData;
        [SerializeField] private float _weight = 1f;

        public float Weight => _weight;
        public SDetectData DetectData => _detectData;
        
        public abstract void Apply();
        public abstract void Remove();
    }
}