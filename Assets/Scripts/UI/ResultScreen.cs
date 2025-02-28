using TMPro;
using UnityEngine;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] screenElements;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI maxComboText;
    [SerializeField] private TextMeshProUGUI rankText;

    public void ShowResultScreen(int finalScore, float timeElapsed, int highestStylishPoints)
    {
        //show the screen elements
        foreach (var element in screenElements)
        {
            element.SetActive(true);
        }
        
        //show player stats
        scoreText.text = $"{GetScoreText(finalScore)}";
        timeText.text = $"{GetTimeText(timeElapsed)}";
        maxComboText.text = $"{GetMaxComboText(highestStylishPoints)}";
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

    //get the score the player got when clearing the level
    private string GetScoreText(int score) => $"{score:000000}";
    
    //get the time the player took in clearing the level
    private string GetTimeText(float timeElapsed) => $"{Mathf.FloorToInt(timeElapsed / 60):D2}:{Mathf.FloorToInt(timeElapsed % 60):D2}.{Mathf.FloorToInt(timeElapsed * 100 % 100):D2}";
    
    //get the max combo based on the player's highest accumulated stylish points
    private string GetMaxComboText(int maxStylishPoints) => $"{maxStylishPoints}";
    
    //get the final rank based on player's final score
    private string GetRankText(int score)
    {
        //the text is formatted to have a font size of 200 and is bold
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
