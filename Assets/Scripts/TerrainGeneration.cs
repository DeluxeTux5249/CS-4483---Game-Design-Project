using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class TerrainGeneration : MonoBehaviour
{
    public Transform player;
    public Tilemap groundTilemap;
    public Tilemap decorationTilemap;

    public TileBase[] grassTile;
    public TileBase[] decorationTiles;

    public int chunkSize = 16;
    public int renderDistance = 3;

    public float noiseScale = 0.1f;
    public int seed = 0;
    public bool randomSeed = true;

    [Range(0f, 1f)] public float decorationDensity = 0.025f;

    public GameObject doorPrefab;
    [Range(0f, 1f)] public float doorChancePerChunk = 0.3f;

    private Dictionary<Vector2Int, ChunkData> generatedChunks = new Dictionary<Vector2Int, ChunkData>();
    private Dictionary<Vector2Int, GameObject> spawnedDoors = new Dictionary<Vector2Int, GameObject>();
    private Vector2Int lastPlayerChunk;
    private System.Random rnd;

    private class ChunkData
    {
        public Dictionary<Vector2Int, int> grassIndices = new Dictionary<Vector2Int, int>();
        public Dictionary<Vector2Int, int> decorations = new Dictionary<Vector2Int, int>();
        public Vector2Int? doorLocalPos = null; // null = no door in this chunk
    }

    private void Start()
    {
        if (randomSeed)
            seed = UnityEngine.Random.Range(0, 100000);

        rnd = new System.Random(seed);

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        lastPlayerChunk = WorldToChunk(player.position);
        LoadChunksAround(lastPlayerChunk);
    }

    private void Update()
    {
        Vector2Int currentChunk = WorldToChunk(player.position);
        if (currentChunk != lastPlayerChunk)
        {
            lastPlayerChunk = currentChunk;
            LoadChunksAround(currentChunk);
        }
    }

    private void LoadChunksAround(Vector2Int center)
    {
        for (int cx = center.x - renderDistance; cx <= center.x + renderDistance; cx++)
        {
            for (int cy = center.y - renderDistance; cy <= center.y + renderDistance; cy++)
            {
                Vector2Int chunkCoord = new Vector2Int(cx, cy);
                if (!generatedChunks.ContainsKey(chunkCoord))
                    GenerateChunk(chunkCoord);
                else
                    RenderChunk(chunkCoord);
            }
        }
    }

    private void GenerateChunk(Vector2Int chunkCoord)
    {
        ChunkData data = new ChunkData();
        int worldOriginX = chunkCoord.x * chunkSize;
        int worldOriginY = chunkCoord.y * chunkSize;
        

        for (int lx = 0; lx < chunkSize; lx++)
        {
            for (int ly = 0; ly < chunkSize; ly++)
            {
                int wx = worldOriginX + lx;
                int wy = worldOriginY + ly;

                Vector3Int tilePos = new Vector3Int(wx, wy, 0);
                if (grassTile != null && grassTile.Length > 0)
                {
                    int grassIndex = PickGrassIndex();
                    data.grassIndices[new Vector2Int(lx, ly)] = grassIndex;
                    groundTilemap.SetTile(tilePos, grassTile[grassIndex]);
                }

                if (decorationTiles != null && decorationTiles.Length > 0)
                {
                    var tileRnd = new System.Random(seed ^ (wx * 73856093) ^ (wy * 19349663));

                    if (tileRnd.NextDouble() < decorationDensity)
                    {
                        int index = tileRnd.Next(0, decorationTiles.Length);
                        decorationTilemap.SetTile(tilePos, decorationTiles[index]);
                        data.decorations[new Vector2Int(lx, ly)] = index;
                    }
                    else
                    {
                        data.decorations[new Vector2Int(lx, ly)] = -1;
                    }
                }
            }
        }

        generatedChunks[chunkCoord] = data;

        // Roll to see if this chunk gets a door
        var chunkRnd = new System.Random(seed ^ (chunkCoord.x * 92837111) ^ (chunkCoord.y * 689287499));
        if (doorPrefab != null && chunkRnd.NextDouble() < doorChancePerChunk)
        {
            // Pick a random interior tile for the door
            int lx = chunkRnd.Next(2, chunkSize - 2);
            int ly = chunkRnd.Next(2, chunkSize - 2);
            data.doorLocalPos = new Vector2Int(lx, ly);
            SpawnDoor(chunkCoord, lx, ly);
        }
    }

    private void SpawnDoor(Vector2Int chunkCoord, int lx, int ly)
    {
        int wx = chunkCoord.x * chunkSize + lx;
        int wy = chunkCoord.y * chunkSize + ly;
        Vector3 worldPos = new Vector3(wx + 0.5f, wy + 0.5f, 0);

        // Don't spawn if a hand-placed dungeon entrance exists nearby
        foreach (var existing in FindObjectsByType<Door>(FindObjectsSortMode.None))
        {
            if (Vector3.Distance(existing.transform.position, worldPos) < chunkSize * 0.5f)
                return;
        }

        GameObject door = Instantiate(doorPrefab, worldPos, Quaternion.identity);


        spawnedDoors[chunkCoord] = door;
    }

    private void RenderChunk(Vector2Int chunkCoord)
    {
        ChunkData data = generatedChunks[chunkCoord];
        int worldOriginX = chunkCoord.x * chunkSize;
        int worldOriginY = chunkCoord.y * chunkSize;

        // Re-spawn door if it had one but the GameObject is gone
        if (data.doorLocalPos.HasValue && doorPrefab != null &&
            (!spawnedDoors.ContainsKey(chunkCoord) || spawnedDoors[chunkCoord] == null))
        {
            SpawnDoor(chunkCoord, data.doorLocalPos.Value.x, data.doorLocalPos.Value.y);
        }

        foreach (var entry in data.decorations)
        {
            int wx = worldOriginX + entry.Key.x;
            int wy = worldOriginY + entry.Key.y;
            Vector3Int tilePos = new Vector3Int(wx, wy, 0);

            if (grassTile != null && data.grassIndices.TryGetValue(entry.Key, out int gi))
                groundTilemap.SetTile(tilePos, grassTile[gi]);

            if (entry.Value >= 0 && decorationTiles != null && entry.Value < decorationTiles.Length)
                decorationTilemap.SetTile(tilePos, decorationTiles[entry.Value]);
        }
    }

    private int PickGrassIndex()
    {
        if (grassTile.Length == 1) return 0;

        double roll = rnd.NextDouble();
        if (roll < 0.9 && grassTile.Length > 1)
            return 1;

        int pick = rnd.Next(0, grassTile.Length - 1);
        if (pick >= 1) pick++;
        return pick;
    }

    private Vector2Int WorldToChunk(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPos.x / chunkSize),
            Mathf.FloorToInt(worldPos.y / chunkSize)
        );
    }
}
