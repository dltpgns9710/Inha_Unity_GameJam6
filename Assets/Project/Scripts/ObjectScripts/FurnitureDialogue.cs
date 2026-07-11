using JUNBEOM.Player;
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
        _textBox.SetActive(true);
    }
}
