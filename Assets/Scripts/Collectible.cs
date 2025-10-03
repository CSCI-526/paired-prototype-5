using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("The amount of score this collectible is worth.")]
    public int scoreValue = 100;

    // Optional: Effects to play when picked up
    [Tooltip("Particle effect to spawn when collected.")]
    public GameObject pickupEffect;
    
    [Tooltip("Sound to play when collected.")]
    public AudioClip pickupSound;


    // This function is called when another object enters a trigger collider attached to this object.
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger has the "Player" tag.
        if (other.CompareTag("Player"))
        {
            // If it's the player, we perform the collection logic.
            Collect();
        }
    }

    private void Collect()
    {
        // Use the ScoreManager singleton to add score.
        // This is clean because the collectible doesn't need a direct reference to the player.
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(scoreValue);
        }

        // --- Optional Visual/Audio Feedback ---
        // If a pickup sound is assigned, play it at the collectible's position.
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        // If a pickup effect (like a particle system) is assigned, spawn it.
        if (pickupEffect != null)
        {
            Instantiate(pickupEffect, transform.position, Quaternion.identity);
        }
        
        // After everything is done, destroy the collectible object.
        Destroy(gameObject);
    }
}