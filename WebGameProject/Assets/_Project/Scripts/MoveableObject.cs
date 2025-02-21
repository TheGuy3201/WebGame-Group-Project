using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace Terminus
{
    public class MoveableObject : MonoBehaviour, IInteractable
    {
        public string InteractMessage => objectInteractMessage;
        [SerializeField] string objectInteractMessage;

        [SerializeField] public GameObject[] interactables;

        private void Start()
        {
            interactables = GameObject.FindGameObjectsWithTag("Interactable");
        }

        //What happens when player interacts
        public void Interact()
        {
            Debug.Log("What sound does a black cow make");
        }
    }
}
