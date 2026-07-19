using JUNBEOM.Player;
using SEHOON.GameSystem;
using SEHOON.UI;
using UnityEngine;
using UnityEngine.Events;

public class FurnitureDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _textBox;

    [Header("Events")]
    [SerializeField] private UnityEvent _interactEndEvent;

    public UnityEvent InteractEndEvent => _interactEndEvent;

    private bool _isInteracted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject interactor)
    {
        if (_textBox == null)
        {
            Debug.LogWarning("TextBox is not assigned in the inspector.");
            return;
        }
        
        DialogueBoxView dialogueBoxView = _textBox.GetComponent<DialogueBoxView>();
        
        if (_isInteracted)
        {
            dialogueBoxView.DialogueLines.Clear();
            DialogueLine interactedDialogue = new DialogueLine();
            interactedDialogue.Name = "닐 브룩스";
            interactedDialogue.Dialogue = "아까와 달라진 점은 없어 보인다.";
            interactedDialogue.Alignment = EDialogueBoxAlignment.Left;
            dialogueBoxView.DialogueLines.Add(interactedDialogue);
        }
        
        PlayerInputManager inputManager = interactor.GetComponentInParent<PlayerInputManager>();
        _textBox.SetActive(true);
        DataManager.Instance.ActiveGameUI?.Invoke(false);
        
        inputManager.DisablePlayerInput();
        
        dialogueBoxView.OnDisableEvent.AddListener(() =>
        {
            inputManager.EnablePlayerInput();
            _isInteracted = true;
            DataManager.Instance.ActiveGameUI?.Invoke(true);
            _interactEndEvent?.Invoke();
        });
        
    }
}
