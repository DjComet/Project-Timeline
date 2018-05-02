using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimb : MonoBehaviour{
    //A
    private float dt;


    private Inputs inputs;
    private Values values;
    private MainPlayerController playerController;

    private Transform cam;
    private CharacterController charCon;
    private GameObject ladder;
    private float oldLadderSpeed;

    public float angleFromLadder;
    public Vector3 speed = new Vector3(0.0f, 0.0f, 0.0f);
    public float speedMagnitude;
    //[HideInInspector]
    public Vector3 axisDirection;

    public Vector3 deltaLadder = new Vector3(-30.0f, 0.0f, 0.0f);

    // Use this for initialization
    void Start()
    {

        //references to scripts
        values = gameObject.GetComponent<Values>();
        inputs = gameObject.GetComponent<Inputs>();
        playerController = gameObject.GetComponent<MainPlayerController>();

        //references to other player-related objects
        cam = Camera.main.transform;
        charCon = gameObject.GetComponent<CharacterController>();

        oldLadderSpeed = values.maxLadderSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        Vector3 forwardXZ = cam.transform.forward;
        forwardXZ.y = 0;
        forwardXZ.Normalize();

        angleFromLadder = Vector3.Angle(forwardXZ, ladder.transform.right);//angle between the ladder and the cam.forward.

        if (values.grounded)
        {
            groundedMovement();
        }
        else
        {
            ladderMovement();
        }
    }

    void ladderMovement()
    {
        
        if (angleFromLadder > 120.0f && inputs.vertical > 0)
        {
            XZMovement();
        }
        else if (angleFromLadder <= 120.0f) 
        {
            speed.x = 0;
            speed.z = 0;
            YMovement();
        }
        

      

        
    }

    void groundedMovement()
    {
        if (angleFromLadder < 40.0f && inputs.vertical > 0)
        {
            speed.x = 0;
            speed.z = 0;
            YMovement();
        }
        else //if (angleFromLadder >= 40.0f)
        {
            XZMovement();
        }

    }
    void YMovement()
    {
        Vector3 dirForward = 2 * cam.forward + cam.up; //dirForward is the forward direction of the camera (z axis)
        dirForward.x = 0;
        dirForward.z = 0;
        dirForward.Normalize();

        Vector3 inputMaxSpeed = new Vector3(0.0f, values.maxLadderSpeed * inputs.vertical * dirForward.y, 0.0f);
        Vector3 offsetSpeed = inputMaxSpeed - speed;
        offsetSpeed.y = Mathf.Clamp(offsetSpeed.y, -values.ladderAcceleration * dt, values.ladderAcceleration * dt);


        speed.y += offsetSpeed.y;


        //By taking the direction directly from the speed values, speed.x or.z are always between [0,1], but because speed
        //increments and decrements with accel, the values for the direction do not become 0 as soon as the inputs aren't pressed
        Vector3 normalizedSpeed = speed.normalized;
        axisDirection = normalizedSpeed.y * dirForward;

        //We clamp the speed before applying it so that we don't move faster while holding both axis
        speedMagnitude = speed.magnitude;
        speedMagnitude = Mathf.Clamp(speedMagnitude, 0, values.maxLadderSpeed);

        if (axisDirection.y < 0)
        {
            values.maxLadderSpeed = 12;
        }
        else values.maxLadderSpeed = oldLadderSpeed;

        charCon.Move(speed * dt);
    }
    void XZMovement()
    {
        Vector3 dirForward = cam.forward; //dirForward is the forward direction of the camera (z axis)
        dirForward.y = 0;
        dirForward.Normalize();

        Vector3 dirSides = cam.right;   //same but with x camera axis.
        dirSides.y = 0;
        dirSides.Normalize();

        Vector3 inputMaxSpeed = new Vector3(values.maxSpeed * inputs.horizontal, 0.0f, values.maxSpeed * inputs.vertical);
        Vector3 offsetSpeed = inputMaxSpeed - speed;
        offsetSpeed.x = Mathf.Clamp(offsetSpeed.x, -values.acceleration * dt, values.acceleration * dt);
        offsetSpeed.z = Mathf.Clamp(offsetSpeed.z, -values.acceleration * dt, values.acceleration * dt);

        speed.x += offsetSpeed.x;
        speed.z += offsetSpeed.z;

        //By taking the direction directly from the speed values, speed.x or.z are always between [0,1], but because speed
        //increments and decrements with accel, the values for the direction do not become 0 as soon as the inputs aren't pressed
        Vector3 normalizedSpeed = speed.normalized;
        axisDirection = normalizedSpeed.x * dirSides + normalizedSpeed.z * dirForward;

        //We clamp the speed before applying it so that we don't move faster while holding both axis
        speedMagnitude = speed.magnitude;
        speedMagnitude = Mathf.Clamp(speedMagnitude, 0, values.maxSpeed);

        charCon.Move(axisDirection * speedMagnitude * dt); //Horizontal charCon input;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            ladder = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Ladder"))
        {
            ladder = null;
        }
    }
}
