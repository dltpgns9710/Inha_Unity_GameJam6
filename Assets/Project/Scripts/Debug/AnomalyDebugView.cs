#if UNITY_EDITOR
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SEHOON.GameSystem;

namespace SEHOON.UI
{
    public class AnomalyDebugView : MonoBehaviour
    {
        #region Serialized Fields
        [Header("Target")]
        [SerializeField] private AnomalyManager _anomalyManager;

        [Header("Widgets")]
        [SerializeField] private TMP_InputField _indexInputField;
        [SerializeField] private TextMeshProUGUI _currentIndexText;
        [SerializeField] private Button _applyButton;
        [SerializeField] private Button _removeButton;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (_anomalyManager == null) _anomalyManager = FindAnyObjectByType<AnomalyManager>();
        }

        private void OnEnable()
        {
            _applyButton?.onClick.AddListener(OnApplyButtonClicked);
            _removeButton?.onClick.AddListener(OnRemoveButtonClicked);

            RefreshCurrentIndexText();
            _anomalyManager.ChangeAnomalyIndex += RefreshCurrentIndexText;
        }

        private void OnDisable()
        {
            _applyButton?.onClick.RemoveListener(OnApplyButtonClicked);
            _removeButton?.onClick.RemoveListener(OnRemoveButtonClicked);
        }
        #endregion

        #region Private Methods
        private void OnApplyButtonClicked()
        {
            if (_anomalyManager == null || _indexInputField == null) return;
            if (!int.TryParse(_indexInputField.text, out int index)) return;

            _anomalyManager.DebugApplyAnomaly(index);
            RefreshCurrentIndexText();
            DataManager.Instance.IsAnomalyApply = true;
        }

        private void OnRemoveButtonClicked()
        {
            if (_anomalyManager == null) return;

            _anomalyManager.DebugRemoveAnomaly();
            RefreshCurrentIndexText();
            DataManager.Instance.IsAnomalyApply = false;
        }

        private void RefreshCurrentIndexText()
        {
            if (_currentIndexText == null || _anomalyManager == null) return;

            _currentIndexText.text = $"Apply Index: {_anomalyManager.CurrentAnomalyIndex}";
        }
        #endregion
    }
}
#endif
