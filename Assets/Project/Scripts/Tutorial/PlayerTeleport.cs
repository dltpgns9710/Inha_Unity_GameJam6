using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{

    [SerializeField] private GameObject interactor;

    private bool _triggered = false;
    private bool _isPlayerInRange = false;

    void Start()
    {

    }

    void Update()
    {

    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        _isPlayerInRange = true;
        interactor.transform.position = new Vector3(-8.5f, -16.7f, 0);


    }
}
