using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text livesText;
    public TMP_Text multiplierText;
    public GameObject pausePanel;
    public GameObject gameFinishedPanel;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        scoreText.text = "Score: " + 0;
        livesText.text = "Teeth remaining: " + gameManager.playerLives.ToString();
        multiplierText.text = "";
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
        gameFinishedPanel.SetActive(false);
    }
}
