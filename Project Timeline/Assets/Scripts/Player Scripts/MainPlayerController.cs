using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerController : MonoBehaviour {
    //A
    private Inputs inputs;
    private Values values;
    private Locomotion locomotion; 
    //private Air air;
    private WallClimb wallClimb;
    private LadderClimb ladderClimb;
    public float oldAcceleration;
    public float oldMaxSpeed;
    float timer = 0.0f;

    public bool isGrounded = true;
    public bool isAirborne = false;
    public bool isOnWallEdge = false;
    public bool isOnLadder = false;

    private TimeScaleControl timeScaleControl;
    private IsolationBomb isolationBomb;
    private PortalCreator portalCreator;
    private TimeSphere timeSphere;
    
    public int weaponSelector = 0;


	// Use this for initialization
	void Start () {
        inputs      = gameObject.GetComponent<Inputs>();
        values      = gameObject.GetComponent<Values>();
        locomotion  = gameObject.GetComponent<Locomotion>(); 
        wallClimb   = gameObject.GetComponent<WallClimb>();
        ladderClimb = gameObject.GetComponent<LadderClimb>();
        oldAcceleration = values.acceleration;
        oldMaxSpeed = values.maxSpeed;
        timeScaleControl = gameObject.GetComponent<TimeScaleControl>();
        isolationBomb = gameObject.GetComponent<IsolationBomb>();
        portalCreator = gameObject.GetComponent<PortalCreator>();
        timeSphere = gameObject.GetComponent<TimeSphere>();

    }
	
	// Update is called once per frame
	void Update () {

        ChangeState();
        ChangeWeapon();
        
	}

    void ChangeState()
    {
        if (isGrounded)
        {
            values.acceleration = oldAcceleration;
            values.maxSpeed = oldMaxSpeed;

            wallClimb.enabled = false;
            timer = 0.0f;

        }

        if (isAirborne)
        {
            timer += Time.deltaTime;
            values.acceleration = values.airAcceleration;
            if (timer >= 0.5)
            {
                values.maxSpeed = values.maxAirSpeed;
            }

        }

        if (isAirborne && isOnWallEdge)
        {

            wallClimb.enabled = true;
        }
        else if (!isOnWallEdge)
        {
            wallClimb.enabled = false;
        }


        if (isOnLadder)
        {
            locomotion.enabled = false;
            locomotion.speed = Vector3.zero;//The direction and speed values must be zeroed on exit because if not,
            locomotion.axisDirection = Vector3.zero;//they will be reapplied as soon as player exits the ladder.
            ladderClimb.enabled = true;
        }
        else
        {
            locomotion.enabled = true;
            ladderClimb.enabled = false;
            ladderClimb.speed = Vector3.zero;//The direction and speed values must be zeroed on exit because if not,
            ladderClimb.axisDirection = Vector3.zero;//they will be reapplied as soon as player reenters the ladder.
        }
    }

    void ChangeWeapon()
    {
        if (inputs.weap1) weaponSelector = 0;
        if (inputs.weap2) weaponSelector = 1;
        if (inputs.weap3) weaponSelector = 2;
        if (inputs.weap4) weaponSelector = 3;

        if(inputs.mouseScroll > 0)
        {
            weaponSelector++;
        }
        else if (inputs.mouseScroll < 0)
        {
            weaponSelector--;
        }

        weaponSelector = Mathf.Clamp(weaponSelector, 0, 3);


        if(weaponSelector == 0) //Time control
        {
            timeScaleControl.enabled = true;
            portalCreator.enabled = false;
            isolationBomb.enabled = false; 
            timeSphere.enabled = false;
        }
        else if (weaponSelector == 1)//Portals
        {
            timeScaleControl.enabled = false;
            portalCreator.enabled = true;
            isolationBomb.enabled = false; 
            timeSphere.enabled = false;
        }
        else if (weaponSelector == 2)//Isolation
        {
            timeScaleControl.enabled = false;
            portalCreator.enabled = false;
            isolationBomb.enabled = true;
            timeSphere.enabled = false;
        }
        else if (weaponSelector == 3)//Time Sphere
        {
            timeScaleControl.enabled = true;
            portalCreator.enabled = false;
            isolationBomb.enabled = false;
            timeSphere.enabled = true;
        }
    }

}
