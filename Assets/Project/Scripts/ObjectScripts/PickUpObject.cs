using Unity.VisualScripting;
using UnityEngine;

public class PickUpObject : MonoBehaviour, IInteractable
{
    private bool _hasPlayerContactThisFrame = false;
    // TODO : SetActive when going through an end door

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _hasPlayerContactThisFrame)
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
        // TODO: Add item to inventory system when interacting with the object
        Debug.Log("Picked up item: " + gameObject.name);
        this.GameObject().SetActive(false);
    }
}
