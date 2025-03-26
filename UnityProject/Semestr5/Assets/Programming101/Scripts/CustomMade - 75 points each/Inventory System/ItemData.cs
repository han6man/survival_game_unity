using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Programming101
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
    public class ItemData : ScriptableObject
    {
        public string itemName = "Item Name";
        public Sprite icon;

    }
}
