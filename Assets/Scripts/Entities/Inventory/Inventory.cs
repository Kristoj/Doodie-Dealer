using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    [SerializeField] private uint slotCount = 16;
    [SerializeField] private List<Slot> slots = new List<Slot>();
    
    void Awake() {
        SetupSlots();
    }
    
    // Instantiate slots 
    void SetupSlots() {
        for (uint i = 0; i < slotCount; i++) {
            slots.Add(new Slot());
        }
    }

    /// <summary>
    /// Adds a item to the inventory if there's space for it.
    /// </summary>
    /// <param name="itemToAdd">Item that will be added</param>
    public void AddItem(Item itemToAdd) {
        for (int i = 0; i < slotCount; i++) {
            // Check if we can stack the item to be added to the target slot item
            if (slots[i].Item != null && slots[i].Item.itemId == itemToAdd.itemId) {
                slots[i].Item.stackSize++;          // Increment the item stack size
                itemToAdd.DestroyItem();            // Destroy item that we just added to the stack
                break;
            }
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
    /// Clears the given slot and destroys the slot item.
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
