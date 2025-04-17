using UnityEngine;

[CreateAssetMenu(fileName = "new Consumable Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]//data specific to Consumable class
    public float healthAdded;

    public override void Use(PlayerController caller)
    {
        base.Use(caller);
        Debug.Log("Eat Consumable");
        caller.inventory.UseSelected();
    }

    public override ConsumableClass GetConsumable()
    {
        return this;
    }
}
