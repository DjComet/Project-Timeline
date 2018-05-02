using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour {
    //A
    public float horizontal;
    public float vertical;
    public bool jump;
    public bool jumpBool;
    public bool leftClick;
    public bool leftClickPressed;
    public bool rightClick;
    public bool rightClickPressed;
    public bool actionRight;
    public bool actionRightPressed;
    public bool actionLeft;
    public bool actionLeftPressed;
    public bool modifier;
    public bool activate;
    public bool submit;
    public bool cancel;
    public float mouseX;
    public float mouseY;
    public float mouseScroll;
    public float timeScaleAxis;

    public bool weap1;
    public bool weap2;
    public bool weap3;
    public bool weap4;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        horizontal = Input.GetAxisRaw("Horizontal1");
        vertical = Input.GetAxisRaw("Vertical1");
        jumpBool = Input.GetButtonDown("Jump");
        jump = Input.GetButton("Jump");

        leftClick = Input.GetButton("LeftClick");
        leftClickPressed = Input.GetButton("LeftClick");
        rightClick = Input.GetButton("RightClick");
        rightClickPressed = Input.GetButton("RightClick");

        actionRight = Input.GetButtonDown("ActionRight");
        actionRightPressed = Input.GetButton("ActionRight");
        actionLeft = Input.GetButtonDown("ActionLeft");
        actionLeftPressed = Input.GetButton("ActionLeft");
        modifier = Input.GetButton("Modifier");

        activate = Input.GetButtonDown("Activate");
        submit = Input.GetButtonDown("Submit");
        cancel = Input.GetButtonDown("Cancel");

        mouseX = Input.GetAxisRaw("MouseX");
        mouseY = Input.GetAxisRaw("MouseY");
        mouseScroll = Input.GetAxisRaw("MouseScrollWheel");

        weap1 = Input.GetButtonDown("Weapon1");
        weap2 = Input.GetButtonDown("Weapon2");
        weap3 = Input.GetButtonDown("Weapon3");
        weap4 = Input.GetButtonDown("Weapon4");


    }
}
