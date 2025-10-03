using UnityEngine;
using System.Collections.Generic;

public class Goal : MonoBehaviour
{
    [Header("UI & Animation")]
    [Tooltip("The 'Success' UI panel to show upon winning.")]
    public GameObject successPanel;
    [Tooltip("The Animator component on the Flag child object.")]
    public Animator flagAnimator;

    [Header("Win Condition")]
    [Tooltip("How many players are required to be in the zone to win.")]
    public int requiredPlayers = 2;

    private List<GameObject> playersInZone = new List<GameObject>();
    private bool isActivated = false;

    void Start()
    {
        // Make sure the success panel is hidden at the start.
        if (successPanel != null)
        {
            successPanel.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("!!! GOAL TRIGGERED at Frame: " + Time.frameCount + " !!! by " + other.name);
        if (isActivated) return;

        if (other.CompareTag("Player") && !playersInZone.Contains(other.gameObject))
        {
            playersInZone.Add(other.gameObject);
            Debug.Log(other.name + " entered the goal. Players in zone: " + playersInZone.Count);

            if (playersInZone.Count >= requiredPlayers)
            {
                TriggerWinState();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playersInZone.Contains(other.gameObject))
        {
            playersInZone.Remove(other.gameObject);
            Debug.Log(other.name + " left the goal. Players in zone: " + playersInZone.Count);
        }
    }

    private void TriggerWinState()
    {
        isActivated = true;
        Debug.Log("Goal activated! Showing success UI and raising flag.");

        if (flagAnimator != null)
        {
            flagAnimator.SetTrigger("RaiseFlag");
        }

        if (successPanel != null)
        {
            successPanel.SetActive(true);
        }
    }
}