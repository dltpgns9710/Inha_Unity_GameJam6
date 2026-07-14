using JUNBEOM.Player;
using SEHOON.UI;
using UnityEngine;

public class FurnitureDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _textBox;

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
        _textBox.SetActive(true);
        /*_textBox.GetComponent<DialogueBoxView>().OnDisableEvent.AddListener(() =>
        {
            _textBox.SetActive(false);
        });*/
    }
}
