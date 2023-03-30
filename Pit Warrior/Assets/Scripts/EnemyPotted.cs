using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPotted : MonoBehaviour
{
    private SpriteRenderer socketRenderer;
    private SpriteRenderer enemyRenderer;
    private GameObject socket;
    private GameObject filledSocket;
    [SerializeField] GameObject crowdSurfLift;
    private GameManager gameManager;
    private UIHandler uIHandler;
    public bool isFilled;
    

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uIHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
        socket = transform.Find("Socket").gameObject;
        socket.SetActive(true);
        filledSocket = transform.Find("FilledSocket").gameObject;
        filledSocket.SetActive(false);
        socketRenderer = filledSocket.transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
        isFilled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isFilled)
        {
            gameManager.score += 100 * gameManager.scoreMultiplier;
            uIHandler.scoreText.text = "Score: " + gameManager.score;
            if (gameManager.scoreMultiplier < 5)
            {
                gameManager.scoreMultiplier++;
            }
                uIHandler.multiplierText.text = "x" + gameManager.scoreMultiplier;
            enemyRenderer = collision.transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
            socket.SetActive(false);
            filledSocket.SetActive(true);
            crowdSurfLift.GetComponent<CrowdSurfManager>().arenaEnemies.Remove(collision.gameObject);
            socketRenderer.color = enemyRenderer.color;
            collision.gameObject.SetActive(false);
            isFilled = true;
            
        }
    }
}
