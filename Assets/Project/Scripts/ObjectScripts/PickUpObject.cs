using Unity.VisualScripting;
using UnityEngine;
using JUNBEOM.Player;

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
        PlayerEventManager.Instance.OnKeyCollected?.Invoke();
        Debug.Log("Picked up item: " + gameObject.name);
    }
}
