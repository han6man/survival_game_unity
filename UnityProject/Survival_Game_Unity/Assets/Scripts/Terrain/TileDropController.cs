using System.Collections;
using UnityEngine;
/// <summary>
/// Droped Items Management Class
/// </summary>
public class TileDropController : MonoBehaviour
{
    private ItemClass inventoryItem;
    private InventoryManager inventory;

    public void SetInventoryItem(ItemClass inventoryItem)
    { this.inventoryItem = inventoryItem; }

    public void SetInventory(InventoryManager inventory)
    { this.inventory = inventory; }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            //adds to the player inventory
            if (inventoryItem != null)
                inventory.Add(inventoryItem, 1);

            Destroy(this.gameObject);
        }        
    }
}
