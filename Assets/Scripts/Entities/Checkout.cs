using UnityEngine;
using System.Collections.Generic;

public class Checkout : Interactable {

    public override void InteractionStart() {
        base.InteractionStart();
        CalculateTransaction();
    }

    void CalculateTransaction() {
        // Get every slot that ha a sellable item in the invetory and
        // add it to the total price
        List<Slot> slots = GameManager.LocalPlayer.Player_Inventory.GetAllSlots();
        List<Slot> sellables = new List<Slot>();
        float totalPrice = 0;
        for (int i = 0; i < slots.Count; i++) {
            Sellable s = slots[i].Item as Sellable;
            if ((slots[i].Item as Sellable) != null) {
                sellables.Add(slots[i]);
                totalPrice += s.price * s.stackSize;
            }
        }

        // Add the total amount of money to the players wallet and clear the sold item slots
        GameManager.LocalPlayer.Player_Wallet.AddMoney(totalPrice);
        for (int i = 0; i < sellables.Count; i++) {
            GameManager.LocalPlayer.Player_Inventory.ClearSlot(sellables[i]);
        }
    }



}
