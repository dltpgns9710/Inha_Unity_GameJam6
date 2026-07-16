using SEHOON.GameSystem;
using UnityEngine;

namespace TAEWOOK.Helper.Detection
{
    public class HelperDetector : MonoBehaviour
    {
        [SerializeField] private AnomalyManager _anomalyManager;

        #region Private Fields        
        #endregion

        #region Public Methods
        public bool TryGetAnomaly(out AnomalyBase anomaly)
        {
            anomaly = null;

            if (_anomalyManager == null)
            {
                return false;
            }
            anomaly = _anomalyManager.SelectedAnomaly;

            if(anomaly == null)
            {
                return false;
            }

            SDetectData detectData = anomaly.DetectData;

            if (detectData.Type == EAnomalyType.Global)
            {
                anomaly = null;
                return false;
            }
            return true;
        }                                        

        public Transform FindAnomaly(Vector2 searchCenter)
        {
            if(!TryGetAnomaly(out AnomalyBase anomaly))
            {
                return null;
            }

            SDetectData detectData = anomaly.DetectData;
            Vector2 anomalyPosition = detectData.AnomalyPos;
            float detectRange = detectData.DetectRange;

            float distance = Vector2.Distance(searchCenter, anomalyPosition);

            

            if (distance > detectRange)
            {
                return null;
            }

            return anomaly.transform;
        }
        #endregion
    }
}

