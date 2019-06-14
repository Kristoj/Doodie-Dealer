using UnityEngine;
using System.Collections.Generic;
using Doodie.NPC;

public static class GameManager {

    private static Player _localPlayer;
    public static Player LocalPlayer {
        get {
            if (_localPlayer == null) {
                Debug.LogError("Vili pls. Could't find local player reference!");
            }
            return _localPlayer;
        }
        set {
            _localPlayer = value;
        }
    }

    // List of all food deposits in the level
    private static List<FoodDeposit> foodDeposits = new List<FoodDeposit>();

    public static void RegisterFoodDeposit(FoodDeposit foodDepositToRegister) {
        if (foodDepositToRegister != null) {
            foodDeposits.Add(foodDepositToRegister);
        }
    }

    
    public static FoodDeposit GetClosestFoodDeposit(Dooker dooker) {
        FoodDeposit closestDeposit = null;
        float lastDistance = 999;

        // Loop trough each food deposit and get the shortest distance from them
        for (int i = 0; i < foodDeposits.Count; i++) {
            FoodDeposit depo = foodDeposits[i];
            float dst = Vector3.Distance(dooker.transform.position, depo.transform.position);

            // Check if the current target depo is closer than the last one
            if (dst < lastDistance) {
                lastDistance = dst;
                closestDeposit = depo;
            }
        }
        return closestDeposit;
    }
    
}
