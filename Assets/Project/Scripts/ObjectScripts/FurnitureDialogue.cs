using UnityEngine;

public class FurnitureDialogue : MonoBehaviour, IInteractable
{
    private bool _hasPlayerContactThisFrame = false;
    private bool _canInteract = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && _hasPlayerContactThisFrame && _canInteract)
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
        _canInteract = false;
        // TODO: Implement UI dialogue system to display dialogue when interacting with objects
        Debug.Log("Interacted with furniture: " + gameObject.name);
        _canInteract = true;
    }
}
