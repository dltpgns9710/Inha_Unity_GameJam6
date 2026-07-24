using UnityEngine;
using SEHOON.GameSystem;

public class ScoreObjectTogglerEvent : MonoBehaviour
{
    public int targetScore = 1;

    private void Start()
    {
        DataManager.Instance.OnScoreChanged += CheckScore;

        CheckScore(DataManager.Instance.Score);
    }

    private void OnDestroy()
    {
        if (DataManager.Instance != null)
        {
            DataManager.Instance.OnScoreChanged -= CheckScore;
        }
    }

    private void CheckScore(int currentScore)
    {
        if(currentScore !=targetScore)
        {
            gameObject.SetActive(false);
        }
    }
}