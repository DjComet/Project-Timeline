using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomotion : MonoBehaviour {
    //A
    private float dt;


    private Inputs inputs;
    private Values values;
    private MainPlayerController playerController;
    private WallClimb wallClimb;
    private Transform cam;
    private CharacterController charCon;


    public Vector3 speed = new Vector3(0.0f, 0.0f, 0.0f);
    public float speedMagnitude;
    [HideInInspector]
    public Vector3 axisDirection;
    public Vector3 nextPos;


   

    
    Vector3 directionPos;
    

    // Use this for initialization
    void Start () {

        //references to scripts
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

        //Position-----------------------------------------------------------------------------------------------POS------------------------------------------------------------------------------------

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

        nextPos = (axisDirection * speedMagnitude * dt);

        charCon.Move(nextPos); //Horizontal charCon input;     
    }
}

