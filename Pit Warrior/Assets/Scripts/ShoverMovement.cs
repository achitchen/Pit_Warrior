using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoverMovement : MonoBehaviour
{
    public GameObject player;
    private Rigidbody2D enemyRb;
    Vector2 moveDir;

    [SerializeField] int enemySpeed = 5;
    [SerializeField] int attackWindup = 1;
    [SerializeField] int attackDelay = 3;
    [SerializeField] int hitDelay = 1;
    [SerializeField] int impactForce = 10;
    [SerializeField] AudioClip[] hitSounds;
    [SerializeField] AudioClip enemyAttackSound;
    private AudioSource enemySoundSource;
    private AudioSource bleghSoundSource;
    private Vector2 impactDirection;
    private float lookDirZ = 0f;
    private GameObject shoverShove;
    public bool canShove = true;
    void Start()
    {
        Activate();
    }

    private void Update()
    {
        RotateShover();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "PlayerAttack" || collision.gameObject.tag == "EnemyAttack")
        {
            enemySoundSource.pitch = Random.Range(0.9f, 1.1f);
            int index = Random.Range(0, 1);
            enemySoundSource.PlayOneShot(hitSounds[index], 1.4f);
            impactDirection = (transform.position - collision.transform.position);
            StartCoroutine("GetHit");
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
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        Vector2 playerDirection = (player.transform.position - transform.position).normalized;
        return playerDirection;
    }

    public IEnumerator launchAtPlayer()
    {
        moveDir = getMoveDir();
        bleghSoundSource.pitch = Random.Range(0.9f, 1.1f);
        bleghSoundSource.Play();
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
        bleghSoundSource.Stop();
        enemyRb.AddForce(impactDirection * impactForce, ForceMode2D.Impulse);
        shoverShove.SetActive(false);
        yield return new WaitForSeconds(hitDelay);
        canShove = true;
        StartCoroutine("launchAtPlayer");

    }

    public void Activate()
    {
        if (player == null)
        {
            player = GameObject.Find("Player");
        }
        if (shoverShove == null)
        {
            shoverShove = transform.Find("Shove").gameObject;
        }
        if (enemySoundSource == null)
        {
            enemySoundSource = gameObject.AddComponent<AudioSource>();
            enemySoundSource.loop = false;
            enemySoundSource.volume = 0.7f;
        }
        if (bleghSoundSource == null)
        {
            bleghSoundSource = gameObject.AddComponent<AudioSource>();
            bleghSoundSource.loop = false;
            bleghSoundSource.volume = 0.7f;
            bleghSoundSource.clip = enemyAttackSound;
        }
        shoverShove.SetActive(false);
        moveDir = getMoveDir();
        enemyRb = GetComponent<Rigidbody2D>();
        lookDirZ = 0f;
        canShove = true;

        StartCoroutine("launchAtPlayer");
    }
}
