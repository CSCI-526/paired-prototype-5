using UnityEngine;

public class ScytheDamageDealer : MonoBehaviour
{
    public int damageAmount = 20; // you can set the damage of the scythe here

    // when other collider enters this trigger, this function is called
    private void OnTriggerEnter2D(Collider2D other)
    {
        // check if the object is the player
        if (other.CompareTag("Player"))
        {
            Debug.Log("The scythe blade hits the player!");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);

                // to prevent multiple hits, you can disable yourself after hitting
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}