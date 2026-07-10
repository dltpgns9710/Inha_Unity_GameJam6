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
        // TODO: Implement UI dialogue system to display dialogue when interacting with objects
        _textBox.SetActive(true);
    }
}
