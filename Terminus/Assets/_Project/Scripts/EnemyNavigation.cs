using UnityEngine;
using UnityEngine.AI;

namespace WebGame397
{
    public class EnemyNavigation : MonoBehaviour
    {
        private NavMeshAgent agent;
        private Transform player;

        [SerializeField] private float stoppingDistance = 1.5f; // Distance to stop

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            GameObject playerObj = GameObject.FindWithTag("Player");

            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
            }
        }

        private void Update()
        {
            if (player != null && agent != null && agent.isActiveAndEnabled && agent.isOnNavMesh)
            {
                float distance = Vector3.Distance(transform.position, player.position);

                if (distance > stoppingDistance)
                {
                    agent.SetDestination(player.position);
                }
                else
                {
                    agent.ResetPath(); // Stop moving if too close
                }
            }
        }
    }
}
