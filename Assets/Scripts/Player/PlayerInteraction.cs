#pragma warning disable 0649
using UnityEngine;

public class PlayerInteraction : MonoBehaviour {

    [SerializeField] private float focusRange = 3.8f;
    [SerializeField] private LayerMask focusMask;
    public Interactable TargetInteractable { get; set; }
    private Interactable lastTargetInteractable;

    void Update() {
        CastFocusRay();
        CheckInput();
    }
    
    void CheckInput() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (TargetInteractable != null) {
                TargetInteractable.InteractionStart();
            }
        }
    }

    /// <summary>
    /// Casts a ray forward from the player camera.
    /// </summary>
    /// <returns> Returns RaycastHit that contains hit data.</returns>
    RaycastHit CastFocusRay() {
        // Specify ray origin and direction
        Ray ray = new Ray(GameManager.LocalPlayer.Player_Camera.head.transform.position, 
                        GameManager.LocalPlayer.Player_Camera.head.transform.forward);
        // Check if we hit something
        if (Physics.Raycast(ray, out RaycastHit hit, focusRange, focusMask, QueryTriggerInteraction.Collide)) {
            TargetInteractable = hit.collider.GetComponent<Interactable>();
            // Check if we should call OnFocusEnter on the target interactable that we hit
            if ((lastTargetInteractable == null || lastTargetInteractable != TargetInteractable) && TargetInteractable != null) {
                TargetInteractable.FocusStart();
                lastTargetInteractable = TargetInteractable;
            }
            // Check if we should call OnFocusExit on the last target interactable that we hit
            else if (lastTargetInteractable != null && TargetInteractable == null) {
                lastTargetInteractable.FocusEnd();
                lastTargetInteractable = null;
            }
            return hit;
        }
        // Check if we should call OnFocusExit on the last target interactable that we hit
        if (lastTargetInteractable != null) {
            lastTargetInteractable.FocusEnd();
            lastTargetInteractable = null;
        }
        TargetInteractable = null;  // Set the target interactable to NULL when we didn't hit anything
        return new RaycastHit();
    }
}