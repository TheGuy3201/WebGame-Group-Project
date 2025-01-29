using UnityEngine;

namespace WebGame397
{
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private InputReader input;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Vector3 movement;

        [SerializeField] private float moveSpeed = 200f;
        [SerializeField] private float rotationSpeed = 200f;

        [SerializeField] private Transform mainCam;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.freezeRotation = true;
            mainCam = Camera.main.transform;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            //Debug.Log("[Start]");
            input.EnablePlayerActions();
        }

        private void OnEnable()
        {
            input.Move += GetMovement;
            //Debug.Log("[OnEnable]");
        }

        private void OnDisable()
        {
            input.Move -= GetMovement;
            //Debug.Log("[OnDisable]");
        }

        private void FixedUpdate()
        {
            UpdateMovement();
        }

        private void UpdateMovement() 
        {
            //auto id of type
            var adjustedDirection = Quaternion.AngleAxis(mainCam.eulerAngles.y, Vector3.up) * movement;
            if (adjustedDirection.magnitude > 0f)
            {
                //Handle rotation and movement
                HandleRotation(adjustedDirection);
                HandleMovement(adjustedDirection);
            }
            else
            {
                //not change rotation or movement, but need to apply rigidbody Y for gravity
                rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
            }
        }

        private void HandleMovement(Vector3 adjustedMovement) 
        {
            var velocity = adjustedMovement * moveSpeed * Time.fixedDeltaTime;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
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
            //Debug.Log($"Input Working {move}");
        }

        /*private void Destroy()
        {
            Debug.Log("[Destroy]");
        }*/
    }
}
