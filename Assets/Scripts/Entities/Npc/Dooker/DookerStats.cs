#pragma warning disable 0649
using UnityEngine;

public class DookerStats : MonoBehaviour {
    
    [SerializeField] private float maxPoower = 100; 

    void Awake() {
        _poower = 100;
    }

    private float _poower;
    public float Poower { 
        get {
            return _poower;
        }
        set {
            value = Mathf.Clamp(value, 0, maxPoower);     // Clamp the poower value between 0 - MAX
            _poower = value;
        }
     }

}
