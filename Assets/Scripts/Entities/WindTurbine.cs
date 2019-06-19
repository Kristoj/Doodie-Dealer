using UnityEngine;

public class WindTurbine : MonoBehaviour {

    [Header("Rotator")]
    public Transform rotator;
    public float rotateSpeed = 3f;

    void Update() {
        RotateWings();
    }

    void RotateWings() {
        if (rotator != null)
            rotator.transform.RotateAround(rotator.transform.position, -rotator.transform.up, rotateSpeed * Time.deltaTime);
    }
}
