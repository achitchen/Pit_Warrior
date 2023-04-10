using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceScript : MonoBehaviour
{
    [SerializeField] int impactForce = 30;
    [SerializeField] AudioClip bounceSound;
    private Vector2 impactDirection;
    private Rigidbody2D enemyRb;
    private GameManager gameManager;
    private bool isHit;

    private void Start()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

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
            gameManager.miscSoundsSource.PlayOneShot(bounceSound, 0.6f);
            impactDirection = (transform.position - collision.transform.position);
            enemyRb.AddForce(impactDirection * impactForce, ForceMode2D.Impulse);
            if (GetComponent<ShoverMovement>() != null)
            {
                ShoverMovement shoverMovement = GetComponent<ShoverMovement>();
                if (!isHit)
                {
                    shoverMovement.canShove = false;
                    shoverMovement.StopCoroutine("launchAtPlayer");
                    shoverMovement.StartCoroutine("GetHit");
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
