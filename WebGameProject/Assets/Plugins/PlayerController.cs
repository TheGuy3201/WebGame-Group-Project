using UnityEngine;

namespace WebGame397
{
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private InputReader input;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            Debug.Log("[Start]");
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

        private void GetMovement(Vector2 move)
        {
            Debug.Log($"Input Working {move}");
        }

        /*private void Destroy()
        {
            Debug.Log("[Destroy]");
        }*/
    }
}
