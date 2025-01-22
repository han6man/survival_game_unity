using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class OreClass
{
    public string oreName;
    [Range(0, 1)]
    public float rarity;
    [Range(0, 1)]
    public float size;
    public int maxSpawnHeight;
    public Texture2D spreadTexture;
}
