using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isLoop = false;

    [SerializeField] private bool _startOpened = false;
    [SerializeField] private bool _randomizeDoor = false;
    [SerializeField] private bool _isLocked = false;
    [SerializeField] private List<GameObject> _otherDoors;

    [SerializeField] private float _shakeDuration = 0.25f;
    [SerializeField] private float _shakeIntensity = 0.05f;

    private Animator _animator;
    private bool _open = false;
    private bool _hasBeenUnlocked = true;
    private Vector3 _originalPosition;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _originalPosition = transform.position;

        if (_startOpened)
        {
            _open = true;
        }
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
            int index = 0;

            _animator.SetBool("isOpen", true);
            yield return new WaitForSeconds(2);
            if (_randomizeDoor && _otherDoors.Count >= 1)
            {
                index = UnityEngine.Random.Range(0, _otherDoors.Count);
            }
            interactor.transform.position = _otherDoors[index].transform.position;
            _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", false);
        }
        else
        {
            // TODO: Implement door open failure sound
            yield return StartCoroutine(ShakeDoor());
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
