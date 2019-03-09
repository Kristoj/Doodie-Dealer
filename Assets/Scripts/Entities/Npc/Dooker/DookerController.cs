#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodie.Npc {

    public class DookerController : MonoBehaviour {

        [Header("Decision Making")]
        [SerializeField] private float decisionMakingRate = .5f;

        [Header("Movement Behaviour")]
        [SerializeField] private Vector2 pathRefreshInterval = new Vector2(5, 15);
        private float nextPathRefreshTime = 0;

        // Misc 
        private DookerDefecation dookerDefecation;
        private NpcNavigator navigator;
        private DookerStats stats;
        private Coroutine npcStateCoroutine;

        #region Setup
        void Awake() {
            SetupDependencies();
        }

        void Start() {
            npcStateCoroutine = StartCoroutine(NpcState());
        }

        void SetupDependencies() {
            navigator = GetComponent<NpcNavigator>();
            if (navigator == null) {
                Debug.LogError("Critical Error! " + gameObject.name + " doesn't have Npc Navigator attached to it.");
            }
            dookerDefecation = GetComponent<DookerDefecation>();
            if (dookerDefecation == null) {
                Debug.LogError("Critical Error! " + gameObject.name + " doesn't have Dooker Defecation attached to it.");
            }
        }
        #endregion

        IEnumerator NpcState() {
            while (true) {
                // Update path ?
                if (CanRefreshPath)
                    UpdatePath();
                // Defecate ?
                else if (dookerDefecation.CanDefecate)
                    dookerDefecation.Defecate();
                yield return new WaitForSeconds(decisionMakingRate);
            }
        }

        void UpdatePath() {
            navigator.TravelToLocation(TravelLocation.Dooker_Wander);
            nextPathRefreshTime = Time.time + Random.Range(pathRefreshInterval.x, pathRefreshInterval.y);
        }

        bool CanRefreshPath {
            get {
                // Wait for x amount of seconds before we can update path again
                if (Time.time < nextPathRefreshTime) {
                    return false;
                }
                return true;
            }
        }

    }

}