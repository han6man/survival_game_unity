using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public Sprite[] tileSprites;
    public bool inBackground = false;
    public bool tileDrop = true;
}
