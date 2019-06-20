using UnityEngine;

[System.Serializable]
public class Slot {

    [SerializeField] private Item item;
    [SerializeField] private SlotGUI slotGUI;

    /// <summary>Returns the item that the slot holds. Returns NULL if there's no item.</summary>
    public Item Item {
        get {
            return item;
        }
        set {
            item = value;
        }
    }

    public void SetItem(Item newItem) {
        
    }
}
