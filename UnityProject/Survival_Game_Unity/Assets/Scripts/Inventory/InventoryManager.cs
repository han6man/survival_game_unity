using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;
    [SerializeField] private GameObject hotbarSelector;
    [SerializeField] private GameObject inventorySlotHolder;
    [SerializeField] private GameObject hotbarSlotHolder;
    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;

    [SerializeField] private GameObject[] inventorySlots;
    [SerializeField] private GameObject[] hotbarSlots;

    [SerializeField] private SlotClass[] inventoryItems;

    [SerializeField] private int selectedSlotIndex = 0;
    public ItemClass selectedItem;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    private bool isMovingItem;

    [SerializeField] private SlotClass[] startingItems;

    private void Start()
    {
        inventorySlots = new GameObject[inventorySlotHolder.transform.childCount];
        inventoryItems = new SlotClass[inventorySlots.Length];

        hotbarSlots = new GameObject[hotbarSlotHolder.transform.childCount];

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i] = new SlotClass();
        }

        //set all the slots
        for (int i = 0; i < inventorySlotHolder.transform.childCount; i++)
        {
            inventorySlots[i] = inventorySlotHolder.transform.GetChild(i).gameObject;
        }
        for (int i = 0; i < hotbarSlotHolder.transform.childCount; i++)
        {
            hotbarSlots[i] = hotbarSlotHolder.transform.GetChild(i).gameObject;
        }

        for (int i = 0; i < startingItems.Length; i++)
        {
            inventoryItems[i] = startingItems[i];
        }

        RefreshUI();

        Add(itemToAdd, 1);
        
        Remove(itemToRemove);
    }

    private void Update()
    {
        itemCursor.SetActive(isMovingItem);
        itemCursor.transform.position = Input.mousePosition;
        if (isMovingItem)
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemIcon;

        if (Input.GetMouseButtonDown(0)) //we left clicked
        {
            //find the closest slot (the slot we clicked on)
            if (isMovingItem)
            {
                //end item move
                EndItemMove();
            }
            else
                BeginItemMove();
        }
        else if (Input.GetMouseButtonDown(1)) //we right clicked
        {
            //find the closest slot (the slot we clicked on)
            if (isMovingItem)
            {
                //end item move
                EndItemMove_Single();
            }
            else
                BeginItemMove_Half();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0) //scrolling up
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex + 1, 0, hotbarSlots.Length - 1);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0) //scrolling down
        {
            selectedSlotIndex = Mathf.Clamp(selectedSlotIndex - 1, 0, hotbarSlots.Length - 1);
        }
        hotbarSelector.transform.position = hotbarSlots[selectedSlotIndex].transform.position;
        selectedItem = inventoryItems[selectedSlotIndex + (inventorySlots.Length - hotbarSlots.Length)].GetItem();
    }

    #region Inventory Utils
    public void RefreshUI()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            try
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = inventoryItems[i].GetItem().itemIcon;
                if (inventoryItems[i].GetItem().isStackable)
                {
                    inventorySlots[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = inventoryItems[i].GetQuantity().ToString();
                }
                else
                    inventorySlots[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
            }
            catch
            {
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                inventorySlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                inventorySlots[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
            }
        }

        RefreshHotbar();
    }

    private void RefreshHotbar()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            int itemIndex = i + (inventorySlots.Length - hotbarSlots.Length);
            try
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = inventoryItems[itemIndex].GetItem().itemIcon;
                if (inventoryItems[itemIndex].GetItem().isStackable)
                    hotbarSlots[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = inventoryItems[itemIndex].GetQuantity().ToString();
                else
                    hotbarSlots[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
            }
            catch
            {
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                hotbarSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                hotbarSlots[i].transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = "";
            }
        }
    }

    public bool Add(ItemClass item, int quantity)
    {
        //check if inventory contains item
        SlotClass slot = Contains(item);
        if (slot != null && slot.GetItem().isStackable)
            slot.AddQuantity(quantity);
        else
        {
            for (int i = 0; i < inventoryItems.Length; i++)
            {
                if (inventoryItems[i].GetItem() == null) //this is an empty slot
                {
                    inventoryItems[i].AddItem(item, quantity);
                    break;
                }
            }
        }

        RefreshUI();
        return true;
    }

    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item);
        if (temp != null)
        {
            if (temp.GetQuantity() > 1)
                temp.SubQuantity(1);
            else
            {
                int slotToRemoveIndex = 0;

                for (int i = 0; i < inventoryItems.Length; i++)
                {
                    if (inventoryItems[i].GetItem() == item)
                    {
                        slotToRemoveIndex = i;
                        break;
                    }
                }

                inventoryItems[slotToRemoveIndex].Clear();
            }
        }
        else
        {
            return false;
        }

        RefreshUI();
        return true;
    }

    public void UseSelected()
    {
        inventoryItems[selectedSlotIndex + (inventorySlots.Length - hotbarSlots.Length)].SubQuantity(1);
        RefreshUI();
    }

    private SlotClass Contains(ItemClass item)
    {
        for (int i = 0; i < inventoryItems.Length; i++)
        {
            if (inventoryItems[i].GetItem() == item)
                return inventoryItems[i];
        }

        return null;
    }
    #endregion Inventory Utils

    #region Moving Stuff
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        { return false; } //there is not item to move!

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool BeginItemMove_Half()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null || originalSlot.GetItem() == null)
        { return false; } //there is not item to move!

        movingSlot = new SlotClass(originalSlot.GetItem(), Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        originalSlot.SubQuantity(Mathf.CeilToInt(originalSlot.GetQuantity() / 2f));
        if (originalSlot.GetQuantity() < 1)
            originalSlot.Clear();

        isMovingItem = true;
        RefreshUI();
        return true;
    }

    private bool EndItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            //droping out where
            Add(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem()) //they're the same item (they should stack)
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                        movingSlot.Clear();
                    }
                    else
                        return false;
                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    RefreshUI();
                    return true;
                }
            }
            else //place item as usual
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }

        isMovingItem = false;
        RefreshUI();
        return true;
    }

    private bool EndItemMove_Single()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
            return false; //there is not item to move!
        if (originalSlot.GetItem() != null && originalSlot.GetItem() != movingSlot.GetItem())
            return false;

        if (originalSlot.GetItem() != null && originalSlot.GetItem() == movingSlot.GetItem())
        {
            if (movingSlot.GetItem().isStackable)
            {
                originalSlot.AddQuantity(1);
                movingSlot.SubQuantity(1);
            }
            else
                return false;
        }
        else
        {
            originalSlot.AddItem(movingSlot.GetItem(), 1);
            movingSlot.SubQuantity(1);
        }

        if (movingSlot.GetQuantity() < 1)
        {
            isMovingItem = false;
            movingSlot.Clear();
        }
        else
        {
            isMovingItem = true;
        }

        RefreshUI();
        return true;
    }

    private SlotClass GetClosestSlot()
    {
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (Vector2.Distance(inventorySlots[i].transform.position, Input.mousePosition) <= 32)
                return inventoryItems[i];
        }

        return null;
    }
    #endregion Moving Stuff
}
