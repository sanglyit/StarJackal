using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;    // List of possible terrain chunks to spawn
    public GameObject Player;                 // Player GameObject
    public int mapWidth = 5;                  // Number of chunks in the horizontal direction
    public int mapHeight = 5;                 // Number of chunks in the vertical direction
    public float chunkSize = 30f;           // Size of each chunk

    private GameObject[,] mapChunks;          // 2D array to hold the chunks
    private Vector2 mapSize;                  // Size of the map in world coordinates

    void Start()
    {
        // Calculate the size of the map in world coordinates
        mapSize = new Vector2(mapWidth * chunkSize, mapHeight * chunkSize);

        // Initialize the 2D array to hold the map chunks
        mapChunks = new GameObject[mapWidth, mapHeight];

        // Generate the map chunks
        GenerateMap();

        // Create the invisible borders
        CreateBorders();

        // Position the player at the center of the map
        Player.transform.position = new Vector3(mapSize.x / 2, mapSize.y / 2, 0);
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                Vector3 spawnPosition = new Vector3(x * chunkSize, y * chunkSize, 0);
                int rand = Random.Range(0, terrainChunks.Count);
                mapChunks[x, y] = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
            }
        }
    }

    void CreateBorders()
    {
        // Left border
        CreateBorder(new Vector2(-chunkSize / 2 + 20, (mapSize.y / 2) - 14.5f), new Vector2(1, mapSize.y));

        // Right border
        CreateBorder(new Vector2((mapSize.x + chunkSize / 2) - 50, (mapSize.y / 2) - 14.5f), new Vector2(1, mapSize.y));

        // Bottom border
        CreateBorder(new Vector2(mapSize.x / 2 - 15.3f, -chunkSize / 2 + 20), new Vector2(mapSize.x, 1));

        // Top border
        CreateBorder(new Vector2(mapSize.x / 2 - 15.3f, mapSize.y + chunkSize / 2 - 50), new Vector2(mapSize.x, 1));
    }

    void CreateBorder(Vector2 position, Vector2 size)
    {
        GameObject border = new GameObject("Border");
        border.transform.position = position;
        BoxCollider2D collider = border.AddComponent<BoxCollider2D>();
        collider.size = size;
        collider.isTrigger = false;  // Ensure it's not a trigger so it blocks movement
    }
}
