using UnityEngine;

public class UICard : MonoBehaviour {

    public GameObject cardPrefab;

    void Start() {
        SetupCallbacks(); // Setup event callbacks
    }

    public void FocusStartedCallback() {
        GameManager.LocalPlayer.Player_UI.CreateUICard(this);
    }

    public void FocusEndedCallback() {
        GameManager.LocalPlayer.Player_UI.DestroyUICard();
    }

    void SetupCallbacks() {
        Interactable intera = GetComponent<Interactable>();
        try {
            intera.FocusStarted += FocusStartedCallback;
            intera.FocusEnded += FocusEndedCallback;
        }
        catch {
            Debug.LogWarning(gameObject.name + " UICard doesn't have interactable component attached to it.");
        }
    }

}
