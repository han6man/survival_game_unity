using UnityEngine;
/// <summary>
/// Basic Class For Tools Items
/// </summary>
[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool/Tool")]
public class ToolClass : ItemClass
{

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        Debug.Log("Swing Tool");
    }

    public override ToolClass GetTool()
    {
        return this;
    }
}
