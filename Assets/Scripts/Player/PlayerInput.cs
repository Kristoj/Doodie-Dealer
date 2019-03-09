using UnityEngine;

namespace Doodie.Player {
    public static class PlayerInput {

        public static void OnUpdate() {
            // Mouse left down
            if (Input.GetButtonDown("Fire1"))
                MouseButtonDown[0] = 1;
            // Mouse left up
            if (Input.GetButtonUp("Fire1"))
                MouseButtonDown[0] = 0;
            // Mouse right
            if (Input.GetButtonDown("Fire2"))
                MouseButtonDown[1] = 1;
            // Mouse right
            if (Input.GetButtonUp("Fire2"))
                MouseButtonDown[1] = 0;
        }

        // Mouse states
        private static int[] MouseButtonDown = new int[2];

        public static bool MouseButtonState(int buttonId) {
            return buttonId != MouseButtonDown[buttonId];
        }

        public static Vector3 GetMovementInput {
            get {
                return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
            }
        }

        public static Vector2 GetMouseInput {
            get {
                return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            }
        }

    }
}