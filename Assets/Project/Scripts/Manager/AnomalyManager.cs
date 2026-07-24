using System;
using UnityEngine;
using System.Collections.Generic;
using SEHOON.UI;
using Random = UnityEngine.Random;

namespace SEHOON.GameSystem
{
    public class AnomalyManager : MonoBehaviour
    {
        [SerializeField] private List<AnomalyBase> _anomalies;
        [SerializeField, Range(0, 100)] private int _anomalyApplyChance = 80;
        [SerializeField] private WhiteboardView _whiteboardView;

        private AnomalyBase _selectedAnomaly = null;

        public AnomalyBase SelectedAnomaly => _selectedAnomaly;


        private void OnEnable()
        {
            DataManager.Instance.IsAnomalyApply = false;
            Invoke("TryApplyAnomaly", .5f);
        }

        private void OnDisable()
        {
            if (_selectedAnomaly != null)
            {
                _selectedAnomaly.Remove();
                DataManager.Instance.IsAnomalyApply = false;
            }

            _whiteboardView?.ClearHint();
        }

        private void TryApplyAnomaly()
        {
            if (DataManager.Instance.Floor == 1) return;

            SelectAnomaly();
            if (_selectedAnomaly != null)
            {
                DataManager.Instance.IsAnomalyApply = _selectedAnomaly.ShouldGoBackAnomaly;
                _selectedAnomaly.Apply();
                UpdateWhiteboardHint();
            }
            else
            {
                _whiteboardView?.ClearHint();
            }
#if UNITY_EDITOR
            ChangeAnomalyIndex?.Invoke();
#endif
        }

        private void UpdateWhiteboardHint()
        {
            if (_whiteboardView == null) return;

            if (_selectedAnomaly.DetectData.Type == EAnomalyType.Global)
            {
                _whiteboardView.ShowHint(_selectedAnomaly.DetectData.HintText);
            }
            else
            {
                _whiteboardView.ClearHint();
            }
        }
        
        private void SelectAnomaly()
        {
            if (_anomalies == null || _anomalies.Count == 0) return;

            if (Random.Range(0, 100) < _anomalyApplyChance)
            {
                _selectedAnomaly = GetWeightedRandomAnomaly();
            }
        }

        private AnomalyBase GetWeightedRandomAnomaly()
        {
            float totalWeight = 0f;
            foreach (AnomalyBase anomaly in _anomalies)
            {
                totalWeight += anomaly.Weight;
            }

            float randomPoint = Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;
            foreach (AnomalyBase anomaly in _anomalies)
            {
                cumulativeWeight += anomaly.Weight;
                if (randomPoint < cumulativeWeight) return anomaly;
            }

            return _anomalies[_anomalies.Count - 1];
        }

#if UNITY_EDITOR
        public int CurrentAnomalyIndex => _selectedAnomaly != null ? _anomalies.IndexOf(_selectedAnomaly) : -1;

        public event Action ChangeAnomalyIndex;
        public void DebugApplyAnomaly(int index)
        {
            if (index < 0 || index >= _anomalies.Count) return;

            if (_selectedAnomaly != null) _selectedAnomaly.Remove();

            _selectedAnomaly = _anomalies[index];
            _selectedAnomaly.Apply();
            UpdateWhiteboardHint();
            ChangeAnomalyIndex?.Invoke();
            DataManager.Instance.IsAnomalyApply = _selectedAnomaly.ShouldGoBackAnomaly;
        }

        public void DebugRemoveAnomaly()
        {
            if (_selectedAnomaly == null) return;

            _selectedAnomaly.Remove();
            _selectedAnomaly = null;
            _whiteboardView?.ClearHint();
            ChangeAnomalyIndex?.Invoke();
            DataManager.Instance.IsAnomalyApply = false;
        }
#endif
    }
}
