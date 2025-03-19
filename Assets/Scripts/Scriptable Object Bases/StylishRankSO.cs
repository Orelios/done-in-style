using UnityEngine;

[CreateAssetMenu(fileName = "New Stylish Rank", menuName = "Scriptable Objects/Stylish Rank")]
public class StylishRankSO : ScriptableObject
{
    [Header("Rank")]
    [SerializeField] private ERankType rankType;
    [SerializeField] private string rankName;
    [SerializeField] private Sprite rankNameSprite;    
    
    [Header("Score")]
    [SerializeField] private float scoreMultiplier;
    [SerializeField] private Sprite scoreMultiplierBaseSprite;
    [SerializeField] private Sprite scoreMultiplierOutlineSprite;
    [SerializeField] private Color scoreMultiplierFillColor;
    
    [Header("Combo Points")]
    [SerializeField] private int requiredBreakthroughPoints;
    
    [Header("Gameplay Multipliers")]
    [SerializeField, Range(1f, 2f)] private float accelerationMultiplier;
    [SerializeField, Range(1f, 2f)] private float maxSpeedMultiplier;
    
    public ERankType RankType => rankType;
    public string RankName => rankName;
    public Sprite RankNameSprite => rankNameSprite;
    public float ScoreMultiplier => scoreMultiplier;
    public Sprite ScoreMultiplierBaseSprite => scoreMultiplierBaseSprite;
    public Sprite ScoreMultiplierOutlineSprite => scoreMultiplierOutlineSprite;
    public Color ScoreMultiplierFillColor => scoreMultiplierFillColor;
    public int RequiredBreakthroughPoints => requiredBreakthroughPoints;
    public float AccelerationMultiplier => accelerationMultiplier;
    public float MaxSpeedMultiplier => maxSpeedMultiplier;
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
