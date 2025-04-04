using System;
using Terminus;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WebGame397
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        //Input and movement readers
        [SerializeField] private InputReader input;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Vector3 movement;

        //Movement variables
        [SerializeField] private float moveSpeed = 200f;
        [SerializeField] private float jumpForce = 10f;

        [SerializeField] private Transform mainCam;

        //Animation/Sound variables
        [SerializeField] private Animator animator;
        [SerializeField] private AudioClip[] FootstepAudioClips;
        [SerializeField] private AudioClip LandingAudioClip;
        [SerializeField] private float FootstepAudioVolume = 1.0f;
        private bool isGrounded;

        //Health System variables
        public event EventHandler OnDamage;
        public float health;

        //Death Screen Variables
        public GameOver_Manager GameOverScreen;

        //Rotation fix
        public float sensitivity = 1;
        public Vector2 lookInput;
        public float maxLookAngle = 70f;



        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            //mainCam = Camera.main.transform;

        }

        private void Start()
        {
            if (input == null)
            {
                input = ScriptableObject.CreateInstance<InputReader>();
            }
            input.EnablePlayerActions();
            int used_load = PlayerPrefs.GetInt(Constants.usedload, 0);
            if (used_load == 1)
            {
                LoadSavedPos();
                PlayerPrefs.SetInt(Constants.usedload, 0);
            }
        }

        private void OnEnable()
        {
            input.Move += GetMovement;
            input.Jump += Jump;
        }

        private void OnDisable()
        {
            input.Move -= GetMovement;
            input.Jump -= Jump;
        }

        public void Update()
        {
            HandleRotation();
        }
        private void FixedUpdate()
        {
            UpdateMovement();
        }


        public void OnLook(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>();  // Right Stick input from gamepad
        }

        private void UpdateMovement()
        {

            // Remove mainCam.eulerAngles.y influence
            var adjustedDirection = movement; // Directly use movement input

            if (adjustedDirection.magnitude > 0f)
            {
                //HandleRotation(adjustedDirection);
                HandleMovement(adjustedDirection);
            }
            else
            {
                animator.SetFloat("Speed", 0);
                animator.SetFloat("MotionSpeed", 1);
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        public void HandleRotation()
        {
            Vector3 currentRotation;
            float newRotationX;

            // Handle vertical camera rotation (looking up/down)
            if (lookInput.y != 0)
            {
                currentRotation = mainCam.eulerAngles;

                // Convert rotation to range -180 to 180 to avoid flipping
                if (currentRotation.x > 180) { currentRotation.x -= 360; }

                // Apply clamped vertical rotation
                newRotationX = currentRotation.x - lookInput.y * sensitivity;
                newRotationX = Mathf.Clamp(newRotationX, -maxLookAngle, maxLookAngle);

                // Set camera rotation
                mainCam.rotation = Quaternion.Euler(newRotationX, currentRotation.y, currentRotation.z);
            }

            // Handle horizontal player rotation (looking left/right)
            if (lookInput.x != 0)
            {
                Quaternion newRotation = Quaternion.Euler(0, lookInput.x * sensitivity + transform.eulerAngles.y, 0);
                rb.MoveRotation(newRotation);  
            }
        }

        private void HandleMovement(Vector3 adjustedMovement)
        {
            // Transform movement to be relative to the player's current rotation
            Vector3 moveDirection = transform.TransformDirection(adjustedMovement);

            var velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z); // Ensure Y velocity isn't overridden

            animator.SetFloat("Speed", 2);
            animator.SetFloat("MotionSpeed", 1);
        }

        /*
                private void HandleRotation(Vector3 adjustedRotation)
                {
                    var targetRotation = Quaternion.LookRotation(adjustedRotation);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
        */

        private void GetMovement(Vector2 move)
        {
            movement.x = move.x;
            movement.z = move.y;
        }

        public void Jump()
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                animator.Play("JumpStart");
                isGrounded = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                animator.SetBool("Grounded", isGrounded);
            }
        }

        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips == null || FootstepAudioClips.Length == 0)
                {
                    Debug.LogWarning("FootstepAudioClips array is empty or not assigned!");
                    return;
                }

                var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);

                // Play sound at footstep position if available, otherwise use transform.position
                Vector3 soundPosition = transform.position;
                AudioSource.PlayClipAtPoint(FootstepAudioClips[index], soundPosition, FootstepAudioVolume);
            }
        }

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (LandingAudioClip == null)
                {
                    Debug.LogWarning("LandingAudioClip is not assigned!");
                    return;
                }

                Vector3 soundPosition = transform.position;
                AudioSource.PlayClipAtPoint(LandingAudioClip, soundPosition, FootstepAudioVolume);
            }
        }

        private void OnDeath()
        {
            health = 0;
            GameOverScreen.Setup("Game Over");
            Debug.Log("You have died");
        }

        public void Damage(float amount)
        {
            health -= amount;

            if (health < 0)
                OnDeath();

            if (OnDamage != null) OnDamage(this, EventArgs.Empty);
        }

        private void LoadSavedPos()
        {
            GameObject player;
            float x, y, z;
            try
            {
                player = GameObject.FindWithTag("Player");
                x = PlayerPrefs.GetFloat(Constants.player_pos_x);
                y = PlayerPrefs.GetFloat(Constants.player_pos_y);
                z = PlayerPrefs.GetFloat(Constants.player_pos_z);
                gameObject.transform.position = new Vector3(x, y, z);
                rb.position = new Vector3(x, y, z);
                Debug.Log("Game Loaded");
                Debug.Log($"x:{x}, y:{y}, z:{z}");
                Debug.Log(gameObject.transform.position);
            }
            catch (System.Exception error)
            {
                Debug.Log("Error loading pos" + error.Message);
                Debug.LogException(error);
                return;
            }
        }
    }
}
