using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public bool canTransport = false;
    private Camera gameCamera;
    [SerializeField] Transform nextArenaSpawn;
    [SerializeField] Transform nextCameraSlot;

    void Start()
    {
        gameCamera = FindObjectOfType<Camera>();
}

    // Update is called once per frame

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canTransport)
        {
            Debug.Log("Transporting Player");
            collision.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            collision.GetComponent<Rigidbody2D>().angularVelocity = 0f;
            collision.transform.position = nextArenaSpawn.position;
            gameCamera.transform.position = nextCameraSlot.position;
        }
    }
}
