using TMPro;
using UnityEngine;

public class TimerUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

    public void UpdateTimerUI(float timeElapsed)
    {
        timerText.text = $"{Mathf.FloorToInt(timeElapsed / 60):D2}:{Mathf.FloorToInt(timeElapsed % 60):D2}.{Mathf.FloorToInt(timeElapsed * 100 % 100):D2}";
    }
}
