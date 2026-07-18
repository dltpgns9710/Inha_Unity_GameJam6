using UnityEngine;

public class FakeSetActive : MonoBehaviour
{
    [SerializeField] private GameObject _entrance;
    [SerializeField] private GameObject _textBoxHitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnTriggerEnter2D(Collider2D collision)
    {
        gameObject.SetActive(false);
        _entrance.SetActive(true);
        _textBoxHitbox.SetActive(true);
    }
}
