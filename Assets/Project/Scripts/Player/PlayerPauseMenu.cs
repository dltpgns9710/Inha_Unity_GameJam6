using UnityEngine;
using SEHOON.UI;          
using JUNBEOM.Player;    

public class PlayerPauseMenu : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private PauseMenuView _pauseMenuView;

    [Header("Input Reference")]
    [SerializeField] private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        if (_playerInputManager == null)
        {
            _playerInputManager = GetComponent<PlayerInputManager>();
        }
    }

    private void Start()
    {
        if (_pauseMenuView != null)
        {
            _pauseMenuView.OnEnableEvent.AddListener(HandlePauseMenuEnabled);
            _pauseMenuView.OnDisableEvent.AddListener(HandlePauseMenuDisabled);
            _pauseMenuView.gameObject.SetActive(false);
        }

        if (_playerInputManager != null)
        {
            _playerInputManager.OnPauseMenuEvent += TogglePauseMenu;
        }
    }

    private void OnDestroy()
    {
        if (_playerInputManager != null)
        {
            _playerInputManager.OnPauseMenuEvent -= TogglePauseMenu;
        }
    }

    private void TogglePauseMenu()
    {
        if (_pauseMenuView != null)
        {
            bool isMenuActive = _pauseMenuView.gameObject.activeSelf;
            _pauseMenuView.gameObject.SetActive(!isMenuActive);
        }
    }

    private void HandlePauseMenuEnabled()
    {
        Debug.Log("일시정지 메뉴 켜짐!");
    }

    private void HandlePauseMenuDisabled()
    {
        Debug.Log("일시정지 메뉴 꺼짐!");
    }
}