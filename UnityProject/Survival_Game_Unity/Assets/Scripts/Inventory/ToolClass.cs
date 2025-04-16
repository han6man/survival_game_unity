using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass
{
    [Header("Tool")]//data specific to Tool class
    public ToolType toolType;

    public enum ToolType
    {
        weapon,
        pickaxe,
        hammer,
        axe
    }

    public override ItemClass GetItem()
    {
        return this;
    }

    public override ToolClass GetTool()
    {
        return this;
    }

    public override MiscClass GetMisc()
    {
        return null;
    }

    public override ConsumableClass GetConsumable()
    {
        return null;
    }
}
