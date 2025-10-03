using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Target and State")]
    public Transform player; 
    public GameObject devilHorns; 

    [Header("Movement Parameters")]
    public float patrolSpeed = 2f; // patrol speed
    public float chaseSpeed = 5f;  // chase speed
    [Tooltip("The minimum rotation angle allowed for the enemy.")]
    public float minRotationAngle = -45f;
    [Tooltip("The maximum rotation angle allowed for the enemy.")]
    public float maxRotationAngle = 45f;

    [Header("Detection Range")]
    public float activationRange = 8f; // the distance to activate and start chasing
    public float losePlayerRange = 15f; // the distance to lose the player and switch to patrolling

    [Header("Patrol Points")]
    public Transform patrolPointA; // patrol point A
    public Transform patrolPointB; // patrol point B

    private enum State
    {
        Dormant,    
        Patrolling,
        Chasing
    }

    private State currentState;
    private Transform currentPatrolTarget;

    void Start()
    {
        currentState = State.Dormant;

        if (devilHorns != null)
        {
            devilHorns.SetActive(false);
        }
        
        currentPatrolTarget = patrolPointA;
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Dormant:
                CheckForActivation();
                break;

            case State.Patrolling:
                HandlePatrolling();
                CheckForPlayer();
                break;

            case State.Chasing:
                HandleChasing();
                CheckIfPlayerLost();
                break;
        }
    }

    private void SwitchState(State newState)
    {
        if (currentState == newState) return;

        // When we stop chasing, reset rotation to 0
        if (newState != State.Chasing)
        {
            transform.rotation = Quaternion.identity; // Quaternion.identity is the same as (0,0,0) rotation
        }
        
        currentState = newState;
    }
    
    private void CheckForActivation()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= activationRange)
        {
            Activate();
        }
    }
    
    private void Activate()
    {
        if (devilHorns != null)
        {
            devilHorns.SetActive(true);
        }
        SwitchState(State.Chasing);
    }

    private void HandlePatrolling()
    {
        if (currentPatrolTarget == null) return;
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            currentPatrolTarget = (currentPatrolTarget == patrolPointA) ? patrolPointB : patrolPointA;
        }
    }

    private void HandleChasing()
    {
        if (player == null) return;
        
        // --- Step 1: Handle Movement ---
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);

        // --- Step 2: Handle Rotation with Constraints ---
        Vector2 directionToPlayer = player.position - transform.position;
        float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
        float clampedAngle = Mathf.Clamp(angle, minRotationAngle, maxRotationAngle);
        transform.rotation = Quaternion.Euler(0f, 0f, clampedAngle);
    }

    private void CheckForPlayer()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= activationRange)
        {
            SwitchState(State.Chasing);
        }
    }
    
    private void CheckIfPlayerLost()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > losePlayerRange)
        {
            SwitchState(State.Patrolling);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, losePlayerRange);
    }
}