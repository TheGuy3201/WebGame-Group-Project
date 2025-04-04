using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace WebGame397
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "_Project/ScriptableObjects/InputReader.asset")]
    public class InputReader : ScriptableObject, IPlayerActions
    {
        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction Jump = delegate { };

        InputSystem_Actions input;

        public void Awake()
        {
            if (input == null)
            {
                input = new InputSystem_Actions();
            }
                input.Enable();
        }
        
        private void OnEnable()
        {
            if (input == null)
            {
                input = new InputSystem_Actions();
            }
            input.Player.SetCallbacks(this);
        }
        
        public void EnablePlayerActions()
        {
            if (input == null)
            {
                input = new InputSystem_Actions();
            }
            input.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Performed:
                case InputActionPhase.Canceled:
                    Move?.Invoke(context.ReadValue<Vector2>());
                    break;
                default:
                    break;
            }
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                Jump?.Invoke();
            }
        }

        public void OnLook(InputAction.CallbackContext context) { }
        public void OnAttack(InputAction.CallbackContext context) { }
        public void OnInteract(InputAction.CallbackContext context) { }
        public void OnCrouch(InputAction.CallbackContext context) { }
        public void OnPrevious(InputAction.CallbackContext context) { }
        public void OnNext(InputAction.CallbackContext context) { }
        public void OnSprint(InputAction.CallbackContext context) { }
    }
}
