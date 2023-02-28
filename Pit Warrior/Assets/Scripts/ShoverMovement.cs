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
    [SerializeField] int hitDelay = 1;
    [SerializeField] int impactForce = 10;

    private Vector2 impactDirection;
    private float lookDirZ = 0f;
    private GameObject shoverShove;
    public bool canShove = true;
    void Start()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if (shoverShove == null)
        {
            shoverShove = transform.Find("Shove").gameObject;
        }
        shoverShove.SetActive(false);
        moveDir = getMoveDir();
        enemyRb = GetComponent<Rigidbody2D>();
        lookDirZ = 0f;
        canShove = true;
        StartCoroutine("launchAtPlayer");
    }

    private void Update()
    {
        RotateShover();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack")
        {
            impactDirection = (transform.position - collision.transform.position);
            StartCoroutine(GetHit());
            canShove = false;
        }
    }

    private void RotateShover()
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
        if (canShove)
        {
            enemyRb.AddForce(moveDir * enemySpeed, ForceMode2D.Impulse);
            shoverShove.SetActive(true);
        }
        yield return new WaitForSeconds(attackDelay);
        shoverShove.SetActive(false);
        StartCoroutine("launchAtPlayer");
    }

    public IEnumerator GetHit()
    {
        StopCoroutine("launchAtPlayer");
        enemyRb.AddForce(impactDirection * impactForce, ForceMode2D.Impulse);
        shoverShove.SetActive(false);
        yield return new WaitForSeconds(hitDelay);
        canShove = true;
        StartCoroutine("launchAtPlayer");

    }
}
