using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    public GameObject gameOverPanel;
    public TMP_Text livesText;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            scoreText.text = "needs to be set";
            livesText.text = "Teeth remaining: " + gameManager.playerLives.ToString();
            gameOverPanel.SetActive(false);

        }
    }
}
