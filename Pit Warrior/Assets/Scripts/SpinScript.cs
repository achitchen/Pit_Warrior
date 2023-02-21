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
    private Rigidbody2D enemyRb;

    private void Start()
    {
        isHit = false;
        if (enemyRb == null)
        {
            enemyRb = GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (!isHit)
        {
            transform.Rotate(Vector3.forward * spinSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision tag" + collision.gameObject.tag.ToString());
        if (collision.gameObject.tag == "PlayerAttack")
        { 
            isHit = true;
            StartCoroutine(ResumeMovement());
        }
    }

    private IEnumerator ResumeMovement()
    {
        //enemyRb.AddForce(impactDirection * impactForce, ForceMode2D.Force);
        yield return new WaitForSeconds(hitDelay);
        isHit = false;
    }
}
