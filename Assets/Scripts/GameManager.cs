using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("UI Panels")]
    public GameObject successPanel;
    public GameObject gameOverPanel;

    [Header("Player Tracking")]
    public List<PlayerHealth> players;
    
    private bool isGameOver = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if(successPanel != null) successPanel.SetActive(false);
        if(gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void ShowSuccessPanel()
    {
        if (isGameOver) return;
        isGameOver = true;
        if(successPanel != null) successPanel.SetActive(true);
    }
    
    public void OnPlayerDied(PlayerHealth deadPlayer)
    {
        if (isGameOver) return;

        int alivePlayers = 0;
        foreach (PlayerHealth player in players)
        {
            if (player != null && player.gameObject.activeInHierarchy)
            {
                alivePlayers++;
            }
        }
        
        Debug.Log("A player died notification was received. Calculating active players... Found: " + alivePlayers);

        // For a 2-player game, if this method is called and the number of active players is 1,
        // it means this is the LAST player dying.
        if (alivePlayers <= 1)
        {
            isGameOver = true;
            Debug.Log("All players are dead. GAME OVER.");
            if(gameOverPanel != null) gameOverPanel.SetActive(true);
        }
    }
    
    public void RestartGame()
    {
        // 发射一颗“曳光弹”，看看我们是否进入了这个函数
        Debug.Log("!!! RESTART BUTTON CLICKED, attempting to reload scene !!!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}