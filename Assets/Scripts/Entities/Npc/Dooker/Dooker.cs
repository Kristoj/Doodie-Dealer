#pragma warning disable 0649
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace Doodie.NPC {

    public class Dooker : MonoBehaviour {
        
        [Header("AI Settings")]
        public int pollRate = 1;

        [Header("Movement")]
        public float moveRadius = 15f;
        public Vector2 idleTimeMinMax = new Vector2(3, 7);

        [Header("Defecation")]
        public float doodieSpawnForce = 5f;
        public Vector3 doodieSpawnPos = new Vector3(0f, .3f, 0f);
        [HideInInspector] public Vector3 doodieSpawnDirection;
        public Vector3 doodieSpawnEuler;

        // Misc
        [HideInInspector] public StateMachine<Dooker> stateMachine;
        private Animator animator;
        private float animationVelocity;

        // Debug
        [SerializeField] private float defecationScore;

        #region Components
        // Navmesh agent for the NPC
        private NavMeshAgent _agent;
        public NavMeshAgent Agent {
            get {
                if (_agent == null) 
                    _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        }

        // Stores stats for the dooker
        private DookerStats _dookerStats;
        public DookerStats DookerStats {
            get {
                if (_dookerStats == null)
                    _dookerStats = GetComponent<DookerStats>();
                return _dookerStats;
            }
        }

        // Storest levels for the dooker
        private DookerLevels _dookerLevels;
        public DookerLevels DookerLevels {
            get {
                if (_dookerLevels == null) 
                    _dookerLevels = GetComponent<DookerLevels>();
                return _dookerLevels;
            }
        }
        #endregion

        void Start() {
            // Setup state machine for the npc and start with idle state
            stateMachine = new StateMachine<Dooker>(this);
            stateMachine.ChangeState(State_Idle.Instance);
            StartCoroutine(PollStates());
            animator = GetComponent<Animator>();
        }

        void Update() {
            animationVelocity = Mathf.Lerp(animationVelocity, Agent.velocity.magnitude, 4 * Time.deltaTime);
            animator.SetFloat("velocity", animationVelocity);    
            defecationScore = State_Defecate.Instance.GetScore(this);
        }

        // Runs for x times in a second and determines if the NPC wants to change state
        IEnumerator PollStates() {
            while(true) {

                if (stateMachine.currentState == null) {
                    // Get the largest score from all possible states
                    State<Dooker>[] states = new State<Dooker>[] {
                        State_Move.Instance,
                        State_Defecate.Instance,
                        State_Eat.Instance
                    };

                    // Select the state that has the biggest score as best state
                    State<Dooker> bestState = null;
                    float highestScore = 0;
                    for (int i = 0; i < states.Length; i++) {
                        float score = states[i].GetScore(this);
                        if (states[i].GetScore(this) > highestScore) {
                            highestScore = score;
                            bestState = states[i];
                        }

                        // Debug
                        Debug.Log(states[i].StateName + " got the score of " + score + ".");
                    }

                    // Change to the best state
                    if (bestState != null) {
                        stateMachine.ChangeState(bestState);
                    }
                }

                stateMachine.Update();
                yield return new WaitForSeconds(1 / (float)pollRate);
            }
        }
    }
    
    #region States
    // Idle state
    public class State_Idle : State<Dooker> {
        
        public override string StateName => "Idle";

        private static State_Idle _instance;
        public static State_Idle Instance {
            get {
                if (_instance == null)
                    _instance = new State_Idle();
                return _instance;
            }
        }

        public override void EnterState(Dooker owner) {
            owner.StartCoroutine(Idle(owner));
        }

        IEnumerator Idle(Dooker owner) {
            // Wait for x amount of seconds before dooker starts to consider doing something else
            float idleTime = Random.Range(owner.idleTimeMinMax.x, owner.idleTimeMinMax.y);
            yield return new WaitForSeconds(idleTime);
            ExitState(owner);
        }

        public override void ExitState(Dooker owner) {
            owner.stateMachine.ClearState();
        }

        public override void UpdateState(Dooker owner) {
            
        }

        public override float GetScore(Dooker owner) {
            float score = 0;
            return score;
        }

    }

    // Move state
    public class State_Move : State<Dooker> {

        public override string StateName => "Move";
        
        // Singleton for this state
        private static State_Move _instance;
        public static State_Move Instance {
            get {
                if (_instance == null)
                    _instance = new State_Move();
                return _instance;
            }
        }

        public override void EnterState(Dooker owner) {
            Vector3 randomPos = new Vector3(
                Random.Range(-owner.moveRadius, owner.moveRadius),
                0,
                Random.Range(-owner.moveRadius, owner.moveRadius)
            );
            randomPos += owner.transform.position;
            owner.Agent.SetDestination(randomPos);
        }

        public override void ExitState(Dooker owner) {
            owner.stateMachine.ClearState();
            owner.stateMachine.ChangeState(State_Idle.Instance);
        }

        public override void UpdateState(Dooker owner) {
            if (owner.transform.position == owner.Agent.destination) {
                ExitState(owner);
            }
        }

        public override float GetScore(Dooker owner) {
            float score = 50;
            return score;
        }
    }

    // Defecate
    public class State_Defecate : State<Dooker> {

        public override string StateName => "Defecate";

        private static State_Defecate _instance;
        public static State_Defecate Instance {
            get {
                if (_instance == null) 
                    _instance = new State_Defecate();
                return _instance;
            }
        }

        public override void EnterState(Dooker owner) {
            owner.StartCoroutine(ShitPush(owner));
        }

        IEnumerator ShitPush(Dooker owner) {
            SoundSystem.PlaySound("shit_push01", 1f, owner.transform.position);
            yield return new WaitForSeconds(Random.Range(.5f, 2));

            // Spawn a doodie and add randomized force to it
            Item clone = Dooker.Instantiate(Database.Instance.GetItem(ItemName.Doodie_Normal), owner.transform.position + owner.doodieSpawnPos, Quaternion.Euler(-owner.transform.forward));
            Rigidbody rig = clone.GetComponent<Rigidbody>();
            rig.AddForce(-owner.transform.forward * owner.doodieSpawnForce, ForceMode.Impulse);
            rig.AddTorque(Random.insideUnitSphere * 15, ForceMode.Impulse);

            // Update dooker stats
            owner.DookerStats.RemoveExcrement(owner.DookerStats.excrement * .8f);

            // FX
            SoundSystem.PlaySound("item_pickup", 1f, owner.transform.position);

            // TODO : Add experience to the dooker skills
            ExitState(owner);
        }

        public override void ExitState(Dooker owner) {
            owner.stateMachine.ClearState();
            owner.stateMachine.ChangeState(State_Idle.Instance);
        }

        public override void UpdateState(Dooker owner) {
            
        }

        public override float GetScore(Dooker owner) {
            float score = 0;
            // Increment the score based on the dookers excrement amount
            float excrementMass = owner.DookerStats.defecationUrgeCurve.Evaluate(owner.DookerStats.excrement / owner.DookerStats.maxFoodMass) * owner.DookerStats.defecationUrgeMultiplier;
            score += excrementMass;
            // TODO : Increment the score based on the distance to a defecation point
            return score;
        }

    }

    public class State_Eat : State<Dooker> {

        public override string StateName => "Eat";

        private static State_Eat _instance;
        public static State_Eat Instance {
            get {
                if (_instance == null)
                    _instance = new State_Eat();
                return _instance;
            }
        }

        public override void EnterState(Dooker owner) {
            FindClosestFoodDeposit(owner);
        }

        void FindClosestFoodDeposit(Dooker owner) {
            FoodDeposit closestDepo = GameManager.GetClosestFoodDeposit(owner);
            owner.StartCoroutine(MoveToFoodDepo(owner, closestDepo));
        }

        float GetDistanceToClosesFoodDeposit(Dooker owner) {
            return Vector3.Distance(owner.transform.position, GameManager.GetClosestFoodDeposit(owner).transform.position);
        }

        IEnumerator MoveToFoodDepo(Dooker owner, FoodDeposit foodDepo) {

            // Set NPC destination to the food depo and start distance checking after he is appromixately in-range
            float travelTime = Vector3.Distance(owner.transform.position, foodDepo.transform.position) / owner.Agent.speed *.7f;
            owner.Agent.SetDestination(foodDepo.transform.position);
            yield return new WaitForSeconds(travelTime);

            // Check the distance between the depo and NPC untill we're close enough to it
            float dst = Vector3.Distance(owner.transform.position, foodDepo.transform.position);
            while (dst > 1) {
                dst = Vector3.Distance(owner.transform.position, foodDepo.transform.position);
                yield return new WaitForSeconds(.2f);
            }

            // Take food from the food depo
            owner.Agent.SetDestination(owner.transform.position);
            Eat(owner, foodDepo);

            // After we have finished eating exit the state
            ExitState(owner);
        }

        void Eat(Dooker owner, FoodDeposit foodDepo) {
            float foodAmount = foodDepo.TakeFood(owner);
            // Add the foodmass to the dooker and decrement the foodmass in the food deposit
            owner.DookerStats.AddFoodMass(foodAmount);
            owner.DookerStats.SetHunger((owner.DookerStats.hunger / 100));
            // TODO : FX
        }

        public override void ExitState(Dooker owner) {
            owner.stateMachine.ClearState();
            owner.stateMachine.ChangeState(State_Idle.Instance);
        }

        public override void UpdateState(Dooker owner) {
            
        }

        public override float GetScore(Dooker owner) {
            float score = 0;
            // Increment the score based on how hungry the dooker is
            float hungerPercentage = owner.DookerStats.hungerUrgeCurve.Evaluate(owner.DookerStats.hunger / 100);
            score += hungerPercentage * 100;

            // Increment the score based on how close a food deposit is
            float searchDistance = 30f;
            float dstPercentage = GetDistanceToClosesFoodDeposit(owner) / searchDistance;
            score += owner.DookerStats.foodDistanceCurve.Evaluate(dstPercentage) * 60 * hungerPercentage;
            return score;
        }
    }
    #endregion

}