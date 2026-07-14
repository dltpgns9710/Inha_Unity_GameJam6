using JUNBEOM.Player;
using UnityEngine;

public class FurnitureDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject _textBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }
    private void OnEnable()
    { 
        PlayerEventManager.Instance.OnInteractionRequested += OnInteraction;   //EventManager 싱글톤 방식으로 변경
    }
    private void OnInteraction(GameObject target, GameObject player)
    {
        if (target != gameObject)
            return;   //EventManager 싱글톤 방식으로 변경
        Interact(player);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(GameObject interactor)
    {
        // TODO: Implement UI dialogue system to display dialogue when interacting with objects

        if(_textBox != null) _textBox.SetActive(true);
        Debug.Log("1234");
    }
}
