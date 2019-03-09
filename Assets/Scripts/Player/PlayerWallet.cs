using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private float CurMoney { get; set; }

    /// <summary>
    /// Add money to player's wallet
    /// </summary>
    /// <param name="amount">Amount to add</param>
    public void AddMoney(float amount){
        CurMoney += amount;
        GameManager.LocalPlayer.Player_UI.moneyAmount.text = "Money: " + CurMoney + "$";
    }

    /// <summary>
    /// Remove money from player's wallet
    /// </summary>
    /// <param name="amount">Amount to remove</param>
    public void RemoveMoney(float amount){
        CurMoney -= amount;
        GameManager.LocalPlayer.Player_UI.moneyAmount.text = "Money: " + CurMoney;
    }
}
