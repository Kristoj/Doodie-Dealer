#pragma warning disable 0649
using UnityEngine;
using System.Collections;
using UnityEngine.AI;

namespace Doodie.NPC {

    public class Dooker : MonoBehaviour {
        
        [Header("AI Settings")]
        public int brainTickRate = 4;

        [Header("Movement")]
        public float moveRadius = 15f;

        [Header("Defecation")]
        public float doodieSpawnForce = 5f;
        public Vector3 doodieSpawnPos = new Vector3(0f, .3f, 0f);
        [HideInInspector] public Vector3 doodieSpawnDirection;
        public Vector3 doodieSpawnEuler;
        
        // Components
        private NavMeshAgent _agent;
        public NavMeshAgent Agent {
            get {
                if (_agent == null) 
                    _agent = GetComponent<NavMeshAgent>();
                return _agent;
            }
        } 

        // Misc
        [HideInInspector] public StateMachine<Dooker> stateMachine;

        void Start() {
            // Init
            doodieSpawnDirection = -transform.forward;

            // Setup state machine for the npc and start with idle state
            stateMachine = new StateMachine<Dooker>(this);
            stateMachine.ChangeState(State_Move.Instance);
            StartCoroutine(BrainTick());
        }

        IEnumerator BrainTick() {
            while(true) {

                // TODO : If NPC is idling rise his will to do something
                if (stateMachine.currentState == null) {
                    // TEMP : Randomizing next state
                    int rng = Random.Range(0, 2);
                    if (rng == 0) {
                        stateMachine.ChangeState(State_Move.Instance);
                    } else {
                        stateMachine.ChangeState(State_Defecate.Instance);
                    }
                }

                stateMachine.Update();
                yield return new WaitForSeconds(1 / (float)brainTickRate);
            }
        }

    }
    
    // Move
    public class State_Move : State<Dooker> {

        public override string StateName => "Dooker Move";
        
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
            owner.Agent.SetDestination(randomPos);
        }

        public override void ExitState(Dooker owner) {
            owner.stateMachine.ClearState();
        }

        public override void UpdateState(Dooker owner) {
            if (owner.transform.position == owner.Agent.destination) {
                ExitState(owner);
            }
        }

    }

    // Defecate
    public class State_Defecate : State<Dooker> {

        public override string StateName => "Defecating";

        private static State_Defecate _instance;
        public static State_Defecate Instance {
            get {
                if (_instance == null) 
                    _instance = new State_Defecate();
                return _instance;
            }
        }

        public override void EnterState(Dooker owner) {
            // Spawn shit
            Item clone = Dooker.Instantiate(Database.Instance.GetItem(ItemName.Doodie_Normal), owner.transform.position + owner.doodieSpawnPos, Quaternion.Euler(-owner.transform.forward));
            Rigidbody rig = clone.GetComponent<Rigidbody>();
            rig.AddForce(owner.doodieSpawnDirection * owner.doodieSpawnForce, ForceMode.Impulse);
            ExitState(owner);
        }

        public override void ExitState(Dooker owner) {
            owner.stateMachine.ClearState();
        }

        public override void UpdateState(Dooker owner) {

        }

    }
}
