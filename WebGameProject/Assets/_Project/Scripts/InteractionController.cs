using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using WebGame397;

namespace Terminus
{
    public class InteractionController : MonoBehaviour
    {
        [SerializeField] Camera playerCam;
        [SerializeField] private InputReader input;

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

            Physics.Raycast(ray, out var hit, interactionDistance);
            if (hit.collider.CompareTag("Interactable"))
            {
                currentTargetedInteractable = manager.GetComponent<IInteractable>();
            }
            else
            {
                currentTargetedInteractable = hit.collider?.GetComponent<IInteractable>();
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
