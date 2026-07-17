using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyTwinkle : AnomalyBase
{
    [Header("깜빡일 검은색 화면 UI")]
    public GameObject blinkUI;

    public float blinkSpeed = 0.15f;

    public override void Apply()
    {
        InvokeRepeating("ToggleBlink", 0f, blinkSpeed);
    }

    public override void Remove()
    {
        CancelInvoke("ToggleBlink");

        if (blinkUI != null)
        {
            blinkUI.SetActive(false);
        }
    }

    private void ToggleBlink()
    {
        if (blinkUI != null)
        {
            blinkUI.SetActive(!blinkUI.activeSelf);
        }
    }
}