using UnityEngine;

namespace SEHOON.UI
{
    public class HighlightMarker : MonoBehaviour
    {
        #region Serialized Fields
        [SerializeField] private Sprite _markerSprite;
        [SerializeField] private Vector3 _offset = new Vector3(0f, 1f, 0f);
        [SerializeField] private int _sortingOrder = 10;
        [SerializeField] private float _blinkInterval = 0.4f;
        #endregion

        #region Private Fields
        private GameObject _marker;
        private SpriteRenderer _markerRenderer;
        #endregion

        #region Unity Lifecycle
        private void OnEnable()
        {
            if (_markerSprite == null) return;

            _marker = new GameObject("HighlightMarkerIcon");
            _marker.transform.SetParent(transform, false);
            _marker.transform.localPosition = _offset;

            _markerRenderer = _marker.AddComponent<SpriteRenderer>();
            _markerRenderer.sprite = _markerSprite;
            _markerRenderer.sortingOrder = _sortingOrder;

            InvokeRepeating(nameof(ToggleVisibility), _blinkInterval, _blinkInterval);
        }

        private void OnDisable()
        {
            CancelInvoke(nameof(ToggleVisibility));

            if (_marker != null) Destroy(_marker);
        }
        #endregion

        #region Private Methods
        private void ToggleVisibility()
        {
            if (_markerRenderer != null) _markerRenderer.enabled = !_markerRenderer.enabled;
        }
        #endregion
    }
}
