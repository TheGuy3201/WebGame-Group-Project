using Unity.Cinemachine;
using UnityEngine;

namespace WebGame397
{
    public class CameraManager : MonoBehaviour
    {
        //References to CinemachineVirtualCamera and the transform of our player
        [SerializeField] private CinemachineCamera freeLookCam;
        [SerializeField] private Transform player;


        //In awake, i want to lock the mouse into the game view in unity and turn the cursor invisible

        private void Awake()
        {
            if (player != null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //On enable, i want to associate the transform of our player into the target of your cinemachine camera
        private void OnEnable()
        {
            freeLookCam.Target.TrackingTarget = player;
        }
    }
}
