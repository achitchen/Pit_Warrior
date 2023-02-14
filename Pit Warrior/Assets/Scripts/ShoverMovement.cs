using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoverMovement : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D enemyRb;
    Vector2 moveDir;

    [SerializeField] int enemySpeed = 5;
    [SerializeField] int attackWindup = 1;
    [SerializeField] int attackDelay = 3;
    private float lookDirZ = 0f;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        moveDir = getMoveDir();
        enemyRb = GetComponent<Rigidbody2D>();
        lookDirZ = 0f;
        StartCoroutine(launchAtPlayer());
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, lookDirZ);
        if (moveDir.x >= 0 && moveDir.y >= 0 && moveDir.x > moveDir.y)
        {
            lookDirZ = 270f;
        }
        else if (moveDir.x <= 0 && moveDir.y >= 0 && (-moveDir.x > moveDir.y))
        {
            lookDirZ = 90f;
        }
        else if (moveDir.x >= 0 && moveDir.y <= 0 && (moveDir.x > -moveDir.y))
        {
            lookDirZ = 270f;
        }
        else if (moveDir.x <= 0 && moveDir.y <= 0 && (moveDir.x < moveDir.y))
        {
            lookDirZ = 90f;
        }
        else if (moveDir.y >= 0 && moveDir.x >= 0 && moveDir.y > moveDir.x)
        {
            lookDirZ = 0f;
        }
        else if (moveDir.y <= 0 && moveDir.x >= 0 && (-moveDir.y > moveDir.x))
        {
            lookDirZ = 180f;
        }
        else if (moveDir.y >= 0 && moveDir.x <= 0 && (moveDir.y > -moveDir.x))
        {
            lookDirZ = 0f;
        }
        else if (moveDir.y <= 0 && moveDir.x <= 0 && (moveDir.y < moveDir.x))
        {
            lookDirZ = 180f;
        }
    }

    private Vector2 getMoveDir()
    {
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        return playerDirection;
    }

    IEnumerator launchAtPlayer()
    {
        moveDir = getMoveDir();
        yield return new WaitForSeconds(attackWindup);
        enemyRb.AddForce(moveDir * enemySpeed, ForceMode2D.Impulse);
        yield return new WaitForSeconds(attackDelay);
        StartCoroutine(launchAtPlayer());
    }
}
