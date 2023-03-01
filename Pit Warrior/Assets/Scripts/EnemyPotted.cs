using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPotted : MonoBehaviour
{
    private SpriteRenderer socketRenderer;
    private SpriteRenderer enemyRenderer;
    private GameObject socket;
    private GameObject filledSocket;
    public bool isFilled;

    private void Start()
    {
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
            enemyRenderer = collision.transform.Find("Body").gameObject.GetComponent<SpriteRenderer>();
            socket.SetActive(false);
            filledSocket.SetActive(true);
            socketRenderer.color = enemyRenderer.color;
            collision.gameObject.SetActive(false);
            isFilled = true;
        }
    }
}
