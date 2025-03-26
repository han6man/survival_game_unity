using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGeneration : MonoBehaviour
{
    [Header("Lighting")]
    [SerializeField] private Texture2D worldTilesMap;
    [SerializeField] private Material lightShader;
    [SerializeField] private float lightThreshold;
    [SerializeField] private float lightRadius = 7f;
    private List<Vector2Int> unlitBlocks = new List<Vector2Int>();

    [SerializeField] private PlayerController player;
    [SerializeField] private CamController mainCamera;
    [SerializeField] private GameObject tileDrop;
    [SerializeField] private int tileSortingOrderInBackground = -10;
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
    //private List<TileClass> worldTileClasses = new List<TileClass>();

    private TileClass[,] world_BackgroundTiles;
    private TileClass[,] world_ForegroundTiles;

    private BiomeClass curBiome;
    private Color[] biomeCols;

    private void Start()
    {
        world_ForegroundTiles = new TileClass[worldSize, worldSize];
        world_BackgroundTiles = new TileClass[worldSize, worldSize];

        //initilise light
        worldTilesMap = new Texture2D(worldSize, worldSize);
        //worldTilesMap.filterMode = FilterMode.Point;
        lightShader.SetTexture("_shadowTexture", worldTilesMap);

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                worldTilesMap.SetPixel(x, y, Color.white);
            }
        }
        worldTilesMap.Apply();

        //generate terrain stuff
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

        for (int x = 0; x < worldSize; x++)
        {
            for (int y = 0; y < worldSize; y++)
            {
                if (worldTilesMap.GetPixel(x, y) == Color.white)
                    LightBlock(x, y, 1f, 0);
            }
        }
        worldTilesMap.Apply();

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
            if (Vector2.Distance(new Vector2((i * chunkSize) + (chunkSize / 2), 0), new Vector2(player.transform.position.x, 0)) > Camera.main.orthographicSize * 5f)
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
        TileClass tileClass;
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
                    tileClass = curBiome.tileAtlas.stone;

                    if (ores[0].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[0].maxSpawnHeight)
                        tileClass = tileAtlas.coal;
                    if (ores[1].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[1].maxSpawnHeight)
                        tileClass = tileAtlas.iron;
                    if (ores[2].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[2].maxSpawnHeight)
                        tileClass = tileAtlas.gold;
                    if (ores[3].spreadTexture.GetPixel(x, y).r > 0.5f && height - y > ores[3].maxSpawnHeight)
                        tileClass = tileAtlas.diamond;
                }
                else if (y < height - 1)
                {
                    tileClass = curBiome.tileAtlas.dirt;
                }
                else
                {
                    // top layer of the terrain
                    tileClass = curBiome.tileAtlas.grass;
                }

                if (generateCaves)
                {
                    if (caveNoiseTexture.GetPixel(x, y).r > 0.5f)
                    {
                        PlaceTile(tileClass, x, y, true);
                    }
                    else if (tileClass.wallVariant != null)
                    {
                        PlaceTile(tileClass.wallVariant, x, y, true);
                    }
                }
                else
                {
                    PlaceTile(tileClass, x, y, true);
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
                                    PlaceTile(curBiome.tileAtlas.tallGrass, x, y + 1, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        worldTilesMap.Apply();
    }

    private void GenerateCactus(TileAtlas atlas, int treeHeight, int x, int y)
    {
        //define our Cactus

        //generate log
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(atlas.log, x, y + i, true);
        }
    }

    private void GenerateTree(int treeHeight, int x, int y)
    {
        //define our tree

        //generate log
        for (int i = 0; i < treeHeight; i++)
        {
            PlaceTile(tileAtlas.log, x, y + i, true);
        }

        //generate leaves
        PlaceTile(tileAtlas.leaf, x, y + treeHeight, true);
        PlaceTile(tileAtlas.leaf, x, y + treeHeight + 1, true);
        PlaceTile(tileAtlas.leaf, x, y + treeHeight + 2, true);

        PlaceTile(tileAtlas.leaf, x - 1, y + treeHeight, true);
        PlaceTile(tileAtlas.leaf, x - 1, y + treeHeight + 1, true);

        PlaceTile(tileAtlas.leaf, x + 1, y + treeHeight, true);
        PlaceTile(tileAtlas.leaf, x + 1, y + treeHeight + 1, true);
    }

    public void RemoveTile(int x, int y)
    {
        TileClass tile = GetTileFromWorld(x, y);

        if (worldTiles.Contains(new Vector2Int(x, y)) && x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            if (tile.wallVariant != null)
            {
                //if the tile is naturally placed
                if (tile.naturallyPlaced)
                {
                    PlaceTile(tile.wallVariant, x, y, true);
                }                
            }
            
            Destroy(worldTileObjects[worldTiles.IndexOf(new Vector2(x, y))]);
            
            //drop tile
            if (tile.tileDrop)
            {
                GameObject newTileDrop = Instantiate(tileDrop, new Vector2(x, y + 0.5f), Quaternion.identity);
                newTileDrop.GetComponent<SpriteRenderer>().sprite = tile.tileDrop;
            }

            worldTileObjects.RemoveAt(worldTiles.IndexOf(new Vector2(x, y)));

            RemoveTileFromWorld(x, y);

            if (GetTileFromWorld(x, y))
            {
                if (GetTileFromWorld(x, y).inBackground && GetTileFromWorld(x, y).name.ToLower().Contains("wall"))
                {
                    worldTilesMap.SetPixel(x, y, Color.white);
                    LightBlock(x, y, 1f, 0);
                }
            }

            worldTiles.RemoveAt(worldTiles.IndexOf(new Vector2(x, y)));

            worldTilesMap.Apply();
        }
    }

    public void CheckTile(TileClass tile, int x, int y, bool isNaturallyPlaced)
    {
        if (x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {
            if (!worldTiles.Contains(new Vector2Int(x, y)))
            {
                RemoveLightSource(x, y);
                //place the tile regardless
                PlaceTile(tile, x, y, isNaturallyPlaced);
            }
            else
            {
                if (world_ForegroundTiles[x, y].inBackground)
                {
                    //overwrite existing tile
                    RemoveLightSource(x, y);
                    //RemoveTile(x, y);
                    PlaceTile(tile, x, y, isNaturallyPlaced);
                }
            }
        }
    }

    public void PlaceTile(TileClass tile, int x, int y, bool isNaturallyPlaced)
    {
        if (x >= 0 && x <= worldSize && y >= 0 && y <= worldSize)
        {            
            GameObject newTile = new GameObject();

            int chunkCoord = Mathf.RoundToInt(Mathf.Round(x / chunkSize) * chunkSize);
            chunkCoord /= chunkSize;

            newTile.transform.parent = worldChunks[chunkCoord].transform;

            newTile.AddComponent<SpriteRenderer>();

            int spriteIndex = Random.Range(0, tile.tileSprites.Length);
            newTile.GetComponent<SpriteRenderer>().sprite = tile.tileSprites[spriteIndex];

            worldTilesMap.SetPixel(x, y, Color.black);
            if (tile.inBackground)
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = tileSortingOrderInBackground;

                if (tile.name.ToLower().Contains("wall"))
                {
                    newTile.GetComponent<SpriteRenderer>().color = new Color(0.6f, 0.6f, 0.6f);
                }
                else
                {
                    worldTilesMap.SetPixel(x, y, Color.white);
                }
            }
            else
            {
                newTile.GetComponent<SpriteRenderer>().sortingOrder = tileSortingOrder;
                newTile.AddComponent<BoxCollider2D>();
                newTile.GetComponent<BoxCollider2D>().size = Vector2.one;
                newTile.tag = "Ground";
            }

            newTile.name = tile.tileSprites[0].name;
            newTile.transform.position = new Vector2(x + 0.5f, y + 0.5f);

            TileClass newTileClass = TileClass.CreateInstance(tile, isNaturallyPlaced);
            worldTiles.Add(newTile.transform.position - (Vector3.one * 0.5f));
            worldTileObjects.Add(newTile);
            //worldTileClasses.Add(newTileClass);
            AddTileToWorld(x, y, newTileClass);
        }
    }

    private void AddTileToWorld(int x, int y, TileClass tile)
    {
        if (tile.inBackground)
        {
            world_BackgroundTiles[x, y] = tile;
        }
        else
        {
            world_ForegroundTiles[x, y] = tile;
        }
    }

    private void RemoveTileFromWorld(int x, int y)
    {
        if (world_ForegroundTiles[x, y] != null)
        {
            world_ForegroundTiles[x, y] = null;
        }
        else if (world_BackgroundTiles[x, y] != null)
        {
            world_BackgroundTiles[x, y] = null;
        }
    }

    private TileClass GetTileFromWorld(int x, int y)
    {
        if (world_ForegroundTiles[x, y] != null)
        {
            return world_ForegroundTiles[x, y];
        }
        else if (world_BackgroundTiles[x, y] != null)
        {
            return world_BackgroundTiles[x, y];
        }

        return null;
    }

    private void LightBlock(int x, int y, float intensity, int iteration)
    {
        if (iteration < lightRadius)
        {
            worldTilesMap.SetPixel(x, y, Color.white * intensity);

            for (int nx = x - 1; nx < x + 2; nx++)
            {
                for (int ny = y - 1; ny < y + 2; ny++)
                {
                    if (nx != x || ny != y)
                    {
                        float dist = Vector2.Distance(new Vector2(x, y), new Vector2(nx, ny));
                        float targetIntensity = Mathf.Pow(0.7f, dist) * intensity;
                        if (worldTilesMap.GetPixel(nx, ny) != null)
                        {
                            if(worldTilesMap.GetPixel(nx, ny).r < targetIntensity)
                            {
                                LightBlock(nx, ny, targetIntensity, iteration + 1);
                            }
                        }
                    }
                }
            }

            worldTilesMap.Apply();
        }
    }

    private void RemoveLightSource(int x, int y)
    {
        unlitBlocks.Clear();
        UnLightBlock(x, y, x, y);

        List<Vector2Int> toRelight = new List<Vector2Int>();
        foreach (Vector2Int block in unlitBlocks)
        {
            for (int nx = block.x - 1; nx < block.x + 2; nx++)
            {
                for(int ny = block.y - 1; ny < block.y + 2; ny++)
                {
                    if (worldTilesMap.GetPixel(nx, ny) != null)
                    {
                        if(worldTilesMap.GetPixel(nx, ny).r > worldTilesMap.GetPixel(block.x, block.y).r)
                        {
                            if (!toRelight.Contains(new Vector2Int(nx, ny)))
                            {
                                toRelight.Add(new Vector2Int(nx, ny));
                            }
                        }
                    }
                }
            }
        }

        foreach (Vector2Int source in toRelight)
        {
            LightBlock(source.x, source.y, worldTilesMap.GetPixel(source.x, source.y).r, 0);
        }

        worldTilesMap.Apply();
    }

    private void UnLightBlock(int x, int y, int ix, int iy)
    {
        if (Mathf.Abs(x - ix) >= lightRadius || Mathf.Abs(y - iy) >= lightRadius || unlitBlocks.Contains(new Vector2Int(x, y)))
            return;

        for (int nx = x - 1; nx < x + 2; nx++)
        {
            for (int ny = y - 1; ny < y + 2; ny++)
            {
                if(nx != x || ny != y)
                {
                    if(worldTilesMap.GetPixel(nx, ny) != null)
                    {
                        if (worldTilesMap.GetPixel(nx, ny).r < worldTilesMap.GetPixel(x, y).r)
                        {
                            UnLightBlock(nx, ny, ix, iy);
                        }
                    }
                }
            }
        }

        worldTilesMap.SetPixel(x, y, Color.black);
        unlitBlocks.Add(new Vector2Int(x, y));
    }
}
