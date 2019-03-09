using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {
    
    public uint itemId;
    public uint stackSize = 1;
    public string itemName = "CHANGEME";

    void Awake(){
        if (itemName == "CHANGEME"){
            itemName = gameObject.name;
        }
    }
    public override void InteractionStart() {
        base.InteractionStart();
        GameManager.LocalPlayer.GetComponent<Inventory>().AddItem(this);
    }

    public override void FocusStart() {
        base.OnFocusStarted();
        //GameManager.LocalPlayer.Player_UI.interactionMsg.text = "Pickup " + itemName;
    }

    public void SetActive(bool state) {
        gameObject.SetActive(state);
    }

    public void DestroyItem() {
        // If player is focusing on this item we need to call
        // OnFocusLeave before destroying
        if (GameManager.LocalPlayer.Player_Interaction.TargetInteractable == this) {
            FocusEnd();
        }
        Destroy(gameObject);
    }

}
