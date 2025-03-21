using FMOD.Studio;
using UnityEngine;
using UnityEngine.Rendering;

public class RailsParent : MonoBehaviour
{
    public bool hasTricked = false;
    public bool hasGivenScore = false;
    public Graffiti graffiti;
    private bool hasAppliedGraffiti;
    private PlayerTricks _playerTricks;

    private Coroutine _scoreRoutine;
    private Coroutine _rankRoutine;

    [Header("Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private RankCalculator rankCalculator;

    [Header("Points Configs")]
    [SerializeField] private int pointsPerSecond;
    [SerializeField] private int maxTimeForPoints;

    //Audio
    private EventInstance _playerRailGrinding;
    private void Start()
    {
        _playerRailGrinding = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerRailGrinding);
    }

    public void ApplyGraffiti()
    {
        if (!hasAppliedGraffiti)
        {
            if (graffiti != null)
            {
                graffiti.StartGraffiti();
            }
            hasAppliedGraffiti = true;
        }
    }

    public void CheckTrickMove()
    {
        if (!hasTricked)
        {
            _playerTricks.DisableCanTrick();
            _playerTricks.EnableTrick(gameObject);
            //hasTricked = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerRailing>(out var playerRailing))
        {
            playerRailing.ApplyOnenterSpeed(collision.gameObject.GetComponent<PlayerMovement>().Rb.linearVelocityX);
        }

        if (collision.gameObject.TryGetComponent<PlayerTricks>(out var playerTricks))
        {
            _playerTricks = playerTricks;
            _playerTricks.DisableCanTrick();

            PLAYBACK_STATE playbackState;
            _playerRailGrinding.getPlaybackState(out playbackState);

            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                _playerRailGrinding.start();
            }

            if (!hasGivenScore)
            {
                _scoreRoutine = StartCoroutine(scoreCalculator.IncreaseScoreContinuousRoutine(pointsPerSecond, rankCalculator.CurrentStylishRank.ScoreMultiplier, maxTimeForPoints));
                _rankRoutine = StartCoroutine(rankCalculator.IncreaseStylishPointsContinuousRoutine(maxTimeForPoints));
                _playerTricks.AddScoreAndRank();
                hasGivenScore = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //StopAllCoroutines();
        _playerRailGrinding.stop(STOP_MODE.ALLOWFADEOUT);
        StopCoroutine(_scoreRoutine);
        StopCoroutine(_rankRoutine);
        ApplyGraffiti();
        CheckTrickMove();
    }
}
