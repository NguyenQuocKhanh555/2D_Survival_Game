using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGenerator : MonoBehaviour
{
    [Header("Map settings")]
    public int mapSize = 1000; // must be even to center nicely
    public int biomeCount = 5;
    public bool autoGenerateOnStart = false;
    public int randomSeed = 0;
    public bool useFixedSeed = false;

    [Header("Biome tiles (size = biomeCount)")]
    public TileBase[] biomeGroundTiles; // ground tiles per biome
    public TileBase[] biomeWaterTiles;  // water tiles per biome

    [Header("Tilemap")]
    public Tilemap groundTilemap;
    public Tilemap waterTilemap;
    public TilemapCollider2D waterTilemapCollider;
    public CompositeCollider2D waterCompositeCollider;
    public GameObject[] biomeTreePrefabs; 
    public GameObject[] biomeOrePrefabs;

    [Range(0f, 1f)] public float treeSpawnChance = 0.02f;
    [Range(0f, 1f)] public float oreSpawnChance = 0.005f;

    [Header("Voronoi / Noise smoothing")]
    [Range(0f, 1f)] public float noiseStrength = 0.15f; // how much Perlin noise biases Voronoi
    public float noiseScale = 100f; // larger = smoother noise

    [Header("Lakes")]
    public int minLakesPerBiome = 2;
    public int maxLakesPerBiome = 5;
    public int minLakeRadius = 4;
    public int maxLakeRadius = 20;
    [Range(0f, 1f)] public float maxLakeBiomeFraction = 0.06f; // lake can't occupy more than this fraction of biome

    [Header("Animal Spawn")]
    public GameObject[] animalPrefabs;
    public int animalCount = 10;
    public float spawnRadius = 50f;

    [Header("Performance")]
    public int tilesPerYield = 20000; // yield to next frame to avoid freeze in editor/play
    public int objectsPerYield = 300;

    // internal
    private Vector2Int[] biomeCenters;
    private System.Random rng;

    // Public helper to launch generation (can be called from inspector via custom button or other scripts)
    [ContextMenu("Generate Map")]
    public void GenerateMap()
    {
        StopAllCoroutines();
        if (groundTilemap == null)
        {
            Debug.LogError("Target Tilemap not assigned!");
            return;
        }

        if (biomeGroundTiles == null || biomeGroundTiles.Length < biomeCount)
        {
            Debug.LogError("Please assign biomeGroundTiles array with at least biomeCount elements.");
            return;
        }

        if (biomeWaterTiles == null || biomeWaterTiles.Length < biomeCount)
        {
            Debug.LogError("Please assign biomeWaterTiles array with at least biomeCount elements.");
            return;
        }

        if (useFixedSeed)
            rng = new System.Random(randomSeed);
        else
            rng = new System.Random();

        StartCoroutine(GenerateMapCoroutine());       
    }

    private void Start()
    {
        if (autoGenerateOnStart)
        {
            GenerateMap();
        }     
    }

    private IEnumerator GenerateMapCoroutine()
    {
        float t0 = Time.realtimeSinceStartup;
        int half = mapSize / 2;

        // 1) Choose biome centers
        biomeCenters = new Vector2Int[biomeCount];
        // simple Poisson-ish: random but try to space them
        for (int i = 0; i < biomeCount; i++)
        {
            // pick random with spacing attempt
            Vector2Int candidate;
            int attempts = 0;
            do
            {
                candidate = new Vector2Int(
                    rng.Next(-half, half),
                    rng.Next(-half, half)
                );
                attempts++;
                if (attempts > 30) break;
            } while (!IsFarFromExistingCenters(candidate, (mapSize / biomeCount) * 0.25f));
            biomeCenters[i] = candidate;
        }

        // 2) Build biome assignment + list of positions per biome
        // We'll prepare a tile array for the whole map and a biome id per tile if needed
        BoundsInt fullBounds = new BoundsInt(-half, -half, 0, mapSize, mapSize, 1);
        bool[] isLakeTile;
        int totalTiles = mapSize * mapSize;
        //TileBase[] tiles = new TileBase[totalTiles];
        TileBase[] groundTiles = new TileBase[totalTiles];
        TileBase[] lakeTiles = new TileBase[totalTiles];
        isLakeTile = new bool[totalTiles];
        int[] biomeIdPerTile = new int[totalTiles]; // store assigned biome id for later lake checks

        // Precompute Perlin offsets (randomize)
        float noiseOffsetX = (float)rng.NextDouble() * 1000f;
        float noiseOffsetY = (float)rng.NextDouble() * 1000f;

        int processed = 0;

        for (int y = -half; y < half; y++)
        {
            for (int x = -half; x < half; x++)
            {
                // compute index into linear array (row-major)
                int ix = x - fullBounds.xMin;
                int iy = y - fullBounds.yMin;
                int idx = ix + iy * fullBounds.size.x;

                // 3) Voronoi nearest center, with Perlin noise bias
                float bestScore = float.MaxValue;
                int bestBiome = 0;

                // per-tile noise in [0,1]
                float noise = Mathf.PerlinNoise((x + noiseOffsetX) / noiseScale, (y + noiseOffsetY) / noiseScale);
                // translate noise to a small offset value (center bias)
                float noiseBias = (noise - 0.5f) * 2f * noiseStrength * mapSize; // scale bias to map scale

                for (int b = 0; b < biomeCount; b++)
                {
                    Vector2Int c = biomeCenters[b];

                    // Euclidean distance squared
                    float dx = x - c.x;
                    float dy = y - c.y;
                    float dist = dx * dx + dy * dy;

                    // Apply bias: we subtract bias * dot(dirToCenter, someAxis?) 
                    // simpler: vary dist by a small per-biome noise bias using the tile noise and biome index
                    float adjustedDist = dist + (b * 0.0001f) + (noiseBias * (b - (biomeCount / 2f)));

                    if (adjustedDist < bestScore)
                    {
                        bestScore = adjustedDist;
                        bestBiome = b;
                    }
                }

                // assign ground tile for that biome
                //tiles[idx] = biomeGroundTiles[bestBiome];
                groundTiles[idx] = biomeGroundTiles[bestBiome];
                lakeTiles[idx] = null;
                biomeIdPerTile[idx] = bestBiome;

                processed++;
                if (processed >= tilesPerYield)
                {
                    processed = 0;
                    // yield one frame to avoid lockup
                    yield return null;
                }
            }
        }

        // 4) Build position lists per biome (for lake placement)
        List<int>[] biomeTileIndices = new List<int>[biomeCount];
        for (int i = 0; i < biomeCount; i++) biomeTileIndices[i] = new List<int>();

        for (int i = 0; i < totalTiles; i++)
            biomeTileIndices[biomeIdPerTile[i]].Add(i);

        // 5) Generate lakes per biome and override tiles array
        for (int b = 0; b < biomeCount; b++)
        {
            int tileCount = biomeTileIndices[b].Count;
            if (tileCount == 0) continue;

            int lakeCount = rng.Next(minLakesPerBiome, maxLakesPerBiome + 1);

            for (int li = 0; li < lakeCount; li++)
            {
                // pick random center index from biome positions
                int pickIndex = biomeTileIndices[b][rng.Next(0, biomeTileIndices[b].Count)];
                // convert pickIndex to (x,y)
                int pickIx = pickIndex % fullBounds.size.x;
                int pickIy = pickIndex / fullBounds.size.x;
                int cx = pickIx + fullBounds.xMin;
                int cy = pickIy + fullBounds.yMin;

                int radius = rng.Next(minLakeRadius, maxLakeRadius + 1);

                // ensure lake isn't too big relative to biome; if too big shrink radius
                float area = Mathf.PI * radius * radius;
                float maxArea = tileCount * maxLakeBiomeFraction;
                if (area > maxArea)
                {
                    // scale down radius
                    radius = Mathf.Max(1, Mathf.FloorToInt(Mathf.Sqrt(maxArea / Mathf.PI)));
                }

                // carve roughly circular lake (you can add perlin for jagged shore)
                int r2 = radius * radius;
                for (int dy = -radius; dy <= radius; dy++)
                {
                    int y = cy + dy;
                    if (y < fullBounds.yMin || y >= fullBounds.yMax) continue;

                    int dy2 = dy * dy;
                    for (int dx = -radius; dx <= radius; dx++)
                    {
                        int x = cx + dx;
                        if (x < fullBounds.xMin || x >= fullBounds.xMax) continue;

                        if (dx * dx + dy2 <= r2)
                        {
                            int ix = x - fullBounds.xMin;
                            int iy = y - fullBounds.yMin;
                            int idx = ix + iy * fullBounds.size.x;

                            // Only carve lake if this tile currently belongs to the same biome (avoid lakes spanning biomes)
                            if (biomeIdPerTile[idx] == b)
                            {
                                //tiles[idx] = biomeWaterTiles[b];
                                lakeTiles[idx] = biomeWaterTiles[b];
                                isLakeTile[idx] = true;
                            }
                        }
                    }
                }
            }
            // yield to keep responsive
            yield return null;
        }

        // 6) Apply to Tilemap using SetTilesBlock (single call)
        groundTilemap.ClearAllTiles();
        waterTilemap.ClearAllTiles();
        groundTilemap.SetTilesBlock(fullBounds, groundTiles);
        waterTilemap.SetTilesBlock(fullBounds, lakeTiles);

        waterTilemap.RefreshAllTiles();
        waterTilemap.CompressBounds();
        waterTilemapCollider.ProcessTilemapChanges();
        waterCompositeCollider.GenerateGeometry();

        StartCoroutine(SpawnBiomeObjectsCoroutine(fullBounds, biomeIdPerTile, isLakeTile));
        StartCoroutine(SpawnAnimalsCoroutine());

        float elapsed = Time.realtimeSinceStartup - t0;
        Debug.Log($"Map generated: {mapSize}×{mapSize}, biomes={biomeCount}. Time: {elapsed:F2}s");
        yield break;
    }

    private bool IsFarFromExistingCenters(Vector2Int candidate, float minDist)
    {
        for (int i = 0; i < biomeCenters?.Length; i++)
        {
            if (biomeCenters[i] == Vector2Int.zero) continue; // uninitialized
            if (Vector2Int.Distance(candidate, biomeCenters[i]) < minDist)
                return false;
        }
        return true;
    }

    private IEnumerator SpawnBiomeObjectsCoroutine(BoundsInt bounds, int[] biomeIdPerTile, bool[] isLakeTile)
    {
        int spawned = 0;

        for (int i = 0; i < biomeIdPerTile.Length; i++)
        {
            if (isLakeTile[i]) continue;

            int biome = biomeIdPerTile[i];

            int ix = i % bounds.size.x;
            int iy = i / bounds.size.x;

            Vector3Int cellPos = new Vector3Int(
                ix + bounds.xMin,
                iy + bounds.yMin,
                0
            );

            Vector3 worldPos = groundTilemap.CellToWorld(cellPos) + new Vector3(0.5f, 0f, 0f);
            worldPos.z = 0f;

            // 🌳 Tree
            if (biomeTreePrefabs[biome] != null &&
                rng.NextDouble() < treeSpawnChance)
            {
                Instantiate(
                    biomeTreePrefabs[biome],
                    worldPos,
                    Quaternion.identity//,
                    //objectParent
                );
                spawned++;
            }
            // ⛏ Ore
            else if (biomeOrePrefabs[biome] != null &&
                     rng.NextDouble() < oreSpawnChance)
            {
                Instantiate(
                    biomeOrePrefabs[biome],
                    worldPos,
                    Quaternion.identity//,
                    //objectParent
                );
                spawned++;
            }

            if (spawned >= objectsPerYield)
            {
                spawned = 0;
                yield return null; // nhường frame
            }
        }

        Debug.Log("Finished spawning biome objects.");
    }

    public IEnumerator SpawnAnimalsCoroutine()
    {
        if (animalPrefabs == null || animalPrefabs.Length == 0)
            yield break;

        int spawned = 0;

        while (spawned < animalCount)
        {
            float angle = (float)(rng.NextDouble() * Mathf.PI * 2f);
            float radius = spawnRadius * Mathf.Sqrt((float)rng.NextDouble());

            Vector3 pos = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );

            int prefabIndex = rng.Next(0, animalPrefabs.Length);
            GameObject prefab = animalPrefabs[prefabIndex];

            if (!Physics2D.OverlapCircle(pos, 0.5f))
            {
                Instantiate(prefab, pos, Quaternion.identity);
                spawned++;
            }

            yield return null; // 1 con / frame
        }
    }

}
