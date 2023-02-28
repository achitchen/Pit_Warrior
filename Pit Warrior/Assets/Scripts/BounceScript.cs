using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    [SerializeField] int impactForce = 30;
    private Vector2 impactDirection;
    private Rigidbody2D enemyRb;
    private bool isHit;

    private void Start()
    {
        if (enemyRb ==  null)
        {
            enemyRb = this.GetComponent<Rigidbody2D>();
        }
        isHit = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            impactDirection = (transform.position - collision.transform.position);
            enemyRb.AddForce(impactDirection * impactForce, ForceMode2D.Impulse);
            if (GetComponent<ShoverMovement>() != null)
            {
                ShoverMovement shoverMovement = GetComponent<ShoverMovement>();
                if (!isHit)
                {
                    StartCoroutine(shoverMovement.GetHit());
                    isHit = true;
                    StartCoroutine("RecoverHitBool");
                }
                else if (isHit)
                {
                    StopCoroutine("RecoverHitBool");
                    StartCoroutine("RecoverHitBool");
                }
            }
        }
    }

    IEnumerator RecoverHitBool()
    {
        yield return new WaitForSeconds(0.5f);
        isHit = false;
    }

}
