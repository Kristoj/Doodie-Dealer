#pragma warning disable 0649
using UnityEngine;

namespace Doodie.NPC {

    public class DookerStats : MonoBehaviour {

        [Header("Defecation Settings")]
        [Tooltip("The maximum amount of foodmass that the stomach can hold in milliliters")]
        public float maxFoodMass = 1500;
        [SerializeField] private AnimationCurve defecationUrgeCurve;
        [SerializeField] private float defecationUrgeMultiplier = 5f;
        [SerializeField] private float defecationUrgeRiseSpeed = 5f;
        [SerializeField] private float defecationThreshold = 65f;
        [SerializeField] private float scoreResult;
        [SerializeField] private float defecationUrge;
        [Tooltip("The amount of foodmass in the stomach in milliliters")]
        public float foodMass;
        [Tooltip("The amount of excrement in the intestines in milliliters.")]
        [SerializeField] private float excrement;

        [Header("Digestion")]
        [SerializeField] private float digestionSpeed = 1;

        public float DefecationScore {
            get {
                float score = 0;

                // Increment the score based on the dookers excrement amount
                float excrementUrge = defecationUrgeCurve.Evaluate(foodMass / maxFoodMass) * defecationUrgeMultiplier;

                // Increment the score based on dookers urge to defecate
                if (defecationUrge >= defecationThreshold) {
                    float scoreMultiplier = 1f;
                    float thresholdRange = 100 - defecationThreshold;
                    float rangePercentage = 100 / (thresholdRange / (defecationUrge - defecationThreshold));
                    score += rangePercentage * scoreMultiplier;
                }
                // TODO : Increment the score based on the distance to a toilet
                return score;
            }
        }

        void Start() {
            excrement = foodMass;
        }

        void Update() {
            UpdateDefecationUrge();
            Digestion();
            scoreResult = DefecationScore;
        }

        // Turns foodmass into excrement overtime
        void Digestion() {
            float digestionAmount = foodMass;
            digestionAmount = Mathf.Clamp(digestionAmount, 0, digestionSpeed);
            if (digestionAmount <= .05) {
                foodMass = 0;
                excrement = 0;
                return;
            }

            foodMass -= digestionAmount * Time.deltaTime;
            excrement += digestionAmount * Time.deltaTime;
        }

        void UpdateDefecationUrge() {
            defecationUrge += defecationUrgeRiseSpeed * Time.deltaTime;
            defecationUrge = Mathf.Clamp(defecationUrge, 0, 100);
        }


        public void RemoveExcrement(float amount) {
            excrement -= amount;
            excrement = Mathf.Clamp(excrement, 0, maxFoodMass);
        }

    }

}
