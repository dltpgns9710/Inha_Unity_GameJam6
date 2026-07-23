using System.Collections; // 코루틴(IEnumerator)을 사용하기 위해 반드시 추가해야 합니다.
using SEHOON.GameSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreaturePlayer : MonoBehaviour
{
    private bool isTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true; 

            StartCoroutine(DelayedAction(2f));
        }
    }

    // 대기 시간을 처리할 코루틴 함수
    private IEnumerator DelayedAction(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        DataManager.Instance.SelectIncorrectDoor();
        SceneManager.LoadScene("MainScene");
    }
}