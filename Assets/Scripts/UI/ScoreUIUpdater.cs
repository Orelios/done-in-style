using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreUIUpdater : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"{score:000000}";
    }
}
