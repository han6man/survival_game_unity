using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Programming101
{
    [System.Serializable]
    public class Inventory
    {
        public List<Slot> slots = new List<Slot>();
        public Slot selectedSlot = null;

        public Inventory(int numSlots)
        {
            for (int i = 0; i < numSlots; i++)
            {
                Slot slot = new Slot();
                slots.Add(slot);
            }
        }

        public void Add(Item item)
        {
            foreach (Slot slot in slots)
            {
                if (slot.itemName == item.data.itemName && slot.CanAddItem(item.data.itemName))
                {
                    slot.AddItem(item);
                    return;
                }
            }

            foreach (Slot slot in slots)
            {
                if (slot.itemName == "")
                {
                    slot.AddItem(item);
                    return;
                }
            }
        }

        public void Remove(int index)
        {
            slots[index].RemoveItem();
        }

        public void Remove(int index, int numToRemove)
        {
            if (slots[index].count >= numToRemove)
            {
                for (int i = 0; i < numToRemove; i++)
                {
                    Remove(index);
                }
            }
        }

        public void MoveSlot(int fromIndex, int toIndex, Inventory toInventory, int numToMove = 1)
        {
            Slot fromSlot = slots[fromIndex];
            Slot toSlot = toInventory.slots[toIndex];

            if (toSlot.IsEmpty || toSlot.CanAddItem(fromSlot.itemName))
            {
                for (int i = 0; i < numToMove; i++)
                {
                    toSlot.AddItem(fromSlot.itemName, fromSlot.icon, fromSlot.maxAllowed);
                    fromSlot.RemoveItem();
                }
            }
        }

        public void SelectSlot(int index)
        {
            if (slots != null && slots.Count > 0)
            {
                selectedSlot = slots[index];
            }
        }
    }
}
