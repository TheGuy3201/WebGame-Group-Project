using UnityEngine;

namespace Terminus
{
    public class MiniMap_Controller : MonoBehaviour
    {
        [SerializeField] public Transform player;

        private void LateUpdate()
        {
            Vector3 newPos = player.position;
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
