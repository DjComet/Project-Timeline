using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {
    //A
    float dt;
    private ObjectTimeLine objectTimeLine;

    public bool sameRotationOnAllAxis = false;
    public float rotationForAllAxis = 5.0f;

    public float rotationSpeedX = 0.0f;
    public float rotationSpeedY = 0.0f;
    public float rotationSpeedZ = 0.0f;

    public bool rotationX = false;
    public bool rotationY = true;
    public bool rotationZ = false;

	// Use this for initialization
	void Start () {
        objectTimeLine = gameObject.GetComponent<ObjectTimeLine>();
	}
	
	// Update is called once per frame
	void Update () {

        if (objectTimeLine != null)
            dt = objectTimeLine.scaledDT;
        else
            dt = Time.deltaTime;
        

        float rotX = (rotationX ? 1 : 0);
        float rotY = (rotationY ? 1 : 0);
        float rotZ = (rotationZ ? 1 : 0);





        if (sameRotationOnAllAxis)
        {
            transform.Rotate(rotX * Mathf.Rad2Deg * rotationForAllAxis * dt, rotY * Mathf.Rad2Deg * rotationForAllAxis * dt, rotZ * Mathf.Rad2Deg * rotationForAllAxis * dt, Space.Self);
        }
        else
        {
            transform.Rotate(rotX * Mathf.Rad2Deg * rotationSpeedX * dt, rotY * Mathf.Rad2Deg * rotationSpeedY * dt, rotZ * Mathf.Rad2Deg * rotationSpeedZ * dt, Space.Self);
        }
        


    }
}
