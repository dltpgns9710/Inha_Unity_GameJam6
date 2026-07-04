using JUNBEOM.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace JUNBEOM
{
    public class SettingsUIController : MonoBehaviour
    {
        #region Serialized Fields

        [Header("UI")]
        [SerializeField] private UIDocument _settingsUIDocument;

        [Header("References")]
        [SerializeField] private PlayerInputManager _inputManager;

        #endregion

        #region Private Fields

        private VisualElement _settingsRoot;
        private bool _isSettingsOpen;

        #endregion

        #region Properties

        public bool IsSettingsOpen => _isSettingsOpen;

        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            Debug.Assert(_settingsUIDocument != null, $"[{name}] Settings UIDocument 누락");
            Debug.Assert(_inputManager != null, $"[{name}] PlayerInputManager 누락");

            _settingsRoot = _settingsUIDocument.rootVisualElement;
        }

        private void Start()
        {
            //CloseSettings();
        }

        private void OnEnable()
        {
            _inputManager.OnToggleSettingsEvent += HandleToggleSettings;
        }

        private void OnDisable()
        {
            _inputManager.OnToggleSettingsEvent -= HandleToggleSettings;
        }

        #endregion

        #region Input Event Handlers

        private void HandleToggleSettings()
        {
            if (_isSettingsOpen)
            {
                CloseSettings();
            }
            else
            {
                OpenSettings();
            }
        }

        #endregion

        #region Private Methods

        private void OpenSettings()
        {
            _isSettingsOpen = true;
            _settingsRoot.style.display = DisplayStyle.Flex;

            // UI가 열리면 플레이어 액션 맵 비활성화 (이동, 공격, 점프 등 모두 차단)
            _inputManager.DisablePlayerInput();
        }

        private void CloseSettings()
        {
            _isSettingsOpen = false;
            _settingsRoot.style.display = DisplayStyle.None;

            // UI가 닫히면 플레이어 액션 맵 다시 활성화
            _inputManager.EnablePlayerInput();
        }

        #endregion
    }
}