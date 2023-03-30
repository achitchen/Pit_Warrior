using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int playerLives = 3;
    public int score = 0;
    public int scoreMultiplier = 1;
    public bool gameOver = false;
    public bool gamePaused = false;
    public bool gameFinished = false;

    private void Start()
    {
        score = 0;
        playerLives = 3;
        gameOver = false;
        gamePaused = false;
        gameFinished = false;
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (gameOver)
        {
            Time.timeScale = 0;
            if (Input.GetKey(KeyCode.Return))
            {
                Time.timeScale = 1;
                GetComponent<UIHandler>().gameOverPanel.SetActive(false);
                playerLives = 3;
                score = 0;
                gameOver = false;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (!gamePaused)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                gamePaused = true;
                Time.timeScale = 0;
                GetComponent<UIHandler>().pausePanel.SetActive(true);
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                gamePaused = false;
                Time.timeScale = 1;
                GetComponent<UIHandler>().pausePanel.SetActive(false);
                GetComponent<UIHandler>().gameFinishedPanel.SetActive(false);
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (gameFinished)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                gameFinished = false;
                Time.timeScale = 1;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
