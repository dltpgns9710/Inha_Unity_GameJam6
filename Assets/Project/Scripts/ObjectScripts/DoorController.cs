// using JUNBEOM.Player;
using SEHOON.GameSystem;
using SEHOON.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isForward = false;
    [SerializeField] private bool _isBackward = false;

    [SerializeField] private bool _randomizeDoor = false;
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private List<GameObject> _otherDoors;

    [SerializeField] private float _shakeDuration = 0.25f;
    [SerializeField] private float _shakeIntensity = 0.05f;
    [SerializeField] private float _openDuration = 2.0f;

    private Animator _animator;
    private bool _isOpening;
    private bool _hasBeenUnlocked = true;
    private Vector3 _originalPosition;
    private bool _hasKey = false;
    // private PlayerEventManager _eventManager;   //EventManager 싱글톤 방식으로 변경

    void Awake()
    {
        // _eventManager = PlayerEventManager.Instance;
    }
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
    private void OnEnable()
    {
        // _eventManager.OnInteractionRequested += OnInteraction;   //EventManager 싱글톤 방식으로 변경
    }

    private void OnDisable()
    {
        // _eventManager.OnInteractionRequested -= OnInteraction;   //EventManager 싱글톤 방식으로 변경
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

    private void OnInteraction(GameObject target, GameObject player)
    {
        if (target != gameObject)
            return;
        // _eventManager.RequestKeyState(CheckKey);    //EventManager 싱글톤 방식으로 변경
        Interact(player);
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
            yield return StartCoroutine(ShakeDoor());
            yield break;
        }

        _isOpening = true;
        PlayerInputManager inputManager = interactor.GetComponentInParent<PlayerInputManager>();  //플레이어 인풋 매니져 연결
        // inputManager.DisablePlayerInput();  //플레이어 입력 불가능

        try
        {
            bool hasAnomaly = DataManager.Instance.IsAnomalyApply();
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
            }
            

            int index = 0;

            _animator.SetBool("isOpen", true);
            yield return new WaitForSeconds(_openDuration);  //_openDuration 기간 동안 정지
            if (_randomizeDoor && _otherDoors.Count >= 1)
            {
                index = UnityEngine.Random.Range(0, _otherDoors.Count);
            }
            interactor.transform.position = _otherDoors[index].transform.position;
            _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", false);
        }
        finally
        {
            // inputManager.EnablePlayerInput();   //플레이어 입력 가능
            _isOpening = false;
        }
    }

    private IEnumerator ShakeDoor()
    {
        float elapsedTime = 0f;

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