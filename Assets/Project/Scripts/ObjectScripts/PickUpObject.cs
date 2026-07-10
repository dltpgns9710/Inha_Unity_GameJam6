using Unity.VisualScripting;
using UnityEngine;

public class PickUpObject : MonoBehaviour, IInteractable
{
    // TODO : SetActive when going through an end door

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
        // TODO: Add item to inventory system when interacting with the object
        Debug.Log("Picked up item: " + gameObject.name);
        this.GameObject().SetActive(false);
    }
}
