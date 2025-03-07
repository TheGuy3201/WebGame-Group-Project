using System;
using Terminus;
using UnityEngine;

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
        [SerializeField] private float rotationSpeed = 200f;
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


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            mainCam = Camera.main.transform;
        }

        private void Start()
        {
            input.EnablePlayerActions();
            animator = GetComponent<Animator>();
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

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement()
        {
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
            if (adjustedDirection.magnitude > 0f)
            {
                HandleRotation(adjustedDirection);
                HandleMovement(adjustedDirection);
            }
            else
            {
                animator.SetFloat("Speed", 0);
                animator.SetFloat("MotionSpeed", 1);
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        private void HandleMovement(Vector3 adjustedMovement)
        {
            var velocity = adjustedMovement * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
            animator.SetFloat("Speed", 2);
            animator.SetFloat("MotionSpeed", 1);
        }

        private void HandleRotation(Vector3 adjustedRotation)
        {
            var targetRotation = Quaternion.LookRotation(adjustedRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        private void GetMovement(Vector2 move)
        {
            movement.x = move.x;
            movement.z = move.y;
        }

        private void Jump()
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
    }

    
}
