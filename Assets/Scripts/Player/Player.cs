using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private string playerId;
    [SerializeField] private int score;
    [SerializeField] private int coins;
    [SerializeField] private int lives;
    [SerializeField] private int playerLevel = 1;
    [SerializeField] private PlayerStatus playerStatus;

    [Header("OnValidated References")]
    [SerializeField] private Transform voidTransform;
    [SerializeField] private PlayerStatusDisplay playerStatusDisplay;
    [SerializeField] private Scoreable scoreable;
    [SerializeField] private PullEffect pullEffect;
    [SerializeField] private KillboxPlayer killboxPlayer;
    [SerializeField] private KillboxObject killboxObject;

    public int PlayerLevel => playerLevel;
    public string PlayerId => playerId;
    public int Score => score;
    public int Lives => lives;
    public Transform VoidTransform => voidTransform;
    public int Coins => coins;

    private Vector3 originalPlayerSize;
    private float activeRespawnTime = 0.0f;

    private void OnValidate()
    {
        scoreable = GetComponent<Scoreable>();
        playerStatusDisplay = GetComponentInChildren<PlayerStatusDisplay>();
        pullEffect = GetComponentInChildren<PullEffect>();
        killboxObject = GetComponentInChildren<KillboxObject>();
        killboxPlayer = GetComponentInChildren<KillboxPlayer>();

        if (scoreable == null)
        {
            Debug.LogError("Scoreable-component not found.");
        }

        if (playerStatusDisplay == null)
        {
            Debug.LogError("PlayerStatusDisplay-component not found in children.");
        }

        if (pullEffect == null)
        {
            Debug.LogError("PullEffect-component not found in voidTransform.");
        }
        else
        {
            voidTransform = pullEffect.transform;
        }

        if (killboxPlayer == null)
        {
            Debug.LogError("KillboxOPlayer-component not found in children.");
        }

        if (killboxObject == null)
        {
            Debug.LogError("KillboxObject-component not found in children.");
        }
    }

    private void Awake()
    {
        Initialize(Guid.NewGuid().ToString(), LevelManager.Instance.StartingLives);
        originalPlayerSize = voidTransform.localScale;
    }

    private void Update()
    {
        if (playerStatus == PlayerStatus.Dead)
        {
            activeRespawnTime -= Time.deltaTime;

            if (activeRespawnTime <= 0.0f)
            {
                LevelManager.Instance.RespawnPlayer(this);
            }
        }
    }

    public void Initialize(string id, int initialLives)
    {
        playerId = id;
        lives = initialLives;
        score = 0;

        pullEffect.SetPlayerId(playerId);
        killboxPlayer.SetPlayerId(playerId);
        killboxObject.SetPlayerId(playerId);

        playerStatusDisplay.UpdateLevelProgress(score, LevelManager.Instance.CalculateScoreForLevel(playerLevel + 1));
        playerStatusDisplay.UpdateLevelDisplay(playerLevel);
        
        OnSpawn();
    }

    public void AddScore(int value)
    {
        score += value;
        coins += value;

        if (playerLevel < LevelManager.Instance.MaxLevel && score >= LevelManager.Instance.CalculateScoreForLevel(playerLevel + 1))
        {
            LevelUp();
        }
        else
        {
            playerStatusDisplay.UpdateLevelProgress(score, LevelManager.Instance.CalculateScoreForLevel(playerLevel + 1));
        }
    }

    public void PlayerKilled()
    {
        lives--;
        voidTransform.gameObject.SetActive(false);
        playerStatusDisplay.gameObject.SetActive(false);
        
        var mainPlayer = LevelManager.Instance.MainPlayer;

        if (this == mainPlayer)
        {
            HapticsManager.Instance.Vibrate(1500, 255);
        }

        if (lives <= 0)
        {
            if (this == mainPlayer)
            {
                string[] gameOverData = new string[2];
                gameOverData[0] = "Match Over!";
                gameOverData[1] = string.Concat(mainPlayer.Coins, " coins earned!");

                EventManager.TriggerEvent(Events.GameOver, gameOverData);
            }
            else
            {
                EventManager.TriggerEvent(Events.PlayerKilled, playerId);
            }

            return;
        }

        activeRespawnTime = LevelManager.Instance.RespawnTime;
        playerStatus = PlayerStatus.Dead;

        EventManager.TriggerEvent(Events.PlayerKilled, playerId);
    }

    public void OnSpawn()
    {
        playerStatus = PlayerStatus.Alive;
        playerStatusDisplay.gameObject.SetActive(true);
        voidTransform.gameObject.SetActive(true);

        var colliders = Physics.OverlapSphere(transform.position, 0.4f);

        foreach (Collider collider in colliders)
        {
            TileColliderController tileCollider = collider.GetComponent<TileColliderController>();
            if (tileCollider != null)
            {
                tileCollider.ToggleCollider(false, PlayerId);
            }
        }

        scoreable.ResetScore();
    }

    private void LevelUp()
    {
        playerLevel++;
        score -= LevelManager.Instance.CalculateScoreForLevel(playerLevel);

        var playerSize = originalPlayerSize * LevelManager.Instance.PlayerSizeByLevel(playerLevel);
        playerSize.y = originalPlayerSize.y;
        voidTransform.localScale = playerSize;
        scoreable.SetPlayerScoreValue(playerLevel);

        pullEffect.SetEffectValues(LevelManager.Instance.VoidPullValue(playerLevel), LevelManager.Instance.VoidRadiusValue(playerLevel));

        OnSpawn();

        playerStatusDisplay.UpdateLevelDisplay(playerLevel);
        playerStatusDisplay.UpdateLevelProgress(score, LevelManager.Instance.CalculateScoreForLevel(playerLevel + 1));

        var mainPlayer = LevelManager.Instance.MainPlayer;

        if (this == mainPlayer)
        {
            HapticsManager.Instance.Vibrate(1000, 255);
        }
    }

    public void AddCoins(int value)
    {
        coins += value;
    }
}

public enum PlayerStatus
{
    Alive,
    Dead,
}