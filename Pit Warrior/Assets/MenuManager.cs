using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject introPanel;
    [SerializeField] GameObject verticalSlicePanel;
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip menuMusicSound;
    private AudioSource menuSoundsSource;
    private AudioSource menuMusicSource;

    private void Start()
    {
        menuSoundsSource = gameObject.AddComponent<AudioSource>();
        menuSoundsSource.volume = 1;
        menuSoundsSource.loop = false;
        menuSoundsSource.clip = buttonSound;
        menuMusicSource = gameObject.AddComponent<AudioSource>();
        menuMusicSource.volume = 0.5f;
        menuMusicSource.loop = true;
        menuMusicSource.clip = menuMusicSound;
        startPanel.SetActive(true);
        introPanel.SetActive(false);
        menuMusicSource.Play();
    }
    public void StartGame()
    {
        menuSoundsSource.Play();
        startPanel.SetActive(false);
        introPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void IntroButton()
    {
        menuSoundsSource.Play();
        introPanel.SetActive(false);
        verticalSlicePanel.SetActive(true);
    }

    public void ReadyButton()
    {
        menuSoundsSource.Play();
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        yield return new WaitForSeconds(2);
        menuMusicSource.Stop();
        SceneManager.LoadScene("Game");
    }
}
