using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    [SerializeField] private GameObject interactor;

    void Start()
    {

    }

    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        interactor.transform.position = new Vector3(-6f, -16.7f, 0);
    }
}
