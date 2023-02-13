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
    private bool isLookingLeft = false;
    private bool isLookingRight = false;
    private bool isLookingUp = false;
    private bool isLookingDown = false;
    private bool isChargingLaunch = false;
    public Vector2 movementDir;

    private Rigidbody2D playerRb;
    // Start is called before the first frame update
    void Start()
    {
        isLookingLeft = false;
        isLookingRight = false;
        isLookingUp = false;
        isLookingDown = false;
        isChargingLaunch = false;
        lookDirZ = 0f;
        movementDir = Vector2.zero;

        playerRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        if (Input.GetKey(KeyCode.Space))
        {
            //remove forces
            playerRb.velocity = Vector2.zero;
            playerRb.angularVelocity = 0f;
            //start charging
            if (!isChargingLaunch)
            {
                StartCoroutine(addLaunchCharge());
                isChargingLaunch = true;
            }

            //add impulse based on charge time
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            playerRb.AddForce(movementDir * launchCharge, ForceMode2D.Impulse);
            isChargingLaunch = false;
            launchCharge = 0;
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
        //transform.Translate(movementDir * speed * Time.deltaTime)
        if (!isChargingLaunch)
        {
            playerRb.AddForce(movementDir * speed);
        }

        if (isLookingLeft)
        {
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
            if ((Input.GetKey(KeyCode.W)))
            {
                movementDir = new Vector2(1, 1);
            }
            else if ((Input.GetKey(KeyCode.S)))
            {
                movementDir = new Vector2 (1,-1);
            }
            else
            {
                movementDir = Vector2.right;
            }
        }
        else if (isLookingUp)
        {
            if ((Input.GetKey(KeyCode.D)))
            {
                movementDir = Vector2.one;
            }
            else if ((Input.GetKey(KeyCode.A)))
            {
                movementDir = new Vector2(-1,1);
            }
            else
            {
                movementDir = Vector2.up;
            }
        }
        else if (isLookingDown)
        {
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
            isLookingDown = true;
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
}
