using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("health setting")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("damage effect")]
    public Color damageColor = new Color(0.5f, 0.5f, 0.5f, 1f);
    public float damageFlashDuration = 0.2f;
    

    
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void TakeDamage(int damage)
    {
        // Don't take damage if health is already zero or less
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " get " + damage + " damage, remaining health: " + currentHealth);
        StartCoroutine(DamageFlashEffect());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator DamageFlashEffect()
    {
        spriteRenderer.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        spriteRenderer.color = originalColor;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " dies!");



        if (GameManager.instance != null)
        {
            GameManager.instance.OnPlayerDied(this);
        }

        

        gameObject.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}