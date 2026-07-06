using UnityEngine;

namespace TAEWOOK.Helper
{
    public class HelperDetector : MonoBehaviour
    {
        #region Private Fields
        private float _searchDistance;
        private LayerMask _anomalyLayer;
        #endregion

        #region Public Methods
        public void Initialize(float searchDistance, LayerMask anomalyLayer)
        {
            _searchDistance = searchDistance;
            _anomalyLayer = anomalyLayer;
        }

        public Transform FindNearestAnomaly(Vector2 searchCenter)
        {
            Collider2D[] detectColliders = Physics2D.OverlapCircleAll(
                searchCenter,
                _searchDistance,
                _anomalyLayer);

            Transform nearestAnomaly = null;
            float nearestDistance = float.MaxValue;

            foreach (Collider2D detectCollider in detectColliders)
            {
                float distance = Vector2.Distance(searchCenter, detectCollider.transform.position);

                if (distance >= nearestDistance)
                {
                    continue;
                }

                nearestDistance = distance;
                nearestAnomaly = detectCollider.transform;
            }

            return nearestAnomaly;
        }
        #endregion
    }
}

