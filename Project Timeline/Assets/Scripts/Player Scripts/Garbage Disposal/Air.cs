using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Air : MonoBehaviour {

    private float dt;

    private Inputs inputs;
    private Values values;
    private MainPlayerController playerController;
    private Transform cam;
    private CharacterController charCon;

    public Vector3 airSpeed = new Vector3(0.0f, 0.0f, 0.0f);
    public float speedMagnitude;
    [HideInInspector]
    public Vector3 axisDirection;

    public bool grounded = false;
    private bool canMaintainJump = false;
    private float jumpForce = 0.0f;
    private float timer = 0;
    private float coyoteTimer = 0.0f;
    private bool allowJump = true;

    
    Vector3 directionPos;

    // Use this for initialization
    void Start () {
        
        //references to scripts
        values = gameObject.GetComponent<Values>();
        inputs = gameObject.GetComponent<Inputs>();
        playerController = gameObject.GetComponent<MainPlayerController>();

        //references to other player-related objects
        cam = Camera.main.transform;
        charCon = gameObject.GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        dt = Time.deltaTime;

        //Position-----------------------------------------------------------------------------------------------POS------------------------------------------------------------------------------------

        Vector3 dirForward = cam.forward; //dirForward is the forward direction of the camera (z axis)
        dirForward.y = 0;
        dirForward.Normalize();

        Vector3 dirSides = cam.right;   //same but with x camera axis.
        dirSides.y = 0;
        dirSides.Normalize();

        Vector3 inputMaxAirSpeed = new Vector3(values.maxAirSpeed * inputs.horizontal, 0.0f, values.maxAirSpeed * inputs.vertical);
        Vector3 offsetSpeed = inputMaxAirSpeed - airSpeed;
        offsetSpeed.x = Mathf.Clamp(offsetSpeed.x, -values.airAcceleration * dt, values.airAcceleration * dt);
        offsetSpeed.z = Mathf.Clamp(offsetSpeed.z, -values.airAcceleration * dt, values.airAcceleration * dt);

        airSpeed.x += offsetSpeed.x;
        airSpeed.z += offsetSpeed.z;

        //By taking the direction directly from the speed values, speed.x or.z are always between [0,1], but because speed
        //increments and decrements with accel, the values for the direction do not become 0 as soon as the inputs aren't pressed
        Vector3 normalizedSpeed = airSpeed.normalized;
        axisDirection = normalizedSpeed.x * dirSides + normalizedSpeed.z * dirForward;

        //We clamp the speed before applying it so that we don't move faster while holding both axis
        speedMagnitude = airSpeed.magnitude;
        speedMagnitude = Mathf.Clamp(speedMagnitude, 0, values.maxSpeed);

        charCon.Move(axisDirection * speedMagnitude * dt);


        HandleJump();


	}


    void HandleJump()//---------------------------------------------------------------------------------------SALTO-----------------------------------------------------------------------------------
    {
        grounded = Physics.Raycast(transform.position, -transform.up, values.deltaGround);

        if (grounded)//If it's touching the ground, allow for an extra time window to jump after it is no longer on the platform
        {
            allowJump = true;
            coyoteTimer = 0.0f;
        }

        if (!grounded && jumpForce == 0)//This starts applying a gravity force as soon as the character is not grounded and isn't in the first half (ascending) part of a jump (while canMaintainJump is true).
        {
            values.jumpDeceleration = values.maxJumpDeceleration;
            float offsetFallSpeed = values.maxFallSpeed - airSpeed.y;
            offsetFallSpeed = Mathf.Clamp(offsetFallSpeed, -values.gravity * dt, values.gravity * dt);

            airSpeed.y -= offsetFallSpeed;

            airSpeed.y = Mathf.Clamp(airSpeed.y, -values.maxFallSpeed, 0);
        }
        else airSpeed.y = 0;

        if (!grounded && !canMaintainJump)//If it's not on the ground and it's not currently jumping
        {
            coyoteTimer += dt;

            if (coyoteTimer >= values.coyoteTime)
            {
                allowJump = false;
                coyoteTimer = 0.0f;
            }

        }

        if (inputs.jumpBool && allowJump)
        {
            jumpForce = values.maxJumpForce;
            values.jumpDeceleration = values.maxJumpDeceleration;
            canMaintainJump = true;
            allowJump = false;
        }

        if (inputs.jump && canMaintainJump)
        {
            timer += dt;

            if (timer >= 0.12)
            {
                values.jumpDeceleration -= values.jumpDecelerationRate * dt;

                values.jumpDeceleration = Mathf.Clamp(values.jumpDeceleration, values.minJumpDeceleration, values.maxJumpDeceleration);
            }

        }
        else values.jumpDeceleration = values.maxJumpDeceleration;

        if (canMaintainJump)
        {
            float offsetJump = values.maxJumpForce - 0;
            offsetJump = Mathf.Clamp(offsetJump, -values.jumpDeceleration * dt, values.jumpDeceleration * dt);
            jumpForce -= offsetJump;

            jumpForce = Mathf.Clamp(jumpForce, 0, values.maxJumpForce);

            airSpeed.y = jumpForce;
            if (jumpForce <= 0)
            {
                canMaintainJump = false;
                timer = 0;
            }
        }


        charCon.Move(Vector3.up * airSpeed.y * dt);
    }
}

