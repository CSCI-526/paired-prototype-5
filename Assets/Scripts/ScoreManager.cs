using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // This is the static instance of the ScoreManager that can be accessed from anywhere.
    public static ScoreManager instance;

    // The current score.
    public int score = 0;

    // Awake is called when the script instance is being loaded.
    private void Awake()
    {
        // This is the Singleton pattern. It ensures that there's only ever one instance of ScoreManager.
        if (instance == null)
        {
            // If no instance exists yet, this one becomes the instance.
            instance = this;
        }
        else
        {
            // If an instance already exists, destroy this duplicate.
            Destroy(gameObject);
        }
    }

    // A public method that other scripts can call to add points to the score.
    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Score is now: " + score); // For debugging, prints the new score to the console.
    }
}