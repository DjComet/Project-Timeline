using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelRotation : MonoBehaviour {
    //A
    private tramMovement tramMovement;
    public float wheelRadius = 1.119f;

	// Use this for initialization
	void Start () {
        tramMovement = GetComponentInParent<tramMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(-Vector3.forward  * tramMovement.speed / wheelRadius);
	}
}
