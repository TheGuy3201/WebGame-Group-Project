using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Terminus
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] Camera playerCam;

        [SerializeField] GameObject manager;

        [SerializeField] PickUp PickUp;

        [SerializeField] TextMeshProUGUI interactionText;

        [SerializeField] float interactionDistance = 5f;

        IInteractable currentTargetedInteractable;

        public GameObject seen;

        public void Update()
        {
            UpdateCurrentInteractable();
            UpdateInteractionText();
            CheckForInteractionInput();
        }

        void UpdateCurrentInteractable()
        {
            Ray ray = playerCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out var hit, interactionDistance) && hit.collider != null)
            {
                if (hit.collider.CompareTag("PickUp") || hit.collider.CompareTag("ToInventory"))
                {
                    seen = hit.collider.gameObject;
                    currentTargetedInteractable = manager.GetComponent<IInteractable>();
                }
                /*else
                {
                    currentTargetedInteractable = null;
                }*/
            }
            else
            {
                if (PickUp.heldObj != null )
                {
                    seen = PickUp.heldObj;
                    currentTargetedInteractable = manager.GetComponent<IInteractable>();
                    
                }
                else
                {
                    currentTargetedInteractable = null;
                }
            }
            
        }


        void UpdateInteractionText()
        {
            if (currentTargetedInteractable == null)
            {
                interactionText.text = string.Empty;
                return;
            }

            interactionText.text = currentTargetedInteractable.InteractMessage;
        }

        void CheckForInteractionInput()
        {
            if (Keyboard.current.eKey.wasPressedThisFrame && currentTargetedInteractable != null)
            {
                currentTargetedInteractable.Interact(seen);
            }
        }
    }
}