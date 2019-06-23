#pragma warning disable 0649
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    [Header("Slots")]
    [SerializeField] private Transform inventoryPanel;
    [SerializeField] private SlotGUI slotPrefab;
    [SerializeField] private uint slotCount = 16;
    [SerializeField] private float slotSize = .1f;
    [SerializeField] private float slotGap = .01f;
    [SerializeField] private float cornerPadding = .05f;
    [SerializeField] private Vector2 slotArea = new Vector2(500, 800);
    [SerializeField] private List<Slot> slots = new List<Slot>();

    [Header("Debug")]
    [SerializeField] private bool canRefreshSlots;
    
    void Awake() {
        SetupSlots();
        StartCoroutine(UpdateSlotsCoroutine());
    }

    IEnumerator UpdateSlotsCoroutine() {
        while (true) {
            if (canRefreshSlots)
                UpdateSlots();
            yield return new WaitForSeconds(.1f);
        }
    }

    
    // Instantiate slots 
    void SetupSlots() {
        for (uint i = 0; i < slotCount; i++) {
            slots.Add(new Slot());
        }

        UpdateSlots();
    }

    void UpdateSlots() {

        // Destroy existing slots if there are any
        for (int i = 0; i < inventoryPanel.childCount; i++) {
            Destroy(inventoryPanel.GetChild(i).gameObject);
        }

        int slotsPerRow = (int)(slotArea.x / (slotSize + slotGap) - cornerPadding * 2);
        int columnCount = Mathf.CeilToInt(((float)slotCount / (float)slotsPerRow));
        float slotOffset = (slotArea.x - (cornerPadding * 2)) / slotsPerRow;

        Debug.Log("Slots per row: " + slotsPerRow);
        Debug.Log("Columns: " + columnCount);
        try {
            for (int y = 0; y < columnCount; y++) {
                for (int x = 0; x < slotsPerRow; x++) {
                    float xGap = x > 0 ? slotGap : 0;
                    float yGap = y > 0 ? slotGap : 0;
                    Vector3 slotPos = new Vector3((slotOffset * x) + xGap + cornerPadding - (slotArea.x * .5f - slotSize * .5f), 
                                                (-slotOffset * y) + yGap - cornerPadding + (slotArea.y * .5f - slotSize * .5f), 
                                                0);
                    SlotGUI clone = Instantiate(slotPrefab, Vector3.zero, Quaternion.Euler(inventoryPanel.transform.forward), inventoryPanel.transform);
                    clone.transform.name = "Slot" + y * slotsPerRow + x;
                    clone.transform.localPosition = slotPos;

                    // Break from the loop when we have instantiated every slot
                    if (y * slotsPerRow + x == slotCount - 1) {
                        break;
                    }
                }
            }
        }
        catch (System.Exception e) {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Adds a item to the inventory if there's space for it.
    /// </summary>
    /// <param name="itemToAdd">Item that will be added</param>
    public void AddItem(Item itemToAdd) {
        for (int i = 0; i < slotCount; i++) {
            // Check if we can stack the item to any slot
            if (slots[i].Item != null && slots[i].Item.itemId == itemToAdd.itemId) {
                slots[i].Item.stackSize++;          // Increment the item stack size
                itemToAdd.DestroyItem();            // Destroy item that we just added to the stack
                break;
            }
            // Otherwise add item to empty slot
            else if (slots[i].Item == null) {
                slots[i].Item = itemToAdd;          // Add item to the slot
                itemToAdd.SetActive(false);         // Set item active state to false
                break;
            }
        }
    }

    /// <summary>
    /// Tries to find a slot that contains the given item.
    /// </summary>
    /// <param name="itemToFind">Item that will be searched.</param>
    /// <returns>Slot that contains the given item.null NULL</returns>
    public Slot FindItem(Item itemToFind) {
        if (itemToFind != null) {
            for (int i = 0; i < slotCount; i++) {
                if (slots[i].Item != null && slots[i].Item.itemId == itemToFind.itemId) {
                    return slots[i];
                }
            }
        }
        return null;
    }

    /// <summary>
    /// Drops the item in the given slot at the given position.
    /// </summary>
    /// <param name="dropSlot">Slot that contains the item to be dropped.</param>
    /// <param name="dropPos">Position that the item will be dropped from.</param>
    public void DropItem(Slot dropSlot, Vector3 dropPos) {
        if (dropSlot != null && dropSlot.Item != null) {
            dropSlot.Item.transform.position = dropPos;     // Orientate the item
            dropSlot.Item.SetActive(true);                  // Enable the item gameobject
            dropSlot.Item = null;                           // Set the slot item to NULL
        }
    }

    /// <summary>
    /// Clears the given slot and destroys the in it.
    /// </summary>
    /// <param name="slotToClear">Slot to be cleared.</param>
    public void ClearSlot(Slot slotToClear) {
        if (slotToClear.Item != null) {
            slotToClear.Item.DestroyItem();
            slotToClear.Item = null;
        }
    }

    /// <summary>
    /// Returns list of all slots in the invetory
    /// </summary>
    /// <returns></returns>
    public List<Slot> GetAllSlots() {
        return slots;
    }
}
