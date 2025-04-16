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
        [SerializeField] private float moveSpeed = 5f;
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
        public float sensitivity = 0.2f; // Reduced sensitivity
        public Vector2 lookInput;
        public float maxLookAngle = 70f;
        private Vector2 smoothLookInput = Vector2.zero;
        private Vector2 currentLookVelocity = Vector2.zero;
        public float lookSmoothTime = 0.1f; // Further reduced smooth time for slower input response

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            mainCam = Camera.main.transform;
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
            input.Look += GetLook;
            input.Move += GetMovement;
            input.Jump += Jump;
        }

        private void OnDisable()
        {
            input.Look -= GetLook;
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

        private void GetLook(Vector2 value)
        {
#if UNITY_ANDROID
            lookInput = value * 0.05f; // Reduce touch input further on mobile
#else
            lookInput = value * 0.5f; // Reduce mouse/web input sensitivity
#endif
        }

        private void UpdateMovement()
        {
            // Remove mainCam.eulerAngles.y influence
            var adjustedDirection = movement; // Directly use movement input

            if (adjustedDirection.magnitude > 0f)
            {
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
            // Smooth the input for rotation
            smoothLookInput = Vector2.SmoothDamp(smoothLookInput, lookInput, ref currentLookVelocity, lookSmoothTime);

            // Handle vertical camera rotation (looking up/down)
            Vector3 currentRotation = mainCam.localEulerAngles;
            if (currentRotation.x > 180) currentRotation.x -= 360f;

            // Apply clamped vertical rotation
            float newX = Mathf.Clamp(currentRotation.x - smoothLookInput.y * sensitivity, -maxLookAngle, maxLookAngle);
            mainCam.localRotation = Quaternion.Euler(newX, 0f, 0f);

            // Handle horizontal player rotation (looking left/right)
            if (smoothLookInput.x != 0)
            {
                float yaw = transform.eulerAngles.y + smoothLookInput.x * sensitivity;
                rb.MoveRotation(Quaternion.Euler(0f, yaw, 0f));
            }
        }

        private void HandleMovement(Vector3 adjustedMovement)
        {
            // Transform movement to be relative to the player's current rotation
            Vector3 moveDirection = transform.TransformDirection(adjustedMovement);

            // Smoothly apply velocity
            var velocity = moveDirection * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z); // Ensure Y velocity isn't overridden

            animator.SetFloat("Speed", 2);
            animator.SetFloat("MotionSpeed", 1);
        }

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
