using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{

    public timeManager pp;
    bool stopTime = false;


    [Space(10)]
    [Header("Stats")]
    public float Health = 0f;
    public float maxHealth = 100f;

    [Space(10)]
    [Header("Movement")]
    public float moveSpeed;
    public float currentSpeed;
    public float maxSpeed = 10f;
    public float sprintMultiplier = 1.5f;
    public float counterMovement = 15f;
    public float threshold = 0.01f;
    public bool isCrouched = false;
    public bool sprinting = false;
    public float crouchMaxSpeed = 7f;
    float xInput, yInput;
    bool crouched = false;
    float crouchedScale;
    float unCrouchedScale;
    float sprintSpeed;
    float originalMaxSpeed;
    

    [Space(10)]
    [Header("Sliding values")]
    public bool isSliding = false;
    bool slide = false;
    public float slideForce = 200f;
    public float slideCounterMovement = 15f;
    public LayerMask slideableGround;

    [Space(10)]
    [Header("Momentum values")]
    public float momentumSpeedMultiplier = 2.5f;
    public float momentumTimer = 0f;
    public float momentumTimerMax = 10f;
    public float momentumMultiplier = 1f;
    public float momentumDeductionMultiplier = 0.8f;

    [Space(10)]
    [Header("Jumping")]
    public float jumpForce;
    bool jumping = false;
    public LayerMask Ground;
    public bool isGrounded;    
    public float groundDistance = 0.4f;
    
    [Space(10)]
    [Header("Wall Running")]
    public float wallrunForce, maxWallRunTime;
    bool isWallRight, isWallLeft;
    public bool isWallRunning;
    public float maxWallrunCameraTilt, wallrunCameraTilt;
    public float momontumSpeed;
    public LayerMask wallrunWalls;
    public bool decendWhileWallRunning;

    [Space(10)]
    [Header("camera")]
    public float sensitivity = 10f;
    float xCam = 0;
    public Transform cam;

    [Space(10)]
    [Header("Other")]
    public Rigidbody rb;
    public Transform groundCheck, orientation;
    public UIController UIScript;

    float timerForYValue = 0;
    float Ycheck = 0;

    void Start()
    {
        UIScript = GetComponent<UIController>();
        momontumSpeed = maxSpeed * momentumSpeedMultiplier;
        sprintSpeed = sprintMultiplier * maxSpeed;
        originalMaxSpeed = maxSpeed;
        Ground = LayerMask.GetMask("Ground");
        wallrunWalls = LayerMask.GetMask("Wallrun-able");
        slideableGround = LayerMask.GetMask("Slide-able");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        unCrouchedScale = transform.localScale.y;
        crouchedScale = transform.localScale.y - 0.5f;
    }

    private void FixedUpdate()
    {
        movement();
    }

    void Update()
    {
        timerForYValue -= Time.deltaTime;
        if (timerForYValue <= 0f)
        {
            Ycheck = rb.position.y;
            timerForYValue = 1f;
        }
        currentSpeed = rb.velocity.magnitude;
        UIScript.updateCurrentSpeed(currentSpeed);
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, Ground) || Physics.CheckSphere(groundCheck.position, groundDistance, slideableGround);

        deductMomentum(Time.deltaTime);
        input();
        cameraLook();
        checkForWall();
        wallrunInput();

    }

    void input()
    {
        if (Input.GetKeyDown(KeyCode.Q)) stopTime = true;
        if (Input.GetKeyDown(KeyCode.E)) {
            pp.resumeTime(); 
            stopTime = false;
        }

        if (stopTime)
        {
            pp.stopTime();
        }
        slide = (Input.GetKey(KeyCode.LeftControl) && isGrounded && !isCrouched & !isSliding);
        decendWhileWallRunning = (Input.GetKey(KeyCode.LeftControl) && isWallRunning);
        jumping = Input.GetKey(KeyCode.Space);
        sprinting = Input.GetKey(KeyCode.LeftShift);
        xInput = Input.GetAxisRaw("Horizontal") * moveSpeed;
        yInput = Input.GetAxisRaw("Vertical") * moveSpeed;

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouched = !crouched;
        }
    }
    void addMomentum(float addedNumber)
    {
        if(momentumTimer < momentumTimerMax)
        {
            momentumTimer += addedNumber * momentumMultiplier;
        }
        else
        {
            momentumTimer = momentumTimerMax;
        }

        UIScript.updateAdreanline(momentumTimer);
    }

    void deductMomentum(float addedNumber)
    {
        if (momentumTimer > 0f)
        {
            if (!isSliding && !isWallRunning) momentumTimer -= addedNumber * momentumDeductionMultiplier;
            maxSpeed = momontumSpeed;
        }
        else if(momentumTimer <= 0f && !sprinting)
        {
            maxSpeed = originalMaxSpeed;
        }
        if (momentumTimer < 0f) momentumTimer = 0f;

        UIScript.updateAdreanline(momentumTimer);
    }
    void crouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, crouchedScale, transform.localScale.z);
        isCrouched = true;
    }

    void uncrouch()
    {
        transform.localScale = new Vector3(transform.localScale.x, unCrouchedScale, transform.localScale.z);
        isCrouched = false;
    }

    void startSlide()
    {
       if (Physics.Raycast(transform.position, -orientation.up, 3f, slideableGround))
        {
            if(Ycheck + 1 > rb.position.y)
            {
                maxSpeed = maxSpeed * 3;
                addMomentum(Time.deltaTime * 3);
                return;
            }
        }

        if (!isCrouched && !isSliding)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchedScale, transform.localScale.z);

            rb.AddForce(orientation.transform.forward * slideForce * rb.velocity.magnitude);
            isSliding = true;
            //Debug.Log("wee9");
        }

    }
    void endSilde()
    {
        isSliding = false;
    }



    void jump()
    {
       

        if (jumping && isGrounded) // normal jump
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            crouched = false;
        }

    }
    void movement()
    {   
        if (isWallRunning) return;

        jump();

        

        if (isSliding)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            addMomentum(Time.deltaTime);
            //Debug.Log("wee");
            if (!isGrounded || rb.velocity.magnitude <= 2f) 
            {
                isSliding = false;
                isCrouched = true;
                crouched = true;

            }

            return;
        }
        if (slide)
        {
            startSlide();
           // Debug.Log("start slide remain : " + delay + slide);
        }
        else
        {
                endSilde();
        }

        if (crouched && isGrounded)
        {
            crouch();
        }
        else if(!isSliding)
        {
            uncrouch();
        }

        float movementRestriction = 1f;
        float inAir = 1f;

        

        Vector3 move = transform.right * xInput + transform.forward * yInput;

        Vector2 movespeed = findMovementSpeed();
        float yMoveSpeed = movespeed.y, xMoveSpeed = movespeed.x;

        counteredMovement(movespeed);
        checkIfSprinting();

        if (isCrouched)
        {
            maxSpeed = crouchMaxSpeed;
        }

        //if ((yMoveSpeed < -maxSpeed) || (yMoveSpeed > maxSpeed) || (xMoveSpeed < -maxSpeed) || (xMoveSpeed > maxSpeed)) overspeed = true;
        //else overspeed = false;

        if ((xMoveSpeed > maxSpeed) || (xMoveSpeed < -maxSpeed)) xInput = 0;
        if ((yMoveSpeed > maxSpeed) || (yMoveSpeed < -maxSpeed)) yInput = 0;

        //Debug.Log(xMoveSpeed + " , " + yMoveSpeed);
        //if (overspeed) return;

        if (!isGrounded)
        {
            movementRestriction = 0.6f;
            inAir = 1.3f;
        }
        rb.AddForce(orientation.transform.forward * yInput * moveSpeed * Time.deltaTime * movementRestriction);
        rb.AddForce(orientation.transform.right * xInput * moveSpeed * Time.deltaTime * inAir);

    }

    void checkIfSprinting()
    {
        if (sprinting && !isSliding && !isCrouched)
        {
            maxSpeed = sprintSpeed;
            crouched = false;
            isCrouched = false;
        }
        else if (!isSliding)
        {
            maxSpeed = originalMaxSpeed;
        }
    }

    Vector2 findMovementSpeed()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);

        return new Vector2(xMag, yMag);
    }

    void counteredMovement(Vector2 mag) //to counter the movement if the player stopped moving
    {
        if (Mathf.Abs(mag.x) > threshold && Mathf.Abs(xInput) < 0.05f || (mag.x < -threshold && xInput > 0) || (mag.x > threshold && xInput < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > threshold && Mathf.Abs(yInput) < 0.05f || (mag.y < -threshold && yInput > 0) || (mag.y > threshold && yInput < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        //checks if the player is too fast and fixes that
        if ((rb.velocity.magnitude > maxSpeed || rb.velocity.magnitude < -maxSpeed))//&& !recientlyWallRanOrSlided
        {
            if (!isGrounded) return;
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }

        if (isSliding) // add wall run here too
        {
            //Debug.Log("ok" + maxSpeed);
            maxSpeed = originalMaxSpeed * momentumSpeedMultiplier;
            
        }
        else
        {
            //Debug.Log("no" + maxSpeed);
            maxSpeed = originalMaxSpeed;
        }
    }

    void cameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        Vector3 rot = cam.transform.localRotation.eulerAngles;
        float desiredX = rot.y + mouseX;

        xCam -= mouseY;
        xCam = Mathf.Clamp(xCam, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xCam, desiredX, wallrunCameraTilt);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        if (Mathf.Abs(wallrunCameraTilt) < maxWallrunCameraTilt && isWallRunning && isWallRight) wallrunCameraTilt += Time.deltaTime * maxWallrunCameraTilt * 2;
        if (Mathf.Abs(wallrunCameraTilt) < maxWallrunCameraTilt && isWallRunning && isWallLeft) wallrunCameraTilt -= Time.deltaTime * maxWallrunCameraTilt * 2;

        if (wallrunCameraTilt > 0 && !isWallRunning) wallrunCameraTilt -= Time.deltaTime * maxWallrunCameraTilt * 2;
        if (wallrunCameraTilt < 0 && !isWallRunning) wallrunCameraTilt += Time.deltaTime * maxWallrunCameraTilt * 2;
    } 
    void wallrunInput()
    {
        if ((isWallRight || isWallLeft) && !isGrounded) startWallrun();
    }
    void startWallrun()
    {

        crouched = false;
        isCrouched = false;
        uncrouch();
        addMomentum(Time.deltaTime);

        if (isWallRight && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space)) rb.AddForce(-orientation.right * jumpForce * 3.2f);
        if (isWallLeft && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space)) rb.AddForce(orientation.right * jumpForce * 3.2f);

        if (Input.GetKey(KeyCode.Space)) rb.velocity = new Vector3(rb.velocity.x, 5f, rb.velocity.z);
        else if (decendWhileWallRunning) rb.velocity = new Vector3(rb.velocity.x, -5f, rb.velocity.z);
        else rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.useGravity = false;
        isWallRunning = true;

        

        if (rb.velocity.magnitude <= (momentumSpeedMultiplier * originalMaxSpeed))
        {
            rb.AddForce(orientation.forward * moveSpeed * Time.deltaTime * wallrunForce);

            if (isWallRight) rb.AddForce(orientation.right  * 2 * Time.deltaTime);
            if (isWallLeft) rb.AddForce(-orientation.right  * 2 * Time.deltaTime);
        }
    }


    void stopWallrun()
    {
        //restoredVelocity = false;
        rb.useGravity = true;
        isWallRunning = false;
    }

    void checkForWall()
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, wallrunWalls);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, wallrunWalls);

        if (!isWallLeft && !isWallRight) stopWallrun();
        if (isGrounded) stopWallrun();

    }

    public float whatsCurrentSpeed()
    {
        return currentSpeed;
    }

    public void takeDamage(float damageTaken)
    {
        Health -= damageTaken;
    }
}