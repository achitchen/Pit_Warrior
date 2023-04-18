using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public bool canTransport = false;
    private GameObject gameCamera;
    [SerializeField] Transform nextArenaSpawn;
    [SerializeField] Transform nextCameraSlot;
    [SerializeField] float arenaTransitionDelay = 1f;
    [SerializeField] AudioClip teleportSound;
    private AudioSource teleportSoundSource;

    void Start()
    {
        gameCamera = GameObject.Find("CameraHolder");
        teleportSoundSource = gameObject.AddComponent<AudioSource>();
        teleportSoundSource.loop = false;
        teleportSoundSource.volume = 1f;
}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canTransport)
        {
            
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            teleportSoundSource.PlayOneShot(teleportSound);
            if (!collision.GetComponent<PlayerMovement>().isTeleporting)
            {
                collision.GetComponent<PlayerMovement>().isTeleporting = true;
                StartCoroutine(TeleportPlayer(collision.gameObject));
            }
            
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        yield return new WaitForSeconds(arenaTransitionDelay);
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        player.transform.position = nextArenaSpawn.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.GetComponent<Rigidbody2D>().angularVelocity = 0f;
        gameCamera.transform.position = nextCameraSlot.position;
        player.GetComponent<PlayerMovement>().isTeleporting = false;
    }
}
