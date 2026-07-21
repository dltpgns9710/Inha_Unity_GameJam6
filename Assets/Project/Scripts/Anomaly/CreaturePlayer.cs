using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreaturePlayer : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            DataManager.Instance.SelectIncorrectDoor();
            SceneManager.LoadScene("MainScene");
        }
    }
}
