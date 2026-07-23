using SEHOON.UI;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DisableObjectInTrigger : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private string _playerTag = "Player";

    [SerializeField] private List<GameObject> _deactivateTargets;
    #endregion

    #region Unity Lifecycle
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(_playerTag)) return;

        if (_deactivateTargets != null && _deactivateTargets.Count != 0)
        {
            foreach (GameObject _deactivateTarget in _deactivateTargets)
            {
                _deactivateTarget.SetActive(false);
            }
        }
    }
    #endregion
}
