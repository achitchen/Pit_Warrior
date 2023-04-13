using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinScript : MonoBehaviour
{
    [SerializeField] int spinSpeed = 30;
    [SerializeField] int hitDelay = 1;
    [SerializeField] float impactForce = 5;
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] ParticleSystem bloodParticles;
    public bool isHit;
    private Vector2 impactDirection;
    private Vector2 moveDir;
    private Rigidbody2D enemyRb;
    private GameObject enemyShove;
    private GameObject player;
    private AudioSource enemySoundSource;

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
        if (enemySoundSource == null)
        {
            enemySoundSource = gameObject.AddComponent<AudioSource>();
            enemySoundSource.loop = false;
            enemySoundSource.volume = 0.7f;
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
            enemySoundSource.pitch = Random.Range(0.7f, 1.2f);
            bloodParticles.Play();
            int index = Random.Range(0, 1);
            enemySoundSource.PlayOneShot(hitSounds[index], 1.4f);
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
