using UnityEngine;

namespace SEHOON.GameSystem
{
    public abstract class AnomalyBase : MonoBehaviour
    {
        public abstract void Apply();
        public abstract void Remove(); 
    }
}