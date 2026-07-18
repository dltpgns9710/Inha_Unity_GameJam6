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

public class TutorialDoorController : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _fadeText;

    [SerializeField] private AudioClip _doorOpenSound;
    [SerializeField] private AudioClip _doorCloseSound;

    private Animator _animator;
    private bool _isOpening;
    private float _openDuration = 2.0f;

    void Start()
    {
        _animator = GetComponent<Animator>();

        _animator.SetBool("isOpen", false);
    }

    // Update is called once per frame
    void Update()
    {
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
        PlayerInputManager inputManager = interactor.GetComponentInParent<PlayerInputManager>();
        inputManager.DisablePlayerInput();  //플레이어 입력 불가능
            
        _animator.SetBool("isOpen", true);
        SoundManager.Instance.PlaySfx(_doorOpenSound);

        yield return new WaitForSeconds(_openDuration);
        
        SoundManager.Instance.PlaySfx(_doorCloseSound);

        SceneManager.LoadScene("MainScene");
        yield break;
    }
}