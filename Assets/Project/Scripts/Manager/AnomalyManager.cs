using UnityEngine;
using System.Collections.Generic;

namespace SEHOON.GameSystem
{
    public class AnomalyManager : MonoBehaviour
    {
        [SerializeField] private List<AnomalyBase> _anomalies;

        private AnomalyBase _selectedAnomaly = null;


        private void Start()
        {
            DataManager.Instance.IsAnomalyApply = false;
            SelectAnomaly();
            if (_selectedAnomaly != null)
            {
                DataManager.Instance.IsAnomalyApply = true;
                _selectedAnomaly.Apply();
            }
        }

        private void OnDisable()
        {
            if (_selectedAnomaly != null)
            {
                _selectedAnomaly.Remove();
                DataManager.Instance.IsAnomalyApply = false;
            }
        }

        private void SelectAnomaly()
        {
            _selectedAnomaly = _anomalies[0];
            /*
            if (Random.Range(0, 10) > 1)
            {
                int randomIndex = Random.Range(0, _anomalies.Count);
                _selectedAnomaly = _anomalies[randomIndex];
                _isAnomalyApply = true;
            }
            */
        }

#if UNITY_EDITOR
        public int CurrentAnomalyIndex => _selectedAnomaly != null ? _anomalies.IndexOf(_selectedAnomaly) : -1;

        public void DebugApplyAnomaly(int index)
        {
            if (index < 0 || index >= _anomalies.Count) return;

            if (_selectedAnomaly != null) _selectedAnomaly.Remove();

            _selectedAnomaly = _anomalies[index];
            _selectedAnomaly.Apply();
            DataManager.Instance.IsAnomalyApply = true;
        }

        public void DebugRemoveAnomaly()
        {
            if (_selectedAnomaly == null) return;

            _selectedAnomaly.Remove();
            _selectedAnomaly = null;
            DataManager.Instance.IsAnomalyApply = false;
        }
#endif
    }
}
