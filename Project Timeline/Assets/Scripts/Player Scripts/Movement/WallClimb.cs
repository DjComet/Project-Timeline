using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallClimb : MonoBehaviour {
    //A
    float dt;

    private Inputs inputs;
    private Values values;
    private Locomotion locomotion;
    private MainPlayerController playerController;
    private CharacterController charCon;
    private Jump jump;

    public Vector3 speed = new Vector3(0.0f, 0.0f, 0.0f);
    
    private float timer = 0;
    public bool canMaintainJump = false;
    public bool notActiveBefore = true;
    public bool setJumpForce = false;

    // Use this for initialization
    void Start () {

        values = gameObject.GetComponent<Values>();
        inputs = gameObject.GetComponent<Inputs>();
        playerController = gameObject.GetComponent<MainPlayerController>();
        locomotion = gameObject.GetComponent<Locomotion>();
        jump = gameObject.GetComponent<Jump>();

        charCon = gameObject.GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        dt = Time.deltaTime;

        if(inputs.jump)
        {
            if (notActiveBefore) setJumpForce = true;            
        }
        
        if(setJumpForce)
        {
            notActiveBefore = false;
            jump.jumpForce = values.maxJumpForce - 3;
            jump.canMaintainJump = true;
            setJumpForce = false; 
        }
    }
}

