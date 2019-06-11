using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text moneyAmount;
    public Text interactionMsg;

    void Awake() {
        Setup();
    }

    void Setup() {
        try{
            moneyAmount.text = "Money: ";
            interactionMsg.text = ""; 
        }
        catch{
            Debug.LogWarning("Assign UI texts!");
        }
    }
}
