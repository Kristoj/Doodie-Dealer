using UnityEngine;

namespace Doodie.Player {
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour {

        [Header("Ground Movement")]
        public float walkSpeed = 4f;
        public float sprintSpeed = 8f;
        private float speed;
        private float speedModifier = 1f;
        public float walkAcceleration = 90f;
        public float sprintAcceleration = 130f;
        private float groundAcceleration;
        public float deacceleration = 9f;

        [Header("Jumping")]
        public float jumpForce = 9f;

        [Header("Air Movement")]
        public float gravity = 33f;
        public float airAcceleration = 20;
        
        // Data
        [HideInInspector]
        public Vector3 velocity;

        // Misc
        private CharacterController controller;

        void Awake() {
            controller = GetComponent<CharacterController>();
            speed = walkSpeed;
            groundAcceleration = walkAcceleration;
        }

        void Update() {
            CheckInput();
            Accelerate();
            ApplyGravity();
            ApplyVelocity();
            Deaccelerate();
        }

        void Accelerate() {
            // Take input and add it to our velocity
            float acc = controller.isGrounded == true ? groundAcceleration * speedModifier : airAcceleration * speedModifier;
            Vector3 wishDir = transform.TransformDirection(PlayerInput.GetMovementInput).normalized * acc * Time.deltaTime;
            velocity += wishDir;

            // Clamp velocity
            Vector2 clampVel = new Vector3(velocity.x, velocity.z);
            clampVel = Vector2.ClampMagnitude(clampVel, speed * speedModifier);
            velocity = new Vector3(clampVel.x, velocity.y, clampVel.y);
        }

        void Deaccelerate() {
            if (controller.isGrounded) {
                
                velocity.x = Mathf.Lerp(velocity.x, 0, deacceleration * Time.deltaTime);
                velocity.z = Mathf.Lerp(velocity.z, 0, deacceleration * Time.deltaTime);
                
                /*
                Vector3 vec = velocity;
                float speed;
                float newSpeed;
                float control;
                float drop;
                float runDeacceleration = 2f;

                vec.y = 0;
                speed = vec.magnitude;
                drop = 0f;

                control = speed < runDeacceleration ? runDeacceleration : speed;
                drop = control * deacceleration * Time.deltaTime;

                newSpeed = speed - drop;
                if (newSpeed < 0)
                    newSpeed = 0;
                if (newSpeed > 0)
                    newSpeed /= newSpeed;

                velocity *= newSpeed;
                */
            }
        }

        void ApplyGravity() {
            // Airborne
            if (!controller.isGrounded)
                velocity.y -= gravity * Time.deltaTime;
            // Grounded
            //else if (velocity.y < 0)
            //velocity.y = 0;
        }

        void ApplyVelocity() {
            controller.Move(velocity * Time.deltaTime);
        }

        void CheckInput() {
            // Jumping
            if (Input.GetKeyDown(KeyCode.Space)) {
                Jump();
            }

            // Sprint start
            if (Input.GetKeyDown(KeyCode.LeftShift) && controller.isGrounded) {
                speed = sprintSpeed;
                groundAcceleration = sprintAcceleration;
            }
            // Sprint stop
            if (Input.GetKeyUp(KeyCode.LeftShift)) {
                speed = walkSpeed;
                groundAcceleration = walkAcceleration;
            }
            // Super speed
            if (Input.GetKeyDown(KeyCode.AltGr)) {
                float superSpeed = 7.5f;
                if (speedModifier == superSpeed) {
                    speedModifier = 1;

                } else {
                    speedModifier = superSpeed;
                }
            }
        }

        void Jump() {
            if (controller.isGrounded) {
                velocity.y = jumpForce;
                //SoundSystem.PlaySound2D("");
            }
        }
    }
}