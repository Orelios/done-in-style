using System;
using TMPro;
using UnityEngine;

public class LevelClearRankDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clearRankText;

    public void DetermineClearRank(int score)
    {
        clearRankText.text = score switch
        {
            < 15000 => "L",
            < 20000 => "C",
            < 25000 => "B",
            < 30000 => "A",
            _ => "S"
        };
    }
}
