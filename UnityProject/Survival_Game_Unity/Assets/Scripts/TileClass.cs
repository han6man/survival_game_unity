using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public TileClass wallVariant;
    public Sprite[] tileSprites;
    public bool inBackground = false;
    public bool tileDrop = true;
    public bool naturallyPlaced = true;

    public float lightLevel = 1;
}
