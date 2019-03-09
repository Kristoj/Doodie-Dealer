using UnityEngine;

public class Interactable : MonoBehaviour {

    public delegate void FocusStartedEventHandler();
    public event FocusStartedEventHandler FocusStarted;
    public delegate void FocusEndedEventHandler();
    public event FocusEndedEventHandler FocusEnded;

    public virtual void InteractionStart() {

    }

    public virtual void FocusStart() {
        OnFocusStarted();
    }

    public virtual void FocusEnd(){
        OnFocusEnded();
        try {
            GameManager.LocalPlayer.Player_UI.interactionMsg.text = "";
        }
        catch {
            Debug.LogWarning("Player interaction msg text is not assigned!");
        }
        
    }

    #region Events
    protected virtual void OnFocusStarted() {
        if (FocusStarted != null) {
            FocusStarted.Invoke();
        }
    }

    protected virtual void OnFocusEnded() {
        if (FocusEnded != null) {
            FocusEnded.Invoke();
        }
    }
    #endregion

}
