using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    [Header("Level Settings")]
    [SerializeField] private float levelDuration = 60f;
    [SerializeField] private float intermissionTime = 15f;
    [SerializeField] private int dummyPlayerCount = 3;
    [SerializeField] private float respawnTime = 2.0f;
    [SerializeField] private float objectSpawnInterval = 5.0f;
    [SerializeField] private int[] intermissionCoinRewards = new int[5];

    [Header("Player Settings")]
    [SerializeField] private List<float> playerSizeByLevel = new ();
    [SerializeField] private float voidPullBase;
    [SerializeField] private float voidRadiusBase;
    [SerializeField] private int startingLives = 3;

    [Header("References")]
    [SerializeField] private Player playerPrefab;
    [SerializeField] private Player dummyPlayerPrefab;
    [SerializeField] private Transform playerParent;
    [SerializeField] private MapGenerator[] spawnableAreas;
    [SerializeField] private GameObject[] spawnableObjects;

    private List<Player> players = new ();
    private Player mainPlayer;

    private float activeObjectSpawnInterval;
    private float activeGameTimeInterval;
    private float activeIntermissionInterval;
    private int spawnedObjectCount;
    private int spawnedObjectCap;

    public int MaxLevel => playerSizeByLevel.Count;
    public int StartingLives => startingLives;
    public float RespawnTime => respawnTime;
    public Player MainPlayer => mainPlayer;

    public override void Awake()
    {
        base.Awake();
        EventManager.Instance.Initialize();
        EventManager.StartListening(Events.ObjectDestroyed, ObjectDestroyed);
    }

    private void Start()
    {
        mainPlayer = SpawnPlayer(playerPrefab);

        SpawnDummyPlayers(dummyPlayerCount);

        foreach (var spawnableArea in spawnableAreas)
        {
            spawnedObjectCount += spawnableArea.ObjectParent.childCount;
        }

        spawnedObjectCap = spawnedObjectCount;
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.ObjectDestroyed, ObjectDestroyed);
    }

    private void Update()
    {
        activeGameTimeInterval += Time.deltaTime;
        activeIntermissionInterval += Time.deltaTime;
        activeObjectSpawnInterval += Time.deltaTime;

        if (activeGameTimeInterval >= levelDuration)
        {
            PlayerPrefs.SetInt("PlayerCoinCount", PlayerPrefs.GetInt("PlayerCoinCount", 0) + LevelManager.Instance.MainPlayer.Coins);

            string[] gameOverData = new string[2];
            gameOverData[0] = "Match Over!";
            gameOverData[1] = string.Concat(mainPlayer.Coins, " coins earned!");
            
            EventManager.TriggerEvent(Events.GameOver, gameOverData);
            activeGameTimeInterval = 0.0f;
        }
        if (activeIntermissionInterval >= intermissionTime)
        {
            EventManager.TriggerEvent(Events.IntermissionActivated, null);
            activeIntermissionInterval = 0.0f;
        }
        if (activeObjectSpawnInterval >= objectSpawnInterval)
        {
            SpawnObject();
            activeObjectSpawnInterval = 0.0f;
        }
    }

    public int CalculateScoreForLevel(int level)
    {
        var baseAmount = 3;
        var requiredExp = baseAmount + ((level - 1) * level / 2);
        return requiredExp;
    }

    public float PlayerSizeByLevel(int level)
    {
        if (level < 0) 
            return 1f;
        if (level >= playerSizeByLevel.Count)
            return playerSizeByLevel[playerSizeByLevel.Count - 1];

        return playerSizeByLevel[level];
    }

    public float VoidPullValue(int level)
    {
        return voidPullBase * level;
    }
    
    public float VoidRadiusValue(int level)
    {
        return voidRadiusBase * level;
    }

    private void SpawnDummyPlayers(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnPlayer(dummyPlayerPrefab);
        }
    }
    
    private Vector3 GetRandomSpawnPosition(GameObject spawnedObject, Transform spawnArea)
    {
        Bounds bounds = spawnArea.GetComponent<Renderer>().bounds;
        Vector3 objectSize = spawnedObject.GetComponent<Renderer>().bounds.size;

        float x = Random.Range(bounds.min.x + objectSize.x / 2, bounds.max.x - objectSize.x / 2);
        float z = Random.Range(bounds.min.z + objectSize.z / 2, bounds.max.z - objectSize.z / 2);
        return new Vector3(x, 0, z);
    }

    public Vector3 GetTargetPositionForDummyPlayer()
    {
        var spawnArea = spawnableAreas[Random.Range(0, spawnableAreas.Length)].Ground;
        var bounds = spawnArea.GetComponent<Renderer>().bounds;
        var x = Random.Range(bounds.min.x, bounds.max.x);
        var z = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(x, 0, z);
    }

    public void AddScoreForPlayer(string playerId, int scoreValue)
    {
        if (playerId == mainPlayer.PlayerId)
        {
            mainPlayer.AddScore(scoreValue);
            HapticsManager.Instance.Vibrate(100, 100);
            //Handheld.Vibrate();
            return;
        }

        players.Find(player => player.PlayerId == playerId)?.AddScore(scoreValue);
    }

    public Player SpawnPlayer(Player playerPrefab)
    {
        var player = Instantiate(playerPrefab, Vector3.one, Quaternion.identity, playerParent);
        
        var spawnArea = spawnableAreas[Random.Range(0, spawnableAreas.Length)].Ground;
        var spawnPosition = GetRandomSpawnPosition(player.VoidTransform.gameObject, spawnArea);

        player.transform.position = spawnPosition;
        players.Add(player);

        return player;
    }

    private void SpawnObject()
    {
        if (spawnedObjectCount >= spawnedObjectCap)
            return;

        var spawnArea = spawnableAreas[Random.Range(0, spawnableAreas.Length)];
        var spawnableObject = spawnableObjects[Random.Range(0, spawnableObjects.Length)];
        var spawnPosition = GetRandomSpawnPosition(spawnableObject, spawnArea.Ground);

        spawnPosition.y = spawnableObject.GetComponent<Renderer>().bounds.size.y / 2;
        Instantiate(spawnableObject, spawnPosition, Quaternion.identity, spawnArea.ObjectParent);
        spawnedObjectCount++;
    }

    public void RespawnPlayer(Player player)
    {
        var spawnArea = spawnableAreas[Random.Range(0, spawnableAreas.Length)].Ground;
        var spawnPosition = GetRandomSpawnPosition(player.VoidTransform.gameObject, spawnArea);
        player.transform.position = spawnPosition;
        player.OnSpawn();

        if (player == mainPlayer)
            HapticsManager.Instance.Vibrate(200, 200);
    }

    private void ObjectDestroyed(object data)
    {
        spawnedObjectCount--;
    }

    public int DropIntermissionReward()
    {
        return intermissionCoinRewards[Random.Range(0, intermissionCoinRewards.Length)];
    }

    public void AddCoinsForMainPlayer(int coinValue)
    {
        mainPlayer.AddCoins(coinValue);
    }
}
