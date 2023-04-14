using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] GameObject lightingPanel;
    [SerializeField] float backgroundLightsTimer = 2f;
    private bool lightsOn = false;
    [SerializeField] Color[] lightingColors;

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
        lightingPanel.SetActive(false);
        StartCoroutine(LightsController());
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
                StopCoroutine(LightsController());
                musicSource.Stop();
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        if (!gamePaused && !gameOver && !gameFinished)
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
                StopCoroutine(LightsController());
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

    IEnumerator LightsController()
    {
        yield return new WaitForSeconds(backgroundLightsTimer);
        if (!lightsOn)
        {
            int index = Random.Range(0, lightingColors.Length - 1);
            lightingPanel.SetActive(true);
            lightingPanel.GetComponent<Image>().color = lightingColors[index];
            lightsOn = true;
        }
        else
        {
            lightingPanel.SetActive(false);
            lightsOn = false;
        }
        StartCoroutine(LightsController());
    }
}
