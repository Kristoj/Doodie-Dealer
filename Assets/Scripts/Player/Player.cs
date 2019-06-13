using UnityEngine;
using Doodie.Player;

public class Player : MonoBehaviour {

    public PlayerCamera Player_Camera { get; set; }
    public PlayerController Player_Controller { get; set; }
    public Inventory Player_Inventory { get; set; }
    public PlayerInteraction Player_Interaction { get;set; }
    public PlayerUI Player_UI { get; set; }
    public PlayerWallet Player_Wallet { get;set; }

    // Start is called before the first frame update
    void Awake() {
        GameManager.LocalPlayer = this; // Register local player
        SetupReferences(); // Setup references that other classes might use
        
        // TODO
        // This should be somewhere else
        // Player class shouldn't have to worry about application settings....
        Application.targetFrameRate = 144;
    }

    
    // On awake we find each objects reference
    void SetupReferences() {

        // Player components
        Player_Camera = GetComponent<PlayerCamera>();
        Player_Controller = GetComponent<PlayerController>();
        Player_Inventory = GetComponent<Inventory>();
        Player_Interaction = GetComponent<PlayerInteraction>();
        Player_UI = GetComponent<PlayerUI>();
        Player_Wallet = GetComponent<PlayerWallet>();

        // Find player head
        Transform t = transform.GetChild(0);
        if (t.name == "Head") {
            Player_Camera.head = t;
            Player_Camera.cam = t.GetChild(0).GetComponent<Camera>();
        } else {
            Debug.LogError("Player HEAD gameobject must be the first child!");
        }

    }
}
