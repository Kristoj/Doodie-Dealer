#pragma warning disable 0649
using UnityEngine;

namespace Doodie.NPC {

    public class DookerStats : MonoBehaviour {

        [Header("Defecation Settings")]
        [Tooltip("The maximum amount of foodmass that the stomach can hold in milliliters")]
        public float maxFoodMass = 1500;
        public AnimationCurve defecationUrgeCurve;
        public float defecationUrgeMultiplier = 30f;
        [Tooltip("The amount of foodmass in the stomach in milliliters")]
        public float foodMass;
        [Tooltip("The amount of excrement in the intestines in milliliters.")]
        public float excrement;

        [Header("Digestion")]
        [SerializeField] private float digestionSpeed = 1;
        public float digestionEfficiency = 3f;

        [Header("Consumption")]
        [Tooltip("Percentage of nutrients in the body.")]
        public float nutrients = 100;
        public float nutrientDrainRate = 2f;
        [Tooltip("Percentage of dookers feeling of hunger.")]
        public float hunger = 0;
        public float hungerGrowRate = 2f;
        public AnimationCurve hungerGrowCurve;
        public AnimationCurve hungerUrgeCurve;
        public AnimationCurve foodDistanceCurve;

        void Start() {
            excrement = foodMass;
        }

        void Update() {
            Digestion();
            DrainNutrients();
            GrowHunger();
        }

        // Turns foodmass into excrement overtime
        void Digestion() {
            float digestionAmount = foodMass;
            digestionAmount = Mathf.Clamp(digestionAmount, 0, digestionSpeed);
            if (digestionAmount <= .05) {
                foodMass = 0;
                return;
            }

            RemoveFoodMass(digestionAmount * Time.deltaTime);
            AddExcrement(digestionAmount * Time.deltaTime);
            AddNutrients(digestionAmount * (digestionEfficiency / 100) * Time.deltaTime);
        }

        void DrainNutrients() {
            nutrients -= nutrientDrainRate * Time.deltaTime;
            nutrients = Mathf.Clamp(nutrients, 0, 100);
        }

        // Grow hunger according to how much nutrients dooker has in his body
        void GrowHunger() {
            if (foodMass <= 0) {
                hunger += hungerGrowCurve.Evaluate((100 - nutrients) / 100) * hungerGrowRate * Time.deltaTime;
                hunger = Mathf.Clamp(hunger, 0, 100);
            }
        }

        public void SetHunger(float amount) {
            hunger = amount;
            hunger = Mathf.Clamp(hunger, 0, 100);
        }

        public void AddNutrients(float amount) {
            nutrients += amount;
            nutrients = Mathf.Clamp(nutrients, 0, 100);
        }

        public void RemoveNutrients(float amount) {
            nutrients -= amount;
            nutrients = Mathf.Clamp(nutrients, 0, 100);
        }

        public void AddExcrement(float amount) {
            excrement += amount;
            excrement = Mathf.Clamp(excrement, 0, maxFoodMass);
        }

        public void RemoveExcrement(float amount) {
            excrement -= amount;
            excrement = Mathf.Clamp(excrement, 0, maxFoodMass);
        }

        public void AddFoodMass(float amount) {
            foodMass += amount;
            foodMass = Mathf.Clamp(foodMass, 0, maxFoodMass);
        }

        public void RemoveFoodMass(float amount) {
            foodMass -= amount;
            foodMass = Mathf.Clamp(foodMass, 0, maxFoodMass);
        }

    }

}
