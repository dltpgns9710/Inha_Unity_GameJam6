using JUNBEOM.Player;
using SEHOON.GameSystem;
using SEHOON.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using PlayerInputManager = JUNBEOM.Player.PlayerInputManager;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isForward = false;
    [SerializeField] private bool _isBackward = false;
    [SerializeField] private bool _randomizeDoor = false;
    [SerializeField] private bool _isLocked = false;

    [SerializeField] private float _shakeDuration = 0.25f;
    [SerializeField] private float _shakeIntensity = 0.05f;
    [SerializeField] private float _openDuration = 2.0f;

    [SerializeField] private AudioClip _doorOpenSound;
    [SerializeField] private AudioClip _doorCloseSound;
    [SerializeField] private AudioClip _doorLockedSound;

    [SerializeField] private GameObject _fadeText;

    [SerializeField] private List<GameObject> _otherDoors;

    private Animator _animator;
    private bool _isOpening;
    private bool _hasBeenUnlocked = true;
    private Vector3 _originalPosition;
    private bool _hasKey = false;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _originalPosition = transform.position;

        if (_isLocked == true)
        {
            _hasBeenUnlocked = false;
        }

        _animator.SetBool("isOpen", false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private bool Unlock(GameObject interactor)
    {
        if (_hasBeenUnlocked)
        {
            return true;
        }

        if (_hasKey && _isLocked)
        {
            _isLocked = false;
            _hasBeenUnlocked = true;
            return true;
        }
        return false;
    }
    private void CheckKey(bool hasKey)
    {
        _hasKey = hasKey;
        if (_hasKey)
        {
            Debug.Log("열쇠 소유");
        }
    }

    public void Interact(GameObject interactor)
    {
        PlayerEventManager.Instance.OnKeyStateRequested?.Invoke(CheckKey);
        if (_isOpening)
        {
            return;
        }

        StartCoroutine(InteractCoroutine(interactor));
    }


    //문 작동시 플레이어 인풋 정지
    private IEnumerator InteractCoroutine(GameObject interactor)
    {

        bool canOpenDoor = _otherDoors.Count > 0 && Unlock(interactor);

        if (!canOpenDoor)
        {
            // TODO: Implement door open failure sound
            yield return StartCoroutine(CoroutineShakeDoor());
            yield break;
        }

        _isOpening = true;
        PlayerInputManager inputManager = interactor.GetComponentInParent<PlayerInputManager>();  //플레이어 인풋 매니져 연결
        inputManager.DisablePlayerInput();  //플레이어 입력 불가능

        bool hasAnomaly = DataManager.Instance.IsAnomalyApply;
        if (_isBackward || _isForward)
        {
            
            if (_isForward && hasAnomaly)
            {
                DataManager.Instance.SelectIncorrectDoor();
            }
            else if (_isBackward && !hasAnomaly)
            {
                DataManager.Instance.SelectIncorrectDoor();
            }
            else
            {
                DataManager.Instance.SelectCorrectDoor();
            }

            _animator.SetBool("isOpen", true);
            yield return new WaitForSeconds(_openDuration);
            SoundManager.Instance.PlaySfx(_doorOpenSound);
            if (DataManager.Instance.Floor == DataManager.Instance.GoalFloor + 1)
            {
                DataManager.Instance.StoryType = EStoryType.Ending;
                SceneManager.LoadScene("StoryScene");
                yield break;
            }
            FadeTextView fadeTextView = _fadeText.GetComponent<FadeTextView>();
            _fadeText.SetActive(true);

            fadeTextView.OnDisableEvent.AddListener(() =>
            {
                SceneManager.LoadScene("MainScene");
            });

            yield break;
        }
            
        int index = 0;

        if (_randomizeDoor && _otherDoors.Count >= 1)
        {
            index = UnityEngine.Random.Range(0, _otherDoors.Count);
        }
        _animator.SetBool("isOpen", true);
        _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", true);
        SoundManager.Instance.PlaySfx(_doorOpenSound);

        yield return new WaitForSeconds(_openDuration);

        interactor.transform.position = _otherDoors[index].transform.position;
        _animator.SetBool("isOpen", false);
        _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", false);
        SoundManager.Instance.PlaySfx(_doorCloseSound);

        inputManager.EnablePlayerInput();   //플레이어 입력 가능
        _isOpening = false;
    }

    private IEnumerator CoroutineShakeDoor()
    {
        float elapsedTime = 0f;
        SoundManager.Instance.PlaySfx(_doorLockedSound);
        while (elapsedTime < _shakeDuration)
        {
            elapsedTime += Time.deltaTime;

            Vector3 randomOffset = UnityEngine.Random.insideUnitCircle * _shakeIntensity;
            transform.position = _originalPosition + randomOffset;

            yield return null;
        }
        transform.position = _originalPosition;
    }
}