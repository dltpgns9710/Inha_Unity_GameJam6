using SEHOON.GameSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isForward = false;
    [SerializeField] private bool _isBackward = false;

    [SerializeField] private bool _randomizeDoor = false;
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private List<GameObject> _otherDoors;

    [SerializeField] private float _shakeDuration = 0.25f;
    [SerializeField] private float _shakeIntensity = 0.05f;

    private Animator _animator;
    private bool _open = false;
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

        _animator.SetBool("isOpen", _open);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        // PlayerEventChannel.OnInteractionRequested += OnInteraction;
    }
    private void OnDisable()
    {
        // PlayerEventChannel.OnInteractionRequested -= OnInteraction;
    }

    private void OnInteraction(GameObject target, GameObject player)
    {
        if (target != gameObject)
            return;
        // PlayerEventChannel.RequestKeyState(CheckKey);
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

    private bool Unlock(GameObject interactor)
    {
        if (_hasBeenUnlocked)
        {
            return true;
        }

        if (/*interactor has key in inventory check 추가*/ _isLocked)
        {
            _isLocked = false;
            _hasBeenUnlocked = true;
            return true;
        }
        return false;
    }

    public void Interact(GameObject interactor)
    {
        StartCoroutine(InteractCoroutine(interactor));
    }

    private IEnumerator InteractCoroutine(GameObject interactor)
    {
        if (_otherDoors.Count > 0 && Unlock(interactor))
        {
            bool hasAnomaly = DataManager.Instance.IsAnomalyApply();
            if (_isForward || _isBackward)
            {
                if (this._isForward && hasAnomaly)
                {
                    DataManager.Instance.SelectIncorrectDoor();
                }
                else if (this._isBackward && !hasAnomaly)
                {
                    DataManager.Instance.SelectIncorrectDoor();
                }
                else
                {
                    DataManager.Instance.SelectCorrectDoor();
                }
            }

            int index = 0;
            if (_randomizeDoor && _otherDoors.Count >= 1)
            {
                index = UnityEngine.Random.Range(0, _otherDoors.Count);
            }

            _animator.SetBool("isOpen", true);
            _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", true);
            yield return new WaitForSeconds(2);
            interactor.transform.position = _otherDoors[index].transform.position;

            _animator.SetBool("isOpen", false);
            _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", false);
        }
        else
        {
            // TODO: Implement door open failure sound
            yield return StartCoroutine(ShakeDoor());
        }
        // PlayerEventChannel.OnInteractionRequested -= OnInteraction;
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
