using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] screenElements;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI maxComboText;
    [SerializeField] private TextMeshProUGUI rankText;

    public void ShowResultScreen(int finalScore, float timeElapsed, int maxStylishPoints)
    {
        //show the screen elements
        foreach (var element in screenElements)
        {
            element.SetActive(true);
        }
        
        scoreText.text = $"score {GetScoreText(finalScore)}";
        timeText.text = $"time {GetTimeText(timeElapsed)}";
        maxComboText.text = $"max combo {GetMaxComboText(maxStylishPoints)}";
        rankText.text = $"RANK {GetRankText(finalScore)}";
    }
    
    public void HideResultScreen()
    {
        //hide the screen elements
        foreach (var element in screenElements)
        {
            element.SetActive(false);
        }
    }

    private string GetScoreText(int score)
    {
        return $"<b><size=80>{score:000000}</size></b>";
    }
    
    private string GetTimeText(float timeElapsed)
    {
        return $"<b><size=80>{Mathf.FloorToInt(timeElapsed / 60):D2}:{Mathf.FloorToInt(timeElapsed % 60):D2}.{Mathf.FloorToInt(timeElapsed * 100 % 100):D2}</size></b>";
    }
    
    private string GetMaxComboText(int maxStylishPoints)
    {
        return $"<b><size=80>{maxStylishPoints}</size></b>";
    }
    
    //get the final rank based on player's final score
    private string GetRankText(int score)
    {
        //the rank letter is formatted to have a font size of 200 and is bold
        return score switch
        {
            > 30000 => "<size=200><b>S</b></size>",
            >= 25000 => "<size=200><b>A</b></size>",
            >= 20000 => "<size=200><b>B</b></size>",
            >= 15000 => "<size=200><b>C</b></size>",
            _ => "<size=200><b>D</b></size>"
        };
    }
}
