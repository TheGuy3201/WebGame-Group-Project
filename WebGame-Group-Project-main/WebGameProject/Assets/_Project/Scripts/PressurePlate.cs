using System.Collections;
using UnityEngine;

namespace Terminus
{

    public class PressurePlate : MonoBehaviour
    {
        public Door ConnectedDoor; // Assign the door in the inspector
        private int objectsOnPlate = 0; // Counter to allow multiple objects
        private Vector3 startPos;

        private void Awake()
        {
            startPos = transform.position;
        }

        private IEnumerator PressPlate()
        {
            transform.position = startPos + Vector3.down * 0.1f;
            yield return new WaitForSeconds(0.2f);
            transform.position = startPos;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Box"))
            {
                objectsOnPlate++;
                StartCoroutine(PressPlate()); // Add animation when object steps on the plate
                if (objectsOnPlate == 1) // First object activates the plate
                {
                    ConnectedDoor.ShallOpen = true;
                    ConnectedDoor.Open(transform.position);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Box"))
            {
                objectsOnPlate--;
                if (objectsOnPlate == 0) // Last object leaves
                {
                    ConnectedDoor.Close();
                }
            }
        }
    }

    // Door.cs (UPDATED)
    public class Door : MonoBehaviour
    {
        public bool IsOpen = false;
        public bool ShallOpen = false; // Controlled by the pressure plate
        [SerializeField] private bool IsRotatingDoor = true;
        [SerializeField] private float Speed = 1f;
        [SerializeField] private float RotationAmount = 90f;
        [SerializeField] private Vector3 SlideDirection = Vector3.back;
        [SerializeField] private float SlideAmount = 2.9f;

        private Vector3 StartRotation;
        private Vector3 StartPosition;
        private Coroutine AnimationCoroutine;

        private void Awake()
        {
            StartRotation = transform.rotation.eulerAngles;
            StartPosition = transform.position;
        }

        public void Open(Vector3 UserPosition)
        {
            if (!IsOpen && ShallOpen)
            {
                if (AnimationCoroutine != null) StopCoroutine(AnimationCoroutine);
                AnimationCoroutine = StartCoroutine(IsRotatingDoor ? DoRotationOpen() : DoSlidingOpen());
            }
        }

        private IEnumerator DoRotationOpen()
        {
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
            IsOpen = true;

            float time = 0;
            while (time < 1)
            {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                time += Time.deltaTime * Speed;
                yield return null;
            }
        }

        private IEnumerator DoSlidingOpen()
        {
            Vector3 endPosition = StartPosition + SlideDirection * SlideAmount;
            IsOpen = true;

            float time = 0;
            while (time < 1)
            {
                transform.position = Vector3.Lerp(StartPosition, endPosition, time);
                time += Time.deltaTime * Speed;
                yield return null;
            }
        }

        public void Close()
        {
            if (IsOpen)
            {
                if (AnimationCoroutine != null) StopCoroutine(AnimationCoroutine);
                AnimationCoroutine = StartCoroutine(IsRotatingDoor ? DoRotationClose() : DoSlidingClose());
            }
        }

        private IEnumerator DoRotationClose()
        {
            Quaternion startRotation = transform.rotation;
            Quaternion endRotation = Quaternion.Euler(StartRotation);
            IsOpen = false;

            float time = 0;
            while (time < 1)
            {
                transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
                time += Time.deltaTime * Speed;
                yield return null;
            }
        }

        private IEnumerator DoSlidingClose()
        {
            Vector3 startPosition = transform.position;
            IsOpen = false;

            float time = 0;
            while (time < 1)
            {
                transform.position = Vector3.Lerp(startPosition, StartPosition, time);
                time += Time.deltaTime * Speed;
                yield return null;
            }
        }
    }
}
