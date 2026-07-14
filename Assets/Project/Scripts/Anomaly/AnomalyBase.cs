using UnityEngine;

namespace SEHOON.GameSystem
{
    public abstract class AnomalyBase : MonoBehaviour
    {
        [SerializeField] private float _weight = 1f;

        public float Weight => _weight;

        public abstract void Apply();
        public abstract void Remove();
    }
}