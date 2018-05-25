using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoorMovement : MonoBehaviour {
    //A
    float scaledDt;

    public GameObject connectedTo;
    [HideInInspector]
    public TimeLine timeLine;

    public Transform doorRD;
    public Transform doorRU;
    public Transform doorLD;
    public Transform doorLU;

    private Quaternion initialRotation;
    private Quaternion targetRotationLU;
    private Quaternion targetRotationLD;
    private Quaternion targetRotationRU;
    private Quaternion targetRotationRD;
    

    
    public float openingSpeed = 5.0f;

    public bool open = false;
    bool debugControl = true;
    public float delayBetweenDoors = 0.1f;
    private float counter = 0.0f;

    float t1 = 0;
    float t2 = 0;
    float t3 = 0;
    float t4 = 0;
   
    // Use this for initialization
    void Start () {
        timeLine = gameObject.GetComponent<TimeLine>();

        initialRotation = doorLD.rotation;
  
        targetRotationLU = doorLU.rotation;
        targetRotationLD = doorLU.rotation;
        targetRotationRU = doorRU.rotation; 
        targetRotationRD = doorRU.rotation;
    }
	
	// Update is called once per frame
	void Update () {
        scaledDt = timeLine.scaledDT;

        if(connectedTo != null)
        {
            if (connectedTo.GetComponent<Linker>().active)
            {
                open = true;
            }
            else open = false;
        }
        else
        {
            if(debugControl)
            {
                Debug.Log("Missing link on door: " + gameObject.name);
                debugControl = false;
            }
            
        }

        tCalculations();

        doorLD.rotation = Quaternion.Slerp(initialRotation, targetRotationLD, t4);
        doorRU.rotation = Quaternion.Slerp(initialRotation, targetRotationRU, t2);
        doorRD.rotation = Quaternion.Slerp(initialRotation, targetRotationRD, t3);
        doorLU.rotation = Quaternion.Slerp(initialRotation, targetRotationLU, t1);

        
        //doorLD.rotation.eulerAngles = new Vector3(0,Mathf.Clamp(doorLD.rotation.eulerAngles.y, 0, targetAngleOfRotation),0);

    }

    void tCalculations()
    {
        //t1 = doorLD.rotation.eulerAngles.y / targetRotation.eulerAngles.y;
        counter = Mathf.Clamp(counter, 0, delayBetweenDoors * 3);

        if (open)
        {
            counter += Mathf.Abs(scaledDt);
            if(counter >= 0)
            {
                t1 += Mathf.Abs(scaledDt) * openingSpeed;
                t1 = Mathf.Clamp01(t1);
            }
            

            if (counter >= delayBetweenDoors)
            {
                t2 += Mathf.Abs(scaledDt) * openingSpeed;
                t2 = Mathf.Clamp01(t2);
            }

            if (counter >= delayBetweenDoors * 2)
            {
                t3 += Mathf.Abs(scaledDt) * openingSpeed;
                t3 = Mathf.Clamp01(t3);
            }

            if (counter >= delayBetweenDoors * 3)
            {
                t4 += Mathf.Abs(scaledDt) * openingSpeed;
                t4 = Mathf.Clamp01(t4);
            }
        }
        else
        {
            counter -= Mathf.Abs(scaledDt);
            if(counter <= 0)
            {
                t1 -= Mathf.Abs(scaledDt) * openingSpeed;
                t1 = Mathf.Clamp01(t1);
            }

            if (counter <= delayBetweenDoors)
            {
                t2 -= Mathf.Abs(scaledDt) * openingSpeed;
                t2 = Mathf.Clamp01(t2);
            }

            if (counter <= delayBetweenDoors * 2)
            {
                t3 -= Mathf.Abs(scaledDt) * openingSpeed;
                t3 = Mathf.Clamp01(t3);
            }

            if (counter <= delayBetweenDoors * 3)
            {
                t4 -= Mathf.Abs(scaledDt) * openingSpeed;
                t4 = Mathf.Clamp01(t4);
            }
        }
    }
}
