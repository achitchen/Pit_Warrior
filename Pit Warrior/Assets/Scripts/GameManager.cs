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
    public AudioSource musicSource;
    public AudioSource miscSoundsSource;
    [SerializeField] AudioClip bgMusic;

    private void Start()
    {
        score = 0;
        playerLives = 3;
        gameOver = false;
        gamePaused = false;
        gameFinished = false;
        Time.timeScale = 1;
        if (musicSource == null) {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.clip = bgMusic;
            musicSource.volume = 0.6f;
            musicSource.loop = false;
        }
        if (miscSoundsSource == null)
        {
            miscSoundsSource = gameObject.AddComponent<AudioSource>();
            musicSource.volume = 0.7f;
            musicSource.loop = false;
        }
        musicSource.Play();
        StartCoroutine("StartMusic");
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
                StopCoroutine("StartMusic");
                musicSource.Stop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (!gamePaused && !gameOver)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                gamePaused = true;
                Time.timeScale = 0;
                GetComponent<UIHandler>().pausePanel.SetActive(true);
            }
        }
        else if (gamePaused)
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
                StopCoroutine("StartMusic");
                musicSource.Stop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    IEnumerator StartMusic()
    {
        yield return new WaitForSeconds(160);
        musicSource.Play();
        StartCoroutine("StartMusic");
    }
}
