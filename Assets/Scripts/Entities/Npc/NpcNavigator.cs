using UnityEngine;
using UnityEngine.AI;

namespace Doodie.Npc {

// List of all possible travel locations
public enum TravelLocation {
    Shop,                           
    Dooker_Nearest_Toilet,                 
    Dooker_Wander
}

/// <summary>
/// Used to command a npc to travel to specified destination.
/// </summary>
public class NpcNavigator : MonoBehaviour {
    
    // Member vars
    private NavMeshAgent agent;

    #region Setup
    void Awake() {
        SetupDependencies();
    }

    void SetupDependencies() {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
            Debug.LogError("Critical error! " + gameObject.name + " Doesn't have Nav Mesh Agent! Please add one.");
    }
    #endregion

    #region Travelling
    /// <summary>
    /// Makes the NPC travel to the specified location
    /// </summary>
    /// <param name="location">Location to which NPC will travel</param>
    public void TravelToLocation(TravelLocation location) {
        // Shop
        if (location == TravelLocation.Shop) {
            TravelShop();
        }
        // Dooker Wander
        if (location == TravelLocation.Dooker_Wander) {
            TravelDookerWander();
        }
    }

    void TravelShop() {
        UpdateDestination(new Vector3(10, 0, 10));
    }

    void TravelDookerNearestToilet() {
        UpdateDestination(new Vector3(5, 0, 5));
    }

    void TravelDookerWander() {
        UpdateDestination(new Vector3(
                Random.Range(-10, 10), 
                0, 
                Random.Range(0, -10)));
    }

    void UpdateDestination(Vector3 destination) {
        agent.SetDestination(destination);
    }

    /// <summary>
    /// Travels the NPC to the given coordinates.
    /// </summary>
    /// <param name="coords">Coordinates to travel to.</param>
    public void TravelToCoordinates(Vector3 coords) {
        agent.SetDestination(coords);
    }
    #endregion
}

}