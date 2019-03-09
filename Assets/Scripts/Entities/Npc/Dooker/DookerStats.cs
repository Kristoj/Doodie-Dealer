#pragma warning disable 0649
using UnityEngine;

public class DookerStats : MonoBehaviour {
    
    [SerializeField] private float maxStomachWaste = 100; 
    [SerializeField] private float _stomachWaste;

    void Awake() {
        _stomachWaste = 100;
    }

    public float StomachWaste { 
        get {
            return _stomachWaste;
        }
        set {
            value = Mathf.Clamp(value, 0, maxStomachWaste);     // Clamp the stomach waste value between 0 - MAX
            _stomachWaste = value;
        }
     }

}
