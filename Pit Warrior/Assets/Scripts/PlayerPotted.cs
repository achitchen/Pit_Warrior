using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPotted : MonoBehaviour
{
    [SerializeField] int respawnTime = 2;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] AudioClip gameOverSound;
    [SerializeField] AudioClip playerPottedSound;
    [SerializeField] ParticleSystem playerPottedParticles;
    [SerializeField] float shakeDuration = 0.5f;
    [SerializeField] float shakeMagnitude = 0.35f;
    private CameraShake cameraShake;
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
        if (cameraShake == null)
        {
            cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        }
        spawnPosition = spawnPoint.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !GetComponent<EnemyPotted>().isFilled)
        {
            Debug.Log("PlayerPotted");
            playerPottedParticles.Play();
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
            GameObject player = collision.gameObject;
            player.GetComponent<PlayerMovement>().StopAllCoroutines();
            player.GetComponent<PlayerMovement>().isHit = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.GetComponent<Rigidbody2D>().angularVelocity = 0;
            if (gameManager.playerLives > 0)
            {
                gameManager.miscSoundsSource.PlayOneShot(playerPottedSound, 1.7f);
                GetComponent<EnemyPotted>().isFilled = true;
                player.GetComponent<PlayerMovement>().isRespawning = true;
                gameManager.playerLives--;
                gameManager.scoreMultiplier = 1;
                uiHandler.multiplierText.text = "";
                uiHandler.livesText.text = "Teeth remaining: " + gameManager.playerLives.ToString();
                StartCoroutine(PlacePlayer(player));
            }
            else
            {
                gameManager.gameOver = true;
                gameManager.miscSoundsSource.PlayOneShot(gameOverSound, 0.8f);
                uiHandler.gameOverPanel.SetActive(true);
            }
        }
    }

    IEnumerator PlacePlayer(GameObject player)
    {
        yield return new WaitForSeconds(respawnTime);
        player.GetComponent<PlayerMovement>().StartCoroutine("RecoverPlayer");
        player.transform.position = spawnPosition;
        GetComponent<EnemyPotted>().isFilled = false;
        player.GetComponent<PlayerMovement>().isHit = false;
        player.GetComponent<PlayerMovement>().isRespawning = false;
    }
}
