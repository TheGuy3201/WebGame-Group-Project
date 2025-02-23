using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using WebGame397;

namespace Terminus
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] Camera playerCam;

        [SerializeField] GameObject manager;

        [SerializeField] TextMeshProUGUI interactionText;

        [SerializeField] float interactionDistance = 5f;

        IInteractable currentTargetedInteractable;

        public void Update()
        {
            UpdateCurrentInteractable();
            UpdateInteractionText();
            CheckForInteractionInput();
        }

        void UpdateCurrentInteractable()
        {
            var ray = playerCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out var hit, interactionDistance) && hit.collider != null)
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    currentTargetedInteractable = manager.GetComponent<IInteractable>();
                }
            }
            else
            {
                currentTargetedInteractable = null;
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
                currentTargetedInteractable.Interact();
            }
        }
    }
}
