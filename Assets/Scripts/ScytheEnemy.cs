using UnityEngine;

// This script defines the behavior for an enemy that attacks the player 
// when they enter a specific range.
public class ScytheEnemy : MonoBehaviour
{
    [Header("AI settings")]
    // Reference to the player's Transform, used to track their position.
    public Transform player; 
    
    // The distance within which the enemy will trigger an attack.
    public float attackRange = 2f; 
    // The minimum time (in seconds) between consecutive attacks.
    public float attackCooldown = 2f; 
    
    private Animator animator; 
    
    // A timestamp of when the last attack occurred, used for the cooldown logic.
    private float lastAttackTime; 


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Safety check: If no player is assigned in the Inspector, do nothing.
        if (player == null) return;

        // Calculate the current distance between the enemy and the player.
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        

        if (distanceToPlayer <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    // This method contains the logic for performing an attack.
    private void Attack()
    {
        lastAttackTime = Time.time;
        
        // Trigger the "AttackTrigger" parameter in the Animator Controller. 
        // This should be connected to a transition that plays the attack animation.
        animator.SetTrigger("AttackTrigger");
        
        Debug.Log("The enemy animation is triggered");
    }
    
    // This is a special Unity function that draws gizmos in the Scene view when the object is selected.
    // It's very useful for debugging and visualizing ranges.
    private void OnDrawGizmosSelected()
    {
        // Set the color of the gizmo to red.
        Gizmos.color = Color.red;
        
        // Draw a red wireframe circle around the enemy to visually represent the attackRange.
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}