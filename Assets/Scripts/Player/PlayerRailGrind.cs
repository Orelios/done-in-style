using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using FMOD.Studio; 
public class PlayerRailGrind : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private RankCalculator rankCalculator;

    [Header("Grinding Configs")] 
    [SerializeField] private float grindingSpeedMultiplier;
    [SerializeField] private float grindingSpeedClamp;
    [SerializeField] private float minimumSpeedToGrind;
    public float MinimumSpeedToGrind => minimumSpeedToGrind;
    [SerializeField] private Vector2 momentumDecay = new Vector2(0.345f, 0.69f);
    
    [Header("Points Configs")]
    [SerializeField] private int pointsPerSecond;
    [SerializeField] private int maxTimeForPoints;
    
    [Space(15f)]
    public bool IsOnRail;
    private float _railDirection;
    private float _grindingSpeed;
    private Quaternion _rotationBeforeGrinding;
    private Player _player;
    private Railing _currentRailing;
    private Coroutine _scoreRoutine;
    private Coroutine _rankRoutine;

    //Audio
    private EventInstance _playerRailGrinding; 
    private void Awake()
    {
        /*_rb = GetComponent<Rigidbody2D>();
        _playerSprite = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();*/
        _player = GetComponent<Player>();
        _playerRailGrinding = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerRailGrinding);
    }

    private void FixedUpdate()
    {
        if (IsOnRail && _currentRailing is not null)
        {
            GrindRail();
        }
    }

    private void GrindRail()
    {
        _grindingSpeed = Mathf.Clamp(_grindingSpeed, float.MinValue, _player.Movement.MaxMovementSpeed * rankCalculator.CurrentStylishRank.MaxSpeedMultiplier * grindingSpeedClamp);
        _player.Rigidbody.linearVelocity = new Vector2(_grindingSpeed, _player.Rigidbody.linearVelocity.y);
    }

    private IEnumerator DecayMomentumRoutine()
    {
        var  lastVelocity = _player.Rigidbody.linearVelocity;

        _player.Rigidbody.linearVelocity = new(lastVelocity.x > 0 ? lastVelocity.x - momentumDecay.x : lastVelocity.x + momentumDecay.x, lastVelocity.y - momentumDecay.y);
        
        yield return null;
    }
    

    public void EnableRailGrinding(Railing railing)
    {
        IsOnRail = true;
        _currentRailing = railing;        
        _rotationBeforeGrinding = transform.rotation;
        _railDirection = transform.rotation.y == 0 ? 1 : -1;
        _grindingSpeed = Mathf.Abs(_player.Rigidbody.linearVelocityX) * _railDirection * grindingSpeedMultiplier;
        
        PLAYBACK_STATE playbackState;
        _playerRailGrinding.getPlaybackState(out playbackState);

        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            _playerRailGrinding.start();
        }

        _player.Sprite.gameObject.transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, Mathf.Approximately(_railDirection, 1) ? railing.transform.localEulerAngles.z : -railing.transform.localEulerAngles.z);
        
        if (railing.CanGeneratePoints)
        {
            _scoreRoutine = StartCoroutine(scoreCalculator.IncreaseScoreContinuousRoutine(pointsPerSecond, rankCalculator.CurrentStylishRank.ScoreMultiplier, maxTimeForPoints));
            _rankRoutine = StartCoroutine(rankCalculator.IncreaseStylishPointsContinuousRoutine(maxTimeForPoints));
        }
    }

    public void DisableRailGrinding()
    {
        IsOnRail = false;
        _playerRailGrinding.stop(STOP_MODE.ALLOWFADEOUT);
        _grindingSpeed = 0f;
        _railDirection = transform.rotation.y == 0 ? 1 : -1;
        _player.Sprite.gameObject.transform.rotation = _rotationBeforeGrinding;
        StopCoroutine(_scoreRoutine);
        StopCoroutine(_rankRoutine);
        StartCoroutine(DecayMomentumRoutine());
    }
}