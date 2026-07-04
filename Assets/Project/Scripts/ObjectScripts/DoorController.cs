using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _startOpened = false;
    [SerializeField] private bool _randomizeDoor = false;
    [SerializeField] private List<GameObject> _otherDoors;


    private Animator _animator;
    private bool _open = false;
    private bool _hasPlayerContactThisFrame = false;
    private bool _canInteract = true;

    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_startOpened)
        {
            _open = true;
        }
        _animator.SetBool("isOpen", _open);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _hasPlayerContactThisFrame && _canInteract)
        {
            Interact(GameObject.FindGameObjectWithTag("Player"));
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_hasPlayerContactThisFrame)
        {
            return;
        }

        _hasPlayerContactThisFrame = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _hasPlayerContactThisFrame = false;
    }

    public void Interact(GameObject interactor)
    {
        StartCoroutine(InteractCoroutine(interactor));
    }

    private IEnumerator InteractCoroutine(GameObject interactor)
    {

        if (_otherDoors.Count > 0)
        {
            int index = 0;

            _canInteract = false;
            _animator.SetBool("isOpen", true);
            yield return new WaitForSeconds(2);
            if (_randomizeDoor && _otherDoors.Count >= 1)
            {
                index = UnityEngine.Random.Range(0, _otherDoors.Count);
            }
            interactor.transform.position = _otherDoors[index].transform.position;
            _otherDoors[index].GetComponent<Animator>().SetBool("isOpen", false);
            _canInteract = true;
        }
        else
        {

        }
    }
}
