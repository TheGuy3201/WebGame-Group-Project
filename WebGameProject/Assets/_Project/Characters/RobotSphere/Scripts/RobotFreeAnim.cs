using UnityEngine;
using UnityEngine.AI;

public class RobotFreeAnim : MonoBehaviour
{
    private Animator anim;
    private NavMeshAgent agent;
    private Transform player;

    [SerializeField] private float detectionRange = 6f; // Detection range
    [SerializeField] private float moveSpeed = 6.5f; // Movement speed
    [SerializeField] private float stoppingDistance = 1.5f; // Stop when close to player

    void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Get the player once
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

    void Update()
    {
        if (player != null)
        {
            MoveToPlayer();
            UpdateAnimation();
        }
    }

    void MoveToPlayer()
    {
        if (agent == null || !agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent is not on a valid NavMesh surface!");
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRange && distance > stoppingDistance)
        {
            agent.speed = moveSpeed;
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath(); // Stop moving if too close
        }
    }

    void UpdateAnimation()
    {
        bool isMoving = agent.velocity.magnitude > 0.5f;
        anim.SetBool("Walk_Anim", isMoving);
        anim.SetBool("Open_Anim", !isMoving);
    }
}
