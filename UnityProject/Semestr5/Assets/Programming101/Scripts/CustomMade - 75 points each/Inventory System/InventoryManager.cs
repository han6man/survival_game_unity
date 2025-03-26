using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Programming101
{
    public class InventoryManager : MonoBehaviour
    {
        public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();

        public Inventory toolbar;
        public int toolbarSlotCount;

        private void Awake()
        {
            toolbar = new Inventory(toolbarSlotCount);

            inventoryByName.Add("Toolbar", toolbar);
        }

        public void Add(string inventoryName, Item item)
        {
            if (inventoryByName.ContainsKey(inventoryName))
            {
                inventoryByName[inventoryName].Add(item);
            }
        }
    }
}
