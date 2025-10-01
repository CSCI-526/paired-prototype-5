using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health settings")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Damage effect")]
    public Color damageColor = new Color(0.5f, 0.5f, 0.5f, 1f); 
    public float damageFlashDuration = 0.2f; 
    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        // initialize the health
        currentHealth = maxHealth;

        // get the SpriteRenderer component attached to the player
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("The SpriteRenderer component is not found on the player object!");
        }
        else
        {
            // save the original color, so that it can be restored
            originalColor = spriteRenderer.color;
        }
    }

    // --- this function needs to be called by the enemy's attack script ---
    public void TakeDamage(int damage)
    {
        // deduct the health
        currentHealth -= damage;
        Debug.Log("The player gets " + damage + " damage, remaining health: " + currentHealth);

        // trigger the damage flash effect
        StartCoroutine(DamageFlashEffect());

        // check if the player is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // --- handles the damage flash effect ---
    private IEnumerator DamageFlashEffect()
    {
        // 1. change the sprite color to the damage color
        spriteRenderer.color = damageColor;

        // 2. wait for the specified duration
        yield return new WaitForSeconds(damageFlashDuration);

        // 3. restore the original color
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        // handle the player death logic
        Debug.Log("The player dies!");
        // temporarily disable the object
        gameObject.SetActive(false);
    }
}