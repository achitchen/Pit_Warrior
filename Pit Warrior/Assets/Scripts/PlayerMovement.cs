using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float lookDirZ = 0f;
    [SerializeField] int speed = 5;
    [SerializeField] int impactForce = 10;
    public GameObject playerShove;
    [SerializeField] float attackStartDelay = 0.2f;
    [SerializeField] float attackDuration = 0.7f;
    [SerializeField] float shakeDuration = .15f;
    [SerializeField] float shakeMagnitude = .4f;
    [SerializeField] AudioClip bounceSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip[] attackSounds;
    [SerializeField] ParticleSystem footstepsParticles;
    [SerializeField] ParticleSystem bloodParticles;
    [SerializeField] ParticleSystem dustParticles;
    private AudioSource playerSoundSource;
    private bool isMoving = false;
    private bool isLookingLeft = false;
    private bool isLookingRight = false;
    private bool isLookingUp = false;
    private bool isLookingDown = false;
    private bool canAttack = true;
    public bool isAttacking = false;
    public bool isTeleporting = false;
    private bool hasFootsteps = false;
    private GameManager gameManager;
    private UIHandler uIHandler;
    public bool isHit = false;
    public bool isRespawning = false;
    private Vector2 impactDirection;
    public Vector2 movementDir;
    public CameraShake cameraShake;
    private Animator animator;
    private Rigidbody2D playerRb;
    void Start()
    {
        isLookingLeft = false;
        isLookingRight = false;
        isLookingUp = false;
        isLookingDown = false;
        canAttack = true;
        isMoving = false;
        isHit = false;
        isAttacking = false;
        isTeleporting = false;
        lookDirZ = 0f;
        movementDir = Vector2.up;

        playerRb = GetComponent<Rigidbody2D>();

        if (playerShove == null)
        {
            playerShove = transform.Find("Shove").gameObject;
            playerShove.SetActive(false);
        }
        if (playerSoundSource == null)
        {
            playerSoundSource = gameObject.AddComponent<AudioSource>();
            playerSoundSource.loop = false;
        }
        if (cameraShake == null)
        {
            cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uIHandler = GameObject.Find("GameManager").GetComponent<UIHandler>();
    }

    void Update()
    {
        RotatePlayer();
        if (!isHit)
        {
            DetermineDirection();
        }

        //Shove mechanic
        if (canAttack && (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)))
            {
            isAttacking = true;
            StartCoroutine(attackSequence());
            //set attack delay
            StartCoroutine(attackDelay());
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        if (isMoving && !isHit && !isTeleporting)
        {
            if (!hasFootsteps)
            {
                footstepsParticles.Play();
                hasFootsteps = true;
            }
            playerRb.AddForce(movementDir * speed);
        }
        else if (hasFootsteps)
        {
            StopFootsteps();
        }
    }

    private void RotatePlayer()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, lookDirZ);
        if (Input.GetKey(KeyCode.A) && !isLookingRight && !isLookingDown && !isLookingUp)
        {
            lookDirZ = 90f;
            isLookingLeft = true;
        }
        else if (Input.GetKey(KeyCode.W) && !isLookingRight && !isLookingDown && !isLookingLeft)
        {
            lookDirZ = 0f;
            isLookingUp = true;
        }
        else if (Input.GetKey(KeyCode.D) && !isLookingLeft && !isLookingDown && !isLookingUp)
        {
            lookDirZ = 270f;
            isLookingRight = true;
        }
        else if (Input.GetKey(KeyCode.S) && !isLookingRight && !isLookingUp && !isLookingLeft)
        {
            lookDirZ = 180f;
            isLookingDown = true;;
        }
        if (Input.GetKeyUp(KeyCode.A) && isLookingLeft)
        {
            isLookingLeft = false;
        }
        if (Input.GetKeyUp(KeyCode.W) && isLookingUp)
        {
            isLookingUp = false;
        }
        if (Input.GetKeyUp(KeyCode.D) && isLookingRight)
        {
            isLookingRight = false;
        }
        if (Input.GetKeyUp(KeyCode.S) && isLookingDown)
        {
            isLookingDown = false;
        }
    }

    private void DetermineDirection()
    {
        if (isLookingLeft)
        {
            isMoving = true;
            if ((Input.GetKey(KeyCode.W)))
            {
                movementDir = new Vector2(-1, 1);
            }
            else if ((Input.GetKey(KeyCode.S)))
            {
                movementDir = new Vector2(-1, -1);
            }
            else
            {
                movementDir = Vector2.left;
            }
        }
        else if (isLookingRight)
        {
            isMoving = true;
            if ((Input.GetKey(KeyCode.W)))
            {
                movementDir = new Vector2(1, 1);
            }
            else if ((Input.GetKey(KeyCode.S)))
            {
                movementDir = new Vector2(1, -1);
            }
            else
            {
                movementDir = Vector2.right;
            }
        }
        else if (isLookingUp)
        {
            isMoving = true;
            if ((Input.GetKey(KeyCode.D)))
            {
                movementDir = Vector2.one;
            }
            else if ((Input.GetKey(KeyCode.A)))
            {
                movementDir = new Vector2(-1, 1);
            }
            else
            {
                movementDir = Vector2.up;
            }
        }
        else if (isLookingDown)
        {
            isMoving = true;
            if ((Input.GetKey(KeyCode.D)))
            {
                movementDir = new Vector2(1, -1);
            }
            else if ((Input.GetKey(KeyCode.A)))
            {
                movementDir = new Vector2(-1, -1);
            }
            else
            {
                movementDir = Vector2.down;
            }
        }
        else
        {
            isMoving = false;
        }
    }

    IEnumerator attackSequence()
    {
        animator.SetTrigger("playerShoveTrigger");
        yield return new WaitForSeconds(attackStartDelay);
        playerShove.SetActive(true);
        int index = Random.Range(0, attackSounds.Length - 1);
        playerSoundSource.pitch = Random.Range(0.9f, 1.1f);
        playerSoundSource.PlayOneShot(attackSounds[index], 0.5f);
        yield return new WaitForSeconds(attackDuration);
        playerShove.SetActive(false);
        isAttacking = false;
    }

    IEnumerator attackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(1);
        canAttack = true;
    }

    IEnumerator RecoverPlayer()
    {
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
        isHit = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "EnemyAttack")
        {
            if (!isAttacking && !isRespawning)
            {
                animator.SetTrigger("bounceTrigger");
                bloodParticles.Play();
                StopFootsteps();
                StartCoroutine(cameraShake.Shake(shakeDuration, shakeMagnitude));
                playerSoundSource.pitch = Random.Range(0.9f, 1.2f);
                playerSoundSource.PlayOneShot(hitSound, 0.5f);
                canAttack = false;
                isHit = true;
                gameManager.scoreMultiplier = 1;
                uIHandler.multiplierText.text = "";
                impactDirection = transform.position - collision.transform.position;
                playerRb.AddForce(impactDirection * impactForce, ForceMode2D.Impulse);
                StartCoroutine(RecoverPlayer());
            }
        }
        else if (collision.gameObject.tag == "Border")
            {
            if (!isRespawning && !isTeleporting)
                {
                animator.SetTrigger("bounceTrigger");
                collision.gameObject.GetComponent<Animator>().SetTrigger("borderBounceTrigger");
                dustParticles.Play();
                StopFootsteps();
                playerSoundSource.PlayOneShot(bounceSound, 0.6f);
                impactDirection = (transform.position - collision.transform.position);
                playerRb.AddForce(impactDirection * (impactForce / 2), ForceMode2D.Impulse);
                }
            }
    }

    public void StopFootsteps()
    {
        if (hasFootsteps)
        {
            footstepsParticles.Stop();
            hasFootsteps = false;
        }
    }
}
