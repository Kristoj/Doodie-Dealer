﻿using UnityEngine;
using Doodie.NPC;

public class FoodDeposit : MonoBehaviour {

    [SerializeField] private float maxFoodMass = 100;
    [SerializeField] private float foodMass;

    void Awake() {
        GameManager.RegisterFoodDeposit(this);
        foodMass = maxFoodMass;
    }

    public float TakeFood(Dooker dookerToFeed) {
        // Get the maximum amount of foodmass that the dooker can eat
        float foodAmount = dookerToFeed.DookerStats.maxFoodMass - dookerToFeed.DookerStats.foodMass;
        foodAmount = Mathf.Clamp(foodAmount, 0, foodMass);
        foodMass -= foodAmount;
        return foodAmount;
    }

}
