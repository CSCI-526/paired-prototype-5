using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Target and State")]
    public Transform player; 
    public GameObject devilHorns; 

    [Header("Movement Parameters")]
    public float patrolSpeed = 2f; // patrol speed
    public float chaseSpeed = 5f;  // chase speed

    [Header("Detection Range")]
    public float activationRange = 8f; // the distance to activate and start chasing
    public float losePlayerRange = 15f; // the distance to lose the player and switch to patrolling

    [Header("Patrol Points")]
    public Transform patrolPointA; // patrol point A
    public Transform patrolPointB; // patrol point B

    // --- private state variables ---
    private enum State
    {
        Dormant,    
        Patrolling,
        Chasing
    }

    private State currentState;
    private Transform currentPatrolTarget;

    // --- initialization ---
    void Start()
    {
        // at the beginning, the enemy is in the dormant state
        currentState = State.Dormant;

        // ensure the devil horns are hidden at the beginning
        if (devilHorns != null)
        {
            devilHorns.SetActive(false);
        }

        // set the first patrol target, but it will not move at the beginning
        currentPatrolTarget = patrolPointA;
    }

    // --- update every frame ---
    void Update()
    {
        // according to the current state, execute different logic
        switch (currentState)
        {
            case State.Dormant:
                // in the dormant state, only check if it needs to be activated
                CheckForActivation();
                break;

            case State.Patrolling:
                HandlePatrolling();
                CheckForPlayer(); // while patrolling, also check if it can find the player
                break;

            case State.Chasing:
                HandleChasing();
                CheckIfPlayerLost();
                break;
        }
    }

    // --- the core logic of state switching ---
    private void SwitchState(State newState)
    {
        if (currentState == newState) return; // if the state is not changed, do nothing

        currentState = newState;
    }
    
    // --- check if it needs to be activated ---
    private void CheckForActivation()
    {
        if (player == null) return;

        // calculate the distance to the player
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // if the player is in the activation range, permanently activate and switch to the chasing state
        if (distanceToPlayer <= activationRange)
        {
            Activate();
        }
    }
    
    // --- the logic of permanently activating ---
    private void Activate()
    {
        // show the devil horns
        if (devilHorns != null)
        {
            devilHorns.SetActive(true);
        }
        
        // switch to the chasing state
        SwitchState(State.Chasing);
    }

    // --- the logic of patrolling ---
    private void HandlePatrolling()
    {
        if (currentPatrolTarget == null) return;
        transform.position = Vector2.MoveTowards(transform.position, currentPatrolTarget.position, patrolSpeed * Time.deltaTime);

        if (Vector2.Distance(transform.position, currentPatrolTarget.position) < 0.1f)
        {
            currentPatrolTarget = (currentPatrolTarget == patrolPointA) ? patrolPointB : patrolPointA;
        }
    }

    // --- the logic of chasing ---
    private void HandleChasing()
    {
        if (player == null) return;
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
    }

    // --- check if it can find the player ---
    private void CheckForPlayer()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // here use the activationRange, which means once activated, its detection range is this value
        if (distanceToPlayer <= activationRange)
        {
            SwitchState(State.Chasing);
        }
    }

    // --- check if the player is lost ---
    private void CheckIfPlayerLost()
    {
        if (player == null) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > losePlayerRange)
        {
            SwitchState(State.Patrolling);
        }
    }
    
    // (optional) draw the detection range in the Scene view, for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, activationRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, losePlayerRange);
    }
}