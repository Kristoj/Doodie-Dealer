using UnityEngine;
using System.Collections.Generic;

public class Checkout : Interactable {

    public override void InteractionStart() {
        base.InteractionStart();
        CalculateTransaction();
    }

    void CalculateTransaction() {
        List<Slot> slots = GameManager.LocalPlayer.Player_Inventory.GetAllSlots();
        List<Sellable> sellables = new List<Sellable>();
        float totalPrice = 0;
        for (int i = 0; i < slots.Count; i++) {
            Sellable s = slots[i].Item as Sellable;
            if ((slots[i].Item as Sellable) != null) {
                sellables.Add(s);
                totalPrice += s.price * s.stackSize;
            }
        }
        Debug.Log("Price : " + totalPrice);
    }



}
