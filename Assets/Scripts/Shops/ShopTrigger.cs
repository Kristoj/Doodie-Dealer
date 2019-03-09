using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : Interactable {
    public List<Item> allowedItems;
    public float shopMultiplier = 1;
    public string interactionMessage = "Sell items";

    public override void FocusStart(){
        float value = GetTotalValue();
        GameManager.LocalPlayer.Player_UI.interactionMsg.text = interactionMessage + " " + value + "$";
    }

    /// <summary>
    /// Gets player's inventory slots
    /// </summary>
    public override void InteractionStart(){
        List<Slot> invSlots;
        Player player = GameManager.LocalPlayer;
        Inventory inv = player.Player_Inventory;
        invSlots = inv.GetAllSlots();
        SlotCheck(invSlots);
    }

    // Checks inventory and sells wanted items
    void SlotCheck(List<Slot> invSlots){
        for(int i = 0; i < invSlots.Count; i++){
            bool isValid;
            isValid = SlotCheck(invSlots[i]);
            if (isValid){
                float value = invSlots[i].Item.GetComponent<Saleable>().SellValue * invSlots[i].Item.stackSize;
                GameManager.LocalPlayer.Player_Inventory.ClearSlot(invSlots[i]);
                GameManager.LocalPlayer.Player_Wallet.AddMoney(value * shopMultiplier);
                GameManager.LocalPlayer.Player_UI.interactionMsg.text = "";
            }
        }
    }

    // Checks if a slot contains a valid item
    bool SlotCheck(Slot s){
        for(int i = 0; i < allowedItems.Count; i++){
            if (s.Item == null) break;
            if (s.Item.itemId == allowedItems[i].itemId){
                return true;
            }
        }
        return false;
    }

    // Gets the total value of the items that will be sold
    float GetTotalValue(){
        float totalValue = 0;
        List<Slot> invSlots;
        Player player = GameManager.LocalPlayer;
        Inventory inv = player.Player_Inventory;
        invSlots = inv.GetAllSlots();

        for(int i = 0; i < invSlots.Count; i++){
        bool isValid;
            isValid = SlotCheck(invSlots[i]);
            if (isValid){
                float value = invSlots[i].Item.GetComponent<Saleable>().SellValue * invSlots[i].Item.stackSize;
                totalValue += value;
            }
        }
        return totalValue;
    }
}
