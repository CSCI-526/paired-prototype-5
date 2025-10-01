using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    [Header("Damage settings")]
    public int damageAmount = 10;           // the damage amount when the player contacts the enemy
    public float damageCooldown = 1.0f;     

    private float lastDamageTime;           // to record the time when the last damage is caused


    private void OnCollisionStay2D(Collision2D collision)
    {
        // check if the object is the player (through Tag)
        if (collision.gameObject.CompareTag("Player"))
        {
            // check if the cooldown time has passed
            if (Time.time >= lastDamageTime + damageCooldown)
            {
                Debug.Log("The player contacts the enemy, causing damage!");

                // try to get the PlayerHealth script from the player object
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

                if (playerHealth != null)
                {
                    // cause damage to the player
                    playerHealth.TakeDamage(damageAmount);

                    // update the time when the last damage is caused to the current time
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}