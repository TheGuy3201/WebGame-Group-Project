using Unity.VisualScripting;
using UnityEngine;
using WebGame397;

namespace Terminus
{
    public class MoveableObject : MonoBehaviour, IInteractable
    {
        public string InteractMessage => objectInteractMessage;

        [SerializeField] string objectInteractMessage;

        [SerializeField] public GameObject[] interactables;

        [SerializeField] PickUp pickUpScript;


        private void Start()
        {
            interactables = GameObject.FindGameObjectsWithTag("PickUp");
        }

        //What happens when player interacts
        public void Interact(GameObject obj)
        {
            switch (obj.tag)
            {
                case "PickUp":
                    if(pickUpScript.heldObj == null)
                        pickUpScript.PickUpObject(obj.transform.gameObject);
                    else
                    {
                        if (pickUpScript.canDrop == true)
                        {
                            pickUpScript.StopClipping(); //prevents object from clipping through walls
                            pickUpScript.DropObject();
                        }
                    }
                    if (pickUpScript.heldObj != null) //if player is holding object
                    {
                        pickUpScript.MoveObject(); //keep object position at holdPos
                        if (Input.GetKeyDown(KeyCode.E) && pickUpScript.canDrop == true) 
                        {
                            pickUpScript.StopClipping();
                            pickUpScript.ThrowObject();
                        }

                    }
                    Debug.Log("PickUp");
                    break;

                case "ToInventory":
                    ToInventory();
                    break;

                default:
                    Debug.Log("What sound does a black cow make");
                    break;
            }
        }

        public void Interact()
        {
            GameObject obj = null;
            obj = GameObject.FindGameObjectWithTag("Player").GetComponent<InteractionController>().seen;
            switch (obj.tag)
            {
                case "PickUp":
                    if (pickUpScript.heldObj == null)
                        pickUpScript.PickUpObject(obj.transform.gameObject);
                    else
                    {
                        if (pickUpScript.canDrop == true)
                        {
                            pickUpScript.StopClipping(); //prevents object from clipping through walls
                            pickUpScript.DropObject();
                        }
                    }
                    if (pickUpScript.heldObj != null) //if player is holding object
                    {
                        pickUpScript.MoveObject(); //keep object position at holdPos
                        if (Input.GetKeyDown(KeyCode.E) && pickUpScript.canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
                        {
                            pickUpScript.StopClipping();
                            pickUpScript.ThrowObject();
                        }

                    }
                    Debug.Log("PickUp");
                    break;

                case "ToInventory":
                    ToInventory();
                    break;

                default:
                    Debug.Log("What sound does a black cow make");
                    break;
            }
        }

        private void ToInventory()
        {
            Debug.Log("Send Item To Inventory, Soon to be complete");
        }
    }
}
