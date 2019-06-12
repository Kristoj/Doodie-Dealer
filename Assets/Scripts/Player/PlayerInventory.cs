using UnityEngine;

public class PlayerInventory : Inventory {

    void Update() {
        CheckInput();
    }

    void CheckInput() {
        if (Input.GetKeyDown(KeyCode.G)) {
            DropShit();
        }
    }

    void DropShit() {
        Slot dropSlot = FindItem(Database.Instance.GetItem(ItemName.Doodie_Normal));
        GameManager.LocalPlayer.Player_Inventory.DropItem(dropSlot, GameManager.LocalPlayer.Player_Camera.head.transform.position);
    }

}
