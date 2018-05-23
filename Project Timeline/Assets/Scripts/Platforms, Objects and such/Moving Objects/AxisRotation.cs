using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisRotation : MonoBehaviour {
    //A
    float dt;
    private TimeLine objectTimeLine;
    public GameObject Parent;
    private ActivatorBehaviour aB;


    public bool sameRotationOnAllAxis = false;
    public float rotationForAllAxis = 5.0f;

    public float rotationSpeedX = 0.0f;
    float initrotX;
    public float rotationSpeedY = 0.0f;
    float initrotY;
    public float rotationSpeedZ = 0.0f;
    float initrotZ;

    public bool rotationX = false;
    public bool rotationY = true;
    public bool rotationZ = false;

    public bool pingPongX = false;
    public bool pingPongY = false;
    public bool pingPongZ = false;
	// Use this for initialization
	void Start () {
        objectTimeLine = gameObject.GetComponent<TimeLine>();
        initrotX = rotationSpeedX;
        initrotY = rotationSpeedY;
        initrotZ = rotationSpeedZ;
        if (Parent != null && Parent.GetComponent<ActivatorBehaviour>())
        {
            aB = Parent.GetComponent<ActivatorBehaviour>();
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (objectTimeLine != null)
            dt = objectTimeLine.scaledDT;
        else
            dt = Time.deltaTime;
        

        


        if(Parent != null && aB != null && aB.active)
        {
            rotations();
        }
        else if (Parent == null)
        {
            rotations();
        }

       
    }

    void rotations()
    {
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

        if (pingPongX)
        {
            rotationSpeedX = -initrotX + Mathf.PingPong(Time.time * 2, initrotX * 2);
        }

        if (pingPongY)
        {
            rotationSpeedY = -initrotY + Mathf.PingPong(Time.time , initrotY * 2);
        }
        if (pingPongZ)
        {
            rotationSpeedZ = -initrotZ + Mathf.PingPong(Time.time * 1.3f, initrotZ * 2);
        }
    }
}
