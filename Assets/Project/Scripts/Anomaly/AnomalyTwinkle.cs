using UnityEngine;
using SEHOON.GameSystem;

public class AnomalyTwinkle : AnomalyBase
{
    [Header("깜빡일 검은색 화면 UI")]
    public GameObject blinkUI;
    public GameObject ghost;
    public ObjectCollider objectCollider;

    [Header("놀라는 사운드")]
    [SerializeField] private AudioClip _scream;

    public float blinkSpeedA = 0.15f;
    public float blinkSpeedB = 0.15f;

    private bool _detect = false;
    private bool _isApplied = false;

    public override void Apply()
    {
        _isApplied = true;
        
    }
    public override void Remove()
    {
        _isApplied = false;

        CancelInvoke("ToggleBlinkA");
        CancelInvoke("ToggleBlinkB");
        if (blinkUI != null) blinkUI.SetActive(false);
        if (ghost != null) ghost.SetActive(false);

        _detect = false; 
    }
    private void Update()
    {
        if (!_isApplied) return;
        bool currentDetect = objectCollider.Detect();
        int count = 0;
        if (currentDetect != _detect)
        {
            if (currentDetect)
            {
                InvokeRepeating("ToggleBlinkA", 0f, blinkSpeedA);
                InvokeRepeating("ToggleBlinkB", 0f, blinkSpeedB);
                if(count == 0)
                    SoundManager.Instance.PlaySfx(_scream);
                ++count;
            }
            else
            {
                CancelInvoke("ToggleBlinkA");
                CancelInvoke("ToggleBlinkB");
                if (blinkUI != null) blinkUI.SetActive(false);
                if (ghost != null) ghost.SetActive(false);
            }
            _detect = currentDetect;
        }
    }

    private void ToggleBlinkA()
    {
        if (blinkUI != null)
        {
            blinkUI.SetActive(!blinkUI.activeSelf);
        }
    }
    private void ToggleBlinkB()
    {
        if (ghost != null)
        {
            ghost.SetActive(!ghost.activeSelf);
        }
    }
}