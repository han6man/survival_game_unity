using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class BiomeClass
{
    public string biomeName;
    public Color biomeCol;

    public TileAtlas tileAtlas;

    [Header("Noise Settings")]
    public float terrainFreq = 0.04f;
    public float caveFreq = 0.08f;
    public Texture2D caveNoiseTexture;

    [Header("Generation Settings")]
    public bool generateCaves = true;
    public int dirtLayerHeight = 5;
    public float surfaceValue = 0.25f;
    public float heightMultiplier = 25.0f;

    [Header("Trees")]
    public int treeChance = 15;
    public int minTreeHeight = 3;
    public int maxTreeHeight = 6;

    [Header("Addons")]
    public int tallGrassChance = 2;

    [Header("Ore Settings")]
    public OreClass[] ores;
}
