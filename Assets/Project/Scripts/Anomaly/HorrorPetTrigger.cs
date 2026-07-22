using UnityEngine;
using SEHOON.GameSystem;
public class HorrorPetTrigger : MonoBehaviour
{
    [Header("호러 펫 재생 사운드")]
    public AudioClip horrorSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (horrorSound != null)
            {
                SoundManager.Instance.PlaySfx(horrorSound);
            }

            // GetComponent<BoxCollider2D>().enabled = false; 
        }
    }
}