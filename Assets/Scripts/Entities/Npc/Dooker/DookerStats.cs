using UnityEngine;

namespace Doodie.NPC {

    public class DookerStats : MonoBehaviour {

        [Header("Defecation Settings")]
        [SerializeField] private float maxExcrementAmount = 100;
        [SerializeField] private float excrementAmount = 100;
        [SerializeField] private AnimationCurve excrementUrgeCurve;
        [SerializeField] private float excrementUrgeMultiplier = 5f;
        [SerializeField] private float defecationUrgeRiseSpeed = 5f;
        [SerializeField] private float defecationThreshold = 65f;
        [SerializeField] private float scoreResult;


        [SerializeField] private float defecationUrge;
        public float DefecationScore {
            get {
                float score = 0;

                // Increment the score based on the dookers excrement amount
                float excrementUrge = excrementUrgeCurve.Evaluate(excrementAmount / maxExcrementAmount) * excrementUrgeMultiplier;


                // Increment the score based on dookers urge to defecate
                if (defecationUrge >= defecationThreshold) {
                    float scoreMultiplier = 1f;
                    float thresholdRange = 100 - defecationThreshold;
                    float rangePercentage = 100 / (thresholdRange / (defecationUrge - defecationThreshold));
                    score = rangePercentage * scoreMultiplier;
                }
                // TODO : Incement the score based on the distance to a toilet
                return score;
            }
        }

        void Update() {
            UpdateDefecationUrge();
            scoreResult = DefecationScore;
        }

        void UpdateDefecationUrge() {
            defecationUrge += defecationUrgeRiseSpeed * Time.deltaTime;
            defecationUrge = Mathf.Clamp(defecationUrge, 0, 100);
        }

    }

}
