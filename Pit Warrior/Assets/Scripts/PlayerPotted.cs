using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPotted : MonoBehaviour
{
    [SerializeField] int respawnTime = 2;
    [SerializeField] GameObject spawnPoint;
    private Vector3 spawnPosition;
    private GameManager gameManager;
    private UIHandler uiHandler;
    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            uiHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
        }
        spawnPosition = spawnPoint.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !GetComponent<EnemyPotted>().isFilled)
        {
            Debug.Log("PlayerPotted");
            GameObject player = collision.gameObject;
            if (gameManager.playerLives > 0)
            {
                GetComponent<EnemyPotted>().isFilled = true;
                player.GetComponent<PlayerMovement>().isHit = true;
                player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                player.GetComponent<Rigidbody2D>().angularVelocity = 0;
                gameManager.playerLives--;
                gameManager.scoreMultiplier = 1;
                uiHandler.multiplierText.text = "";
                uiHandler.livesText.text = "Teeth remaining: " + gameManager.playerLives.ToString();
                StartCoroutine(PlacePlayer(player));
            }
            else
            {
                gameManager.gameOver = true;
                uiHandler.gameOverPanel.SetActive(true);
            }
        }
    }

    IEnumerator PlacePlayer(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);
        player.transform.position = spawnPosition;
        GetComponent<EnemyPotted>().isFilled = false;
        player.GetComponent<PlayerMovement>().isHit = false;
    }
}
