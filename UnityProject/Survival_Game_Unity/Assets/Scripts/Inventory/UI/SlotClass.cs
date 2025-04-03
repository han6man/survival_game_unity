using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class SlotClass
{
    [SerializeField] private ItemClass item;
    [SerializeField] private int quantity;

    public SlotClass()
    {
        this.item = null;
        this.quantity = 0;
    }

    public SlotClass(ItemClass _item, int _quantity)
    {
        this.item = _item;
        this.quantity = _quantity;
    }

    public ItemClass GetItem() { return item; }
    public int GetQuantity() { return quantity; }
    public void AddQuantity(int _quantity) { this.quantity += _quantity; }
    public void SubQuantity(int _quantity) { this.quantity -= _quantity; }
}
