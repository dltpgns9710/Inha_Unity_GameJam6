using UnityEngine;

public class WaitForAkita : MonoBehaviour
{
    [SerializeField] private GameObject _blocker;

    private bool _canMove = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_canMove)
        {
            _blocker.SetActive(false);
            _canMove = true;
            return;
        }
        _blocker.SetActive(true);
        _canMove = false;
    }
}
