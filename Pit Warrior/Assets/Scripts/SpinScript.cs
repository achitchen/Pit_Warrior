using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField] int spinSpeed = 30;
    [SerializeField] int hitDelay = 1;
    [SerializeField] float impactForce = 5;
    public bool isHit;
    private Vector2 impactDirection;
    private Vector2 moveDir;
    private Rigidbody2D enemyRb;
    private GameObject enemyShove;
    private GameObject player;

    private void Start()
    {
        isHit = false;
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if (enemyRb == null)
        {
            enemyRb = GetComponent<Rigidbody2D>();
        }
        if (enemyShove == null)
        {
            enemyShove = transform.Find("Shove").gameObject;
        }
        else enemyShove.SetActive(true);
    }

    void Update()
    {
        if (!isHit)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        if (!isHit)
        {
            moveDir = (player.transform.position - transform.position).normalized;
            enemyRb.AddForce(moveDir * spinSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "EnemyAttack")
        {
            impactDirection = (transform.position - collision.transform.position);
            isHit = true;
            enemyShove.SetActive(false);
            StartCoroutine(ResumeMovement());
        }
    }

    private IEnumerator ResumeMovement()
    {
        enemyRb.AddForce(impactDirection * impactForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(hitDelay);
        isHit = false;
        enemyShove.SetActive(true);
    }
}
