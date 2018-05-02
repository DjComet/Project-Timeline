using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {
    //A
    private float dt;

    private Inputs inputs;
    private Values values;
    private MainPlayerController playerController;
    private WallClimb wallClimb;
    private Transform cam;
    private CharacterController charCon;
    public Transform raycastForeheadTransform;

    public Vector3 speed = Vector3.zero;
    public bool canMaintainJump = false;
    public float jumpForce = 0.0f;
    private float timer = 0;
    public float coyoteTimer = 0.0f;
    public bool allowJump = true;

    // Use this for initialization
    void Start ()
    {
        values = gameObject.GetComponent<Values>();
        inputs = gameObject.GetComponent<Inputs>();
        playerController = gameObject.GetComponent<MainPlayerController>();
        wallClimb = gameObject.GetComponent<WallClimb>();

        //references to other player-related objects
        cam = Camera.main.transform;
        charCon = gameObject.GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;

        if(playerController.isOnLadder)
        {
            speed.y = 0;
        }

        if (values.grounded || playerController.isOnLadder)//If it's touching the ground or in a ladder, allow for an extra time window to jump after it is no longer on the platform
        {
            playerController.isAirborne = false;
            playerController.isGrounded = true;
            allowJump = true;
            coyoteTimer = 0.0f;
        }
        else
        {
            playerController.isGrounded = false;
            playerController.isAirborne = true;
        }

        OnWallEdge();

        if (values.hitCeiling)
        {
            jumpForce = 0;
        }

        if (!values.grounded && jumpForce == 0 && !playerController.isOnLadder)//This starts applying a gravity force as soon as the character is not grounded and isn't in the first half (ascending) part of a jump (while canMaintainJump is true).
        {
            values.jumpDeceleration = values.maxJumpDeceleration;


            float offsetFallSpeed = values.maxFallSpeed - speed.y;
            offsetFallSpeed = Mathf.Clamp(offsetFallSpeed, -values.gravity * dt, values.gravity * dt);

            speed.y -= offsetFallSpeed;

            speed.y = Mathf.Clamp(speed.y, -values.maxFallSpeed, 0);
        }
        else if(!playerController.isOnLadder)
            speed.y = -values.stickToGroundForce;

        
        //aqui es el salto de verdad
        if (inputs.jumpBool && allowJump)
        {
            jumpForce = values.maxJumpForce;
            values.jumpDeceleration = values.maxJumpDeceleration;
            canMaintainJump = true;
            allowJump = false;
        }
        else if (!values.grounded && !canMaintainJump)//If it's not on the ground and it's not currently jumping
        {
            coyoteTimer += dt;

            if (coyoteTimer >= values.coyoteTime)
            {
                allowJump = false;
                coyoteTimer = 0.0f;
            }

        }

        if (inputs.jump && canMaintainJump)//the deceleration decreases the longer you hold jump button, so you jump higher.
        {
            timer += dt;

            if (timer >= 0.12)
            {
                values.jumpDeceleration -= values.jumpDecelerationRate * dt;

                values.jumpDeceleration = Mathf.Clamp(values.jumpDeceleration, values.minJumpDeceleration, values.maxJumpDeceleration);
            }

        }
        else values.jumpDeceleration = values.maxJumpDeceleration;

        if (canMaintainJump)//if jump is initiated, this is the code that makes the player ascend until jumpForce reaches 0. jumpForce is decelerated at 'jumpDeceleration' per second.
        {
            float offsetJump = values.maxJumpForce - 0;
            offsetJump = Mathf.Clamp(offsetJump, -values.jumpDeceleration * dt, values.jumpDeceleration * dt);
            jumpForce -= offsetJump;

            jumpForce = Mathf.Clamp(jumpForce, 0, values.maxJumpForce);

            speed.y = jumpForce;
            if (jumpForce <= 0)
            {
                canMaintainJump = false;
                timer = 0;
            }
        }


        charCon.Move(Vector3.up * speed.y * dt);
    }

    void OnWallEdge()
    {

        bool isRaycastForehead = Physics.Raycast(raycastForeheadTransform.position, transform.forward, values.deltaWallClimb);
        bool isRaycastBody = Physics.Raycast(transform.position, transform.forward, values.deltaWallClimb);

        if (!isRaycastForehead && isRaycastBody)
        {
            playerController.isOnWallEdge = true;
        }
        else if (isRaycastForehead || !isRaycastBody)
        {
            playerController.isOnWallEdge = false;
            wallClimb.notActiveBefore = true;
        }
    }
}
