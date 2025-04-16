using UnityEngine;

[CreateAssetMenu(fileName = "new Misc Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
    //data specific to Misc class
    public override ItemClass GetItem()
    {
        return this;
    }

    public override ToolClass GetTool()
    {
        return null;
    }

    public override MiscClass GetMisc()
    {
        return this;
    }

    public override ConsumableClass GetConsumable()
    {
        return null;
    }
}
