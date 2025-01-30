using UnityEngine;
using UnityEngine.AI;

namespace WebGame397
{
    public class EnemyNavigation : MonoBehaviour
    {

        private NavMeshAgent agent;
        private Transform player;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
            player = GameObject.FindWithTag("Player").transform;
        }

        void Update()
        {
            agent.destination = player.position;
        }
    }
}
