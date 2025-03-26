using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Programming101
{
    [RequireComponent(typeof(Item))]
    public class Collectable : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Player player = collision.gameObject.GetComponent<Player>();
            Player player = collision.GetComponent<Player>();

            if (player)
            {
                Item item = GetComponent<Item>();
                if (item != null)
                {
                    player.inventoryManager.Add("Backpack", item);
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
