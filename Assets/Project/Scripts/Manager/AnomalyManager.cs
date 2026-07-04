using UnityEngine;
using System.Collections.Generic;

namespace SEHOON.GameSystem
{
    public class AnomalyManager : MonoBehaviour
    {
        [SerializeField] private List<AnomalyBase> _anomalies;

        private AnomalyBase _selectedAnomaly = null;
        private bool _isAnomalyApply = false;

        public bool IsAnomalyApply => _isAnomalyApply;

        private void Start()
        {
            SelectAnomaly();
            if (_selectedAnomaly != null)
            {
                _selectedAnomaly.Apply();
            }
        }

        private void OnDisable()
        {
            if (_selectedAnomaly != null)
            {
                _selectedAnomaly.Remove();
            }
        }

        private void SelectAnomaly()
        {
            if (Random.Range(0, 10) > 1)
            {
                int randomIndex = Random.Range(0, _anomalies.Count);
                _selectedAnomaly = _anomalies[randomIndex];
            }
        }
    }
}
