using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private int mapWidth = 10;
    [SerializeField] private int mapHeight = 10;
    [SerializeField] private float tileSize = 1.0f;
    [SerializeField] List<ObjectSpawnSettings> objectSpawnSettings = new();
    [SerializeField] private Transform ground;
    [SerializeField] private Transform tileParent;
    [SerializeField] private Transform wallParent;
    [SerializeField] private Transform objectParent;

    public Transform Ground => ground;
    public Transform ObjectParent => objectParent;

    [ContextMenu("Generate Map")]
    public void GenerateMapInEditor()
    {
        if (tileParent != null)
        {
            DestroyImmediate(tileParent.gameObject);
        }

        var newMap = new GameObject("TileParent").transform;
        tileParent = newMap;
        tileParent.position = Vector3.zero;
        tileParent.SetParent(transform);

        GenerateMap();
    }

    public void GenerateObjectsInEditor()
    {
        if (objectParent != null)
        {
            DestroyImmediate(objectParent.gameObject);
        }

        objectParent = new GameObject("ObjectParent").transform;
        objectParent.position = Vector3.zero;
        objectParent.SetParent(transform);

        GenerateObjects();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            var columnParent = new GameObject($"Column {x + 1}").transform;
            columnParent.SetParent(tileParent);
            columnParent.localPosition = new Vector3(x * tileSize, 0, 0);
            columnParent.tag = "TileColumn";

            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 tilePosition = new Vector3(x * tileSize, 0, y * tileSize);
                var newTile = Instantiate(tilePrefab, tilePosition, Quaternion.identity, columnParent);
                newTile.name = $"Tile {x + 1},{y + 1}";
            }
        }

        tileParent.transform.localPosition = new Vector3((mapWidth - 1) * tileSize / -2, 0, (mapHeight - 1) * tileSize / -2);
        Ground.transform.localScale = new Vector3(mapWidth * tileSize / 10, 1, mapHeight * tileSize / 10);

        foreach (Transform child in wallParent)
        {
            child.gameObject.SetActive(true);
        }

        var wallColliders = wallParent.GetComponentsInChildren<BoxCollider>();
        var colliderThickness = 0.1f;

        wallColliders[0].transform.localPosition = new Vector3(0, 0, (mapHeight * tileSize) / 2 + colliderThickness);
        wallColliders[0].transform.localScale = new Vector3(mapWidth * tileSize, 1, colliderThickness);

        wallColliders[1].transform.localPosition = new Vector3(0, 0, (mapHeight * tileSize) / -2 - colliderThickness);
        wallColliders[1].transform.localScale = new Vector3(mapWidth * tileSize, 1, colliderThickness);

        wallColliders[2].transform.localPosition = new Vector3((mapWidth * tileSize) / 2 + colliderThickness, 0, 0);
        wallColliders[2].transform.localScale = new Vector3(colliderThickness, 1, mapHeight * tileSize);

        wallColliders[3].transform.localPosition = new Vector3((mapWidth * tileSize) / -2 - colliderThickness, 0, 0);
        wallColliders[3].transform.localScale = new Vector3(colliderThickness, 1, mapHeight * tileSize);
    }

    private void GenerateObjects()
    {
        foreach (var spawnSetting in objectSpawnSettings)
        {
            for (int i = 0; i < spawnSetting.amount; i++)
            {
                var randomPrefab = spawnSetting.prefabPool[UnityEngine.Random.Range(0, spawnSetting.prefabPool.Count)];
                SpawnObject(randomPrefab);
            }
        }
    }

    public void SpawnObject(GameObject randomPrefab)
    {
        Bounds bounds = Ground.GetComponent<Renderer>().bounds;
        Vector3 objectSize = randomPrefab.GetComponent<Renderer>().bounds.size;

        float x = UnityEngine.Random.Range(bounds.min.x + objectSize.x / 2, bounds.max.x - objectSize.x / 2);
        float z = UnityEngine.Random.Range(bounds.min.z + objectSize.z / 2, bounds.max.z - objectSize.z / 2);

        Instantiate(randomPrefab, new Vector3(x, objectSize.y / 2, z), Quaternion.identity, objectParent.transform);
    }
}

[Serializable]
public class ObjectSpawnSettings
{
    public List<GameObject> prefabPool = new();
    public int amount;
}