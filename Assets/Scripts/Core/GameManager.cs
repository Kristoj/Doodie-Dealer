using UnityEngine;

public static class GameManager {

    private static Player _localPlayer;
    public static Player LocalPlayer {
        get {
            if (_localPlayer == null) {
                Debug.LogError("Vili pls. Could't find local player reference!");
            }
            return _localPlayer;
        }
        set {
            _localPlayer = value;
        }
    }
}
