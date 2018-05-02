using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtJoint : MonoBehaviour {
    //A
    public Transform joint;
    public bool inverseDirection = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if(inverseDirection)
        {
            transform.LookAt(2 * transform.position - joint.position);
        }
        else transform.LookAt(joint.position);

	}
}
