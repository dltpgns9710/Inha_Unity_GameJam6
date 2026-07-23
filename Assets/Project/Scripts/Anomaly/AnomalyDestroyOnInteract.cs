using System.Collections.Generic;
using UnityEngine;
using SEHOON.GameSystem;
using SEHOON.UI;

public class AnomalyDestroyOnInteract : AnomalyBase
{
    [Header("생성 위치")]
    [SerializeField] private Vector3 _spawnPosition;
    [SerializeField] private Vector3 _spawnRotationEuler;

    [Header("생성 프리팹 (FurnitureDialogue 포함)")]
    [SerializeField] private GameObject _furniturePrefab;

    [Header("커스텀 스프라이트")]
    [SerializeField] private Sprite _sprite;

    [Header("커스텀 대화 내용")]
    [SerializeField] private List<DialogueLine> _dialogueLines;

    private GameObject _spawnedInstance;
    private FurnitureDialogue _spawnedDialogue;

    public override void Apply()
    {
        if (_furniturePrefab == null)
        {
            return;
        }

        _spawnedInstance = Instantiate(_furniturePrefab, _spawnPosition, Quaternion.Euler(_spawnRotationEuler));
        _spawnedDialogue = _spawnedInstance.GetComponent<FurnitureDialogue>();

        if (_spawnedDialogue != null)
        {
            _spawnedDialogue.InteractEndEvent.AddListener(PostInteract);
        }

        ApplyCustomization();
    }

    public override void Remove()
    {
        if (_spawnedDialogue != null)
        {
            _spawnedDialogue.InteractEndEvent.RemoveListener(PostInteract);
        }

        DespawnInstance();
    }

    private void PostInteract()
    {
        DespawnInstance();
    }

    private void ApplyCustomization()
    {
        if (_spawnedInstance == null)
        {
            return;
        }

        if (_sprite != null)
        {
            SpriteRenderer spriteRenderer = _spawnedInstance.GetComponentInChildren<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = _sprite;
            }
        }

        if (_spawnedDialogue != null)
        {
            DialogueBoxView dialogueBoxView = _spawnedDialogue.TextBoxView;
            if (dialogueBoxView != null)
            {
                dialogueBoxView.DialogueLines = _dialogueLines;
            }
        }
    }

    private void DespawnInstance()
    {
        if (_spawnedInstance != null)
        {
            Destroy(_spawnedInstance);
            _spawnedInstance = null;
            _spawnedDialogue = null;
        }
    }
}
