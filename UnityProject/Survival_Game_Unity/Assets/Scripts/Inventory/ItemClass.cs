using UnityEngine;
/// <summary>
/// Items Parent Class
/// </summary>
public class ItemClass : ScriptableObject
{
    public enum ItemType
    {
        None,
        Block,
        Tool
    }

    public enum ToolType
    {
        None,
        Weapon,
        Pickaxe,
        Hammer,
        Axe,
        Unbreakable
    }

    [Header("Item")]//data shared across every item
    public ItemType itemType = ItemType.None;
    public ToolType toolType = ToolType.None;
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;
    public TileClass tile;

    public virtual void Use(PlayerController caller)
    {
        Debug.Log("Used Item");
    }

    public virtual ItemClass GetItem() {  return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
}
