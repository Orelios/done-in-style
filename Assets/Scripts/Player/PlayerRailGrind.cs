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
        /*_playerRailGrinding = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerRailGrinding);*/
    }

    private void Start()
    {
        _playerRailGrinding = AudioManager.instance.CreateInstance(FMODEvents.instance.PlayerRailGrinding);
    }

    private void FixedUpdate()
    {
        if (IsOnRail && _currentRailing is not null)
        {
            GrindRail();
        }
    }

    //clamps the grinding speed for when the player's grinding the rail (doesnt't work)
    //movement is done by setting the Rigidbody2D.LinearVelocity
    private void GrindRail()
    {
        _grindingSpeed = Mathf.Clamp(_grindingSpeed, float.MinValue, _player.Movement.MaxMovementSpeed * rankCalculator.CurrentStylishRank.MaxSpeedMultiplier * grindingSpeedClamp);
        _player.Rigidbody.linearVelocity = new Vector2(_grindingSpeed, _player.Rigidbody.linearVelocity.y);
    }

    //slows down the player when exiting rails
    private IEnumerator DecayMomentumRoutine()
    {
        var  lastVelocity = _player.Rigidbody.linearVelocity;

        _player.Rigidbody.linearVelocity = new(lastVelocity.x > 0 ? lastVelocity.x - momentumDecay.x : lastVelocity.x + momentumDecay.x, lastVelocity.y - momentumDecay.y);
        
        yield return null;
    }
    
  //assigns values to the necessary variables such as the player's grinding speed and rotation,
  //adjust the player sprite's rotation based on current rail's angle,
  //determines the direction of railing based player's direction when landing on rails
  // calculates grinding speed by getting the absolute value of the player's current Rigidbody2D.LinearVelocityX and multiplying it with the direction and a speed multiplier
  //and starts continuous score generation
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
            if (railing.graffiti != null)
            {
                railing.graffiti.StartGraffiti();
            }
        }
    }

    //resets value of necessary variables,
    //resets player's rotation, stops continuous score generation,
    //and starts to slowly decay the player's current speed
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