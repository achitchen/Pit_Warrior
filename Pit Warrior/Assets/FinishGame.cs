using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    private GameManager gameManager;
    private UIHandler uIHandler;
    
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uIHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            gameManager.gameFinished = true;
            uIHandler.gameFinishedPanel.SetActive(true);
        }
    }
}
