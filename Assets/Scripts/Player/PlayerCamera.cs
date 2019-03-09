using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Doodie.Player {

    public class PlayerCamera : MonoBehaviour {

        public Vector3 defaultCameraOffset = new Vector3(0, 1.5f, 0);
        public float mouseSensitivity = 130f;
        [HideInInspector]
        public Transform head;
        [HideInInspector]
        public Camera cam;

        // Data
        [HideInInspector]
        public Vector3 camEuler;
        [HideInInspector]
        public Vector3 positionOffset;

        [Header("Options")]
        public bool hideCursor = true;

        void Awake() {
            camEuler.y = transform.eulerAngles.y;
        }

        void Start() {
            // Hide cursor ?
            if (hideCursor) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void Update() {
            CameraLook();
            CheckInput();
        }

        void CameraLook() {

            // Input
            camEuler.x -= PlayerInput.GetMouseInput.y * mouseSensitivity * Time.deltaTime;
            camEuler.y += PlayerInput.GetMouseInput.x * mouseSensitivity * Time.deltaTime;

            // Clamp our cam euler
            camEuler.x = Mathf.Clamp(camEuler.x, -90, 90);

            // Set our rotation
            transform.rotation = Quaternion.Euler(0, camEuler.y, 0);
            head.transform.rotation = Quaternion.Euler(camEuler.x, camEuler.y, 0);

            // Set our position
            head.transform.localPosition = defaultCameraOffset + positionOffset;
        }

        void CheckInput() {

            // Show cursor
            if (Input.GetKeyDown(KeyCode.Escape)) {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }

            // Hide cursor
            if (Input.GetButtonDown("Fire1")) {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }

        }
    }
}
