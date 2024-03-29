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
    [SerializeField] AudioClip enemyPottedSound;
    [SerializeField] ParticleSystem enemyPottedParticles;
    [SerializeField] float shakeDuration = 0.2f;
    [SerializeField] float shakeMagnitude = 0.2f;
    private CameraShake cameraShake;
    private GameManager gameManager;
    private UIHandler uIHandler;
    public bool isFilled;
    

    private void Start()
    {
        gameObject.tag = "Untagged";
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uIHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
        socket = transform.Find("Socket").gameObject;
        socket.SetActive(true);
        filledSocket = transform.Find("FilledSocket").gameObject;
        filledSocket.SetActive(false);
        socketRenderer = filledSocket.transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
        isFilled = false;
        if (cameraShake == null)
        {
            cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !isFilled)
        {
            StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
            enemyPottedParticles.Play();
            gameManager.miscSoundsSource.PlayOneShot(enemyPottedSound, 0.9f);
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
            gameObject.tag = "Border";
            crowdSurfLift.GetComponent<CrowdSurfManager>().arenaEnemies.Remove(collision.gameObject);
            socketRenderer.color = enemyRenderer.color;
            collision.gameObject.SetActive(false);
            isFilled = true;
            
        }
    }
}
