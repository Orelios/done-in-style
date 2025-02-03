using UnityEngine;

[CreateAssetMenu(fileName = "New Stylish Rank", menuName = "Scriptable Objects/Stylish Rank")]
public class StylishRankSO : ScriptableObject
{
    [SerializeField] private ERankType rankType;
    [SerializeField] private string rankName;
    [SerializeField] private float scoreMultiplier;
    [SerializeField] private int requiredBreakthroughPoints;
    
    public ERankType RankType => rankType;
    public string RankName => rankName;
    public float ScoreMultiplier => scoreMultiplier;
    public int RequiredBreakthroughPoints => requiredBreakthroughPoints;
}

public enum ERankType
{
    NoRank,
    Loser,
    Cooking,
    Ballin,
    Awesome,
    Stylish
}
