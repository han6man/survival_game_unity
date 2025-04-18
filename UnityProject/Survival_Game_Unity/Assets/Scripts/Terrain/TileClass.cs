using UnityEngine;
/// <summary>
/// Tile Data For Game World Generation Class
/// </summary>
[CreateAssetMenu(fileName = "newtileclass", menuName = "Tile Class")]
public class TileClass : ScriptableObject
{
    public string tileName;
    public TileClass wallVariant;
    public Sprite[] tileSprites;
    public bool inBackground = false;
    public Sprite tileDrop;
    public bool naturallyPlaced = true;
    public ItemClass inventoryItem;

    public static TileClass CreateInstance(TileClass tile, bool isNaturallyPlaced)
    {
        var thisTile = ScriptableObject.CreateInstance<TileClass>();
        thisTile.Init(tile, isNaturallyPlaced);

        return thisTile;
    }

    private void Init(TileClass tile, bool isNaturallyPlaced)
    {
        tileName = tile.name;
        wallVariant = tile.wallVariant;
        tileSprites = tile.tileSprites;
        inBackground = tile.inBackground;
        tileDrop = tile.tileDrop;
        inventoryItem = tile.inventoryItem;
        naturallyPlaced = isNaturallyPlaced;
    }
}
