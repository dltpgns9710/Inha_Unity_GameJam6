using UnityEngine;
using SEHOON.GameSystem;
public class HorrorPetTrigger : MonoBehaviour
{
    [Header("PetSound")]
    [SerializeField] private AudioClip _horrorSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_horrorSound != null)
            {
                SoundManager.Instance.PlaySfx(_horrorSound);
            }

            // GetComponent<BoxCollider2D>().enabled = false; 
        }
    }
}