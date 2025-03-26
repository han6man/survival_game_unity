using UnityEngine;

namespace Programming101
{
    public class Player : MonoBehaviour
    {
        public InventoryManager inventoryManager;

        private void Awake()
        {
            inventoryManager = GetComponent<InventoryManager>();
        }
    }
}
