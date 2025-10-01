using UnityEngine;

public class JellyProjectile : MonoBehaviour
{
    // ... (speed, damage, lifeTime variables remain the same) ...
    public float speed = 10f;
    public int damage = 15;
    public float lifeTime = 5f;
    
    private Rigidbody2D rb;
    // We will no longer find the player here. The Boss will tell us who to target.
    // private Transform player; 

    // We change Start() to a public method so the Boss can call it and pass the target.
    public void SetTarget(Transform target)
    {
        rb = GetComponent<Rigidbody2D>();
        
        if (target != null)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;
        }

        Destroy(gameObject, lifeTime);
    }

    // OnTriggerEnter2D remains exactly the same.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}