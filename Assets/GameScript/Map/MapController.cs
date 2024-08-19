using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 playerLastPosition;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float opCooldown;
    public float opCooldownDuration;

    void Start()
    {
        playerLastPosition = player.transform.position;
    }
    void Update()
    {
        ChunkCheker();
        ChunkOptimizer();
    }
    void ChunkCheker()
    {
        if (!currentChunk)
        {
            return;
        }
        Vector3 moveDir = player.transform.position - playerLastPosition;
        playerLastPosition = player.transform.position;

        string directionName = GetDirectionName(moveDir);

        CheckAndSpawnChunk(directionName);

        string[] directions = { "Up", "Down", "Left", "Right", "Right Up", "Right Down", "Left Up", "Left Down" };

        foreach (string dir in directions)
        {
            CheckAndSpawnChunk(dir);
        }
    }
    void CheckAndSpawnChunk(string direction)
    {
        Transform directionTransform = currentChunk.transform.Find(direction);

        if (directionTransform != null && !Physics2D.OverlapCircle(directionTransform.position, checkerRadius, terrainMask))
        {
            SpawnChunk(directionTransform.position);
        }
    }
    string GetDirectionName(Vector3 direction)
    {
        direction = direction.normalized;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Moving horizontally more than vertically
            if (direction.y > 0.5f)
            {
                // Moving diagonally up
                return direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (direction.y < -0.5f)
            {
                // Moving diagonally down
                return direction.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                // Moving straight horizontally
                return direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            // Moving vertically more than horizontally
            if (direction.x > 0.5f)
            {
                // Moving diagonally right
                return direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if (direction.x < -0.5f)
            {
                // Moving diagonally left
                return direction.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                // Moving straight vertically
                return direction.y > 0 ? "Up" : "Down";
            }
        }
    }

    void SpawnChunk(Vector3 spawnPosition)
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], spawnPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimizer()
    {
        opCooldown -= Time.deltaTime;
        if (opCooldown <= 0f)
        {
            opCooldown = opCooldownDuration;
        } else {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            } else {
                chunk.SetActive(true);
            }
        }
    }


}
