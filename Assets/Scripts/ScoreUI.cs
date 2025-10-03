using UnityEngine;
using TMPro; // VERY IMPORTANT: You need this line to work with TextMeshPro.

public class ScoreUI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The TextMeshPro UI element that will display the score.")]
    public TextMeshProUGUI scoreText; // A reference to our Text component.

    // Update is called once per frame.
    void Update()
    {
        // Check if the ScoreManager instance and the text component exist.
        if (ScoreManager.instance != null && scoreText != null)
        {
            // Every frame, update the text to show the current score from the ScoreManager.
            // .ToString() converts the integer score to a string.
            scoreText.text = "Score: " + ScoreManager.instance.score.ToString();
        }
    }
}