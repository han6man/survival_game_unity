using UnityEngine;
/// <summary>
/// Items Parent Class
/// </summary>
public class ItemClass : ScriptableObject
{
    [Header("Item")]//data shared across every item
    public string itemName;
    public Sprite itemIcon;
    public bool isStackable = true;

    public virtual void Use(PlayerController caller)
    {
        Debug.Log("Used Item");
    }

    public virtual ItemClass GetItem() {  return this; }
    public virtual ToolClass GetTool() { return null; }
    public virtual MiscClass GetMisc() { return null; }
    public virtual ConsumableClass GetConsumable() { return null; }
}
