using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float lookDirZ = 0f;
    [SerializeField] int speed = 5;
    [SerializeField] int launchCharge = 0;
    [SerializeField] int maxLaunchCharge = 5;
    [SerializeField] int launchRechargeDelay = 3;
    [SerializeField] GameObject playerShove;
    [SerializeField] float attackStartDelay = 0.2f;
    [SerializeField] float attackDuration = 0.7f;
    private bool isMoving = false;
    private bool isLookingLeft = false;
    private bool isLookingRight = false;
    private bool isLookingUp = false;
    private bool isLookingDown = false;
    private bool isChargingLaunch = false;
    private bool canLaunch = true;
    private bool canAttack = true;
    public Vector2 movementDir;

    private Rigidbody2D playerRb;
    void Start()
    {
        isLookingLeft = false;
        isLookingRight = false;
        isLookingUp = false;
        isLookingDown = false;
        isChargingLaunch = false;
        canLaunch = true;
        canAttack = true;
        isMoving = false;
        lookDirZ = 0f;
        movementDir = Vector2.up;

        playerRb = GetComponent<Rigidbody2D>();

        if (playerShove == null)
        {
            playerShove = transform.Find("Shove").gameObject;
            playerShove.SetActive(false);
        }
    }

    void Update()
    {
        RotatePlayer();
        LaunchPlayer();
        DetermineDirection();
        //Shove mechanic
        if (canAttack && Input.GetKeyDown(KeyCode.Return))
            {
            StartCoroutine(attackSequence());
            //set attack delay
            StartCoroutine(attackDelay());
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    IEnumerator addLaunchCharge()
    {
        if (isChargingLaunch && launchCharge <= maxLaunchCharge)
        launchCharge++;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(addLaunchCharge());
    }

    private void MovePlayer()
    {
        if (isMoving && !isChargingLaunch)
        {
            playerRb.AddForce(movementDir * speed);
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

    private void LaunchPlayer()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (canLaunch)
            {
                //start charging
                if (!isChargingLaunch)
                {
                    playerRb.velocity = Vector2.zero;
                    playerRb.angularVelocity = 0f;
                    StartCoroutine(addLaunchCharge());
                    isChargingLaunch = true;
                }

                //add impulse based on charge time
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerRb.AddForce(movementDir * launchCharge, ForceMode2D.Impulse);
            isChargingLaunch = false;
            canLaunch = false;
            StartCoroutine(rechargeLaunch());
            launchCharge = 0;
        }
    }

    IEnumerator rechargeLaunch()
    {
        yield return new WaitForSeconds(launchRechargeDelay);
        canLaunch = true;
    }

    IEnumerator attackSequence()
    {
        yield return new WaitForSeconds(attackStartDelay);
        playerShove.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        playerShove.SetActive(false);
    }

    IEnumerator attackDelay()
    {
        canAttack = false;
        yield return new WaitForSeconds(1);
        canAttack = true;
    }
}
