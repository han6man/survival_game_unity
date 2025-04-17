using UnityEngine;

[CreateAssetMenu(fileName = "new Misc Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass
{
    //data specific to Misc class

    public override void Use(PlayerController caller)
    {
        //base.Use(caller);
    }

    public override MiscClass GetMisc()
    {
        return this;
    }
}
