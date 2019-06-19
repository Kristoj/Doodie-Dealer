using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private float money;

    /// <summary>
    /// Add money to player's wallet
    /// </summary>
    /// <param name="amount">Amount to add</param>
    public void AddMoney(float amount){
        money += amount;
        GameManager.LocalPlayer.Player_UI.moneyAmount.text = "Money: " + money + "$";
    }

    /// <summary>
    /// Remove money from player's wallet
    /// </summary>
    /// <param name="amount">Amount to remove</param>
    public void RemoveMoney(float amount){
        money -= amount;
        GameManager.LocalPlayer.Player_UI.moneyAmount.text = "Money: " + money;
    }
}
