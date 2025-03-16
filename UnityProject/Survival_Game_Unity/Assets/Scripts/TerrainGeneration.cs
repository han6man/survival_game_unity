using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private CamController mainCamera;
    [SerializeField] private int tileSortingOrder = -5;

    [Header("Tile Atlas")]
    [SerializeField] private TileAtlas tileAtlas;
    [SerializeField] private float seed;
    [SerializeField] private BiomeClass[] biomes;

    [Header("Biomes")]
    [SerializeField] private float biomeFrequency;
    [SerializeField] private Gradient biomeGradient;
    [SerializeField] private Texture2D biomeMap;

    [Header("Generation Settings")]
    [SerializeField] private int chunkSize = 20;
    [SerializeField] private int worldSize = 100;
    [SerializeField] private int heightAddition = 50;
    [SerializeField] private bool generateCaves = true;

    [Header("Noise Settings")]
    [SerializeField] private Texture2D caveNoiseTexture;
    [SerializeField] private float terrainFreq = 0.05f;
    [SerializeField] private float caveFreq = 0.05f;

    //[Header("Ore Settings")]
    [SerializeField] private OreClass[] ores;
    private GameObject[] worldChunks;
    private List<Vector2> worldTiles = new List<Vector2>();
    private List<GameObject> worldTileObjects = new List<GameObject>();

    private BiomeClass curBiome;
    private Color[] biomeCols;

    private void Start()
    {
        seed = Random.Range(-10000, 10000);

        biomeCols = new Color[biomes.Length];
        for (int i = 0; i < biomes.Length; i++)
        {
            biomeCols[i] = biomes[i].biomeCol;
        }        

        //DrawTextures();
        DrawBiomeMap();
        DrawCavesAndOres();

        CreateChunks();
        GenerateTerrain();

        mainCamera.Spawn(new Vector3(player.spawnPos.x, player.spawnPos.y, mainCamera.transform.position.z));
        mainCamera.worldSize = worldSize;

        player.Spawn();

        RefreshChunks();
    }

    private void Update()
    {
        RefreshChunks();
    }

    private void RefreshChunks()
    {
        for (int i = 0; i < worldChunks.Length; i++)
        {
            if (Vector2.Distance(new Vector2((i * chunkSize) + (chunkSize / 2), 0), new Vector2(player.transform.position.x, 0)) > Camera.main.orthographicSize * 4f)
            {
                worldChunks[i].SetActive(false);
            }
            else
            {
                worldChunks[i].SetActive(true);
            }
        }
    }

    private void DrawBiomeMap()
    {
        float b;
        Color col;
        biomeMap = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < biomeMap.width; x++)
        {
            for (int y = 0; y < biomeMap.height; y++)
            {
                b = Mathf.PerlinNoise((x + seed) * biomeFrequency, (y + seed) * biomeFrequency);
                col = biomeGradient.Evaluate(b);
                biomeMap.SetPixel(x, y, col);
            }
        }

        biomeMap.Apply();
    }
    private void DrawCavesAndOres()
    {
        float v;
        float o;
        caveNoiseTexture = new Texture2D(worldSize, worldSize);

        for (int x = 0; x < caveNoiseTexture.width; x++)
        {
            for (int y = 0; y < caveNoiseTexture.height; y++)
            {
                curBiome = GetCurentBiome(x, y);
                v = Mathf.PerlinNoise((x + seed) * caveFreq, (y + seed) * caveFreq);
                if (v > curBiome.surfaceValue)
                {
                    caveNoiseTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    caveNoiseTexture.SetPixel(x, y, Color.black);
                }

                for (int i = 0; i < ores.Length; i++)
                {
                    ores[i].spreadTexture.SetPixel(x, y, Color.black);
                    if (curBiome.ores.Length > 0)
                    {
                        o = Mathf.PerlinNoise((x + seed) * curBiome.ores[i].frequency, (y + seed) * curBiome.ores[i].frequency);
                        if (o > curBiome.ores[i].size)
                            ores[i].spreadTexture.SetPixel(x, y, Color.white);

                        ores[i].spreadTexture.Apply();
                    }
                }
            }
        }

        caveNoiseTexture.Apply();
    }

    private void DrawTextures()
    {
        biomeMap = new Texture2D(worldSize, worldSize);
        caveNoiseTexture = new Texture2D(worldSize, worldSize);

        for (int i = 0; i < ores.Length; i++)
        {
            ores[i].spreadTexture = new Texture2D(worldSize, worldSize);
        }
        
        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i].caveNoiseTexture = new Texture2D(worldSize, worldSize);
            for (int j = 0; j < biomes[i].ores.Length; j++)
            {
                biomes[i].ores[j].spreadTexture = new Texture2D(worldSize, worldSize);
                GenerateNoiseTextures(biomes[i].ores[j].frequency, biomes[i].ores[j].size, biomes[i].ores[j].spreadTexture);
            }
        }
        
    }

    private void GenerateNoiseTextures(float frequency, float limit, Texture2D noiseTexture)
    {
        float v;

        for (int x = 0; x < noiseTexture.width; x++)
        {
            for (int y = 0; y < noiseTexture.height; y++)
            {
                v = Mathf.PerlinNoise((x + seed) * frequency, (y + seed) * frequency);

                if (v > limit)
                {
                    noiseTexture.SetPixel(x, y, Color.white);
                }
                else
                {
                    noiseTexture.SetPixel(x, y, Color.black);
                }
            }
        }

        noiseTexture.Apply();
    }

    public void CreateChunks()
    {
        int numChunks = worldSize / chunkSize;
        worldChunks = new GameObject[numChunks];

        for (int i = 0; i < numChunks; i++)
        {
            GameObject newChunk = new GameObject();
            newChunk.name = i.ToString();
            newChunk.transform.parent = this.transform;
            worldChunks[i] = newChunk;
        }
    }

    private BiomeClass GetCurentBiome(int x, int y)
    {
        if (System.Array.IndexOf(biomeCols, biomeMap.GetPixel(x, y)) >= 0)
            return biomes[System.Array.IndexOf(biomeCols, biomeMap.GetPixel(x, y))];

        return curBiome;
    }

    private void GenerateTerrain()
    {
        Sprite[] tileSprites;
        for (int x = 0; x < worldSize; x++)
        {
            float height;

            for (int y = 0; y < worldSize; y++)
            {
                curBiome = GetCurentBiome(x, y);
                height = Mathf.PerlinNoise((x + seed) * terrainFreq, seed * terrainFreq) * curBiome.heightMultiplier + heightAddition;
                if (x == worldSize / 2)
                    player.spawnPos = new Vector2(x, height + 2);

                if (y >= height)
                    break;

                if (y < height - curBiome.dirtLayerHeight)
                {
                    tileSprites = curBiome.tileAtlas.stone.tileSprites;

                    if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].maxSpawnHeight)
                        tileSprites = tileAtlas.coal.tileSprites;
                    if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].maxSpawnHeight)
                        tileSprites = tileAtlas.iron.tileSprites;
                    if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].maxSpawnHeight)
                        tileSprites = tileAtlas.gold.tileSprites;
                    if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].maxSpawnHeight)
                        tileSprites = tileAtlas.diamond.tileSprites;
                }
                else if (y < height - 1)
                {
                    tileSprites = curBiome.tileAtlas.dirt.tileSprites;
                }
                else
                {
                    // top layer of the terrain
                    tileSprites = curBiome.tileAtlas.grass.tileSprites;
                }

                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                    {
                        PlaceTile(tileSprites, x, y, false);
                    }
                }
                else
                {
                    PlaceTile(tileSprites, x, y, false);
                }


                if (y >= height - 1)
                {
                    int t = Random.Range(0, curBiome.treeChance);

                    if (t == 1)
                    {
                        //generate a tree
                        if (worldTiles.Contains(new Vector2(x, y)))
                        {
                            if (curBiome.biomeName == "Desert")
                            {
                                //generete cactus
                                GenerateCactus(curBiome.tileAtlas, Random.Range(curBiome.minTreeHeight, curBiome.maxTreeHeight), x, y + 1);
                            }
                            else
                            {
                                GenerateTree(Random.Range(curBiome.minTreeHeight, curBiome.maxTreeHeight), x, y + 1);
                            }
                        }
                    }
                    else
                    {
                        int i = Random.Range(0, curBiome.tallGrassChance);
                        //generate grass
                        if (i == 1)
                        {
                            if (worldTiles.Contains(new Vector2(x, y)))
                            {
                                if (curBiome.tileAtlas.tallGrass != null)
                                {
                                    PlaceTile(curBiome.tileAtlas.tallGrass.tileSprites, x, y + 1, true);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void GenerateCactus(TileAtlas atlas, int treeHeight, int x, int y)
    {
        //define our Cactus

        //generate log
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(atlas.log.tileSprites, x, y + i, true);
        }
    }

    private void GenerateTree(int treeHeight, int x, int y)
    {
        //define our tree

        //generate log
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(tileAtlas.log.tileSprites, x, y + i, true);
        }

        //generate leaves
        PlaceTile(tileAtlas.leaf.tileSprites, x, y + treeHeight, true);
        PlaceTile(tileAtlas.leaf.tileSprites, x, y + treeHeight + 1, true);
        PlaceTile(tileAtlas.leaf.tileSprites, x, y + treeHeight + 2, true);

        PlaceTile(tileAtlas.leaf.tileSprites, x - 1, y + treeHeight, true);
        PlaceTile(tileAtlas.leaf.tileSprites, x - 1, y + treeHeight + 1, true);

        PlaceTile(tileAtlas.leaf.tileSprites, x + 1, y + treeHeight, true);
        PlaceTile(tileAtlas.leaf.tileSprites, x + 1, y + treeHeight + 1, true);
    }

    public void RemoveTile(int x, int y)
    {
        if (worldTiles.Contains(new Vector2Int(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            Destroy(worldTileObjects[worldTiles.IndexOf(new Vector2(x, y))]);
        }
    }

    private void PlaceTile(Sprite[] tileSprites, int x, int y, bool backgroundElement)
    {
        if (!worldTiles.Contains(new Vector2Int(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            GameObject newTile = new GameObject();
            int chunkCoord = Mathf.RoundToInt(Mathf.Round(x / chunkSize) * chunkSize);
            chunkCoord /= chunkSize;

            newTile.transform.parent = worldChunks[chunkCoord].transform;

            newTile.AddComponent<SpriteRenderer>();
            if (!backgroundElement)
            {
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }

            int spriteIndex = Random.Range(0, tileSprites.Length);
            newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[spriteIndex];
            newTile.GetComponent<SpriteRenderer>().sortingOrder = tileSortingOrder;

            newTile.name = tileSprites[0].name;
            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

            worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
            worldTileObjects.Add(newTile);
        }
    }
}
