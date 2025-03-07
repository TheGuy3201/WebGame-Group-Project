using System.Collections;
using UnityEngine;

namespace Terminus
{
    public class Puzzle1 : MonoBehaviour
    {
        public Door ConnectedDoor; // Assign the door in the inspector
        private int objectsOnPlate = 0; // Counter for multiple objects
        private Vector3 startPos;

        private void Awake()
        {
            startPos = transform.position;
        }

        private IEnumerator PressPlate()
        {
            transform.position = startPos + Vector3.down * 0.1f; // Plate goes down
            yield return new WaitForSeconds(0.2f);
        }

        private IEnumerator ReleasePlate()
        {
            yield return new WaitForSeconds(0.2f);
            transform.position = startPos; // Plate goes back up
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PickUp") || other.CompareTag("Player"))
            {
                objectsOnPlate++;
                StartCoroutine(PressPlate());
                if (objectsOnPlate == 1) // Only trigger door once
                {
                    ConnectedDoor.ShallOpen = true;
                    ConnectedDoor.Open(transform.position);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("PickUp") || other.CompareTag("Player"))
            {
                objectsOnPlate--;
                if (objectsOnPlate == 0) // When no objects left, close the door
                {
                    StartCoroutine(ReleasePlate());
                    ConnectedDoor.Close();
                }
            }
        }
    }
}