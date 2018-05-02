using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyMovementToPlayer : MonoBehaviour {
    //A [WIP]
    
    private BoxCollider box;
    public GameObject otherGo;
    public float maxSpeedDifference = 20.0f;
    public Vector3 speedDifference = Vector3.zero;
    private bool nextFrameOff = false;

    public float speedMagnitude;
    public Vector3 speedVector;
    
	// Use this for initialization
	void Start () {
        /*if (!gameObject.GetComponent<BoxCollider>())
        {
            gameObject.AddComponent<BoxCollider>();
        }
        box = gameObject.GetComponent<BoxCollider>();
        box.isTrigger = true;
        box.size = new Vector3(1.5f,3f,1.5f);
        box.center = new Vector3(0, 1.5f, 0);*/
       
	}
	
	// Update is called once per frame
	void Update () {
		if(otherGo)
        {
            speedDifference = otherGo.GetComponent<Locomotion>().speed - speedVector;
            if (Mathf.Abs(otherGo.GetComponent<Locomotion>().speedMagnitude - speedMagnitude) < maxSpeedDifference)
            {
                otherGo.transform.parent = gameObject.transform;
            }
            else
            {
                //Add code here to make the player go the fuck away from the too-fast-moving platform.
                otherGo.transform.parent = null;
    
            }

            //Debug.Log("SpeedVector: " + movingObjectSpeed.speedVector);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            otherGo = other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //otherGo.GetComponent<Locomotion>().speed += movingObjectSpeed.speedVector;
            if(otherGo.transform.parent)
            {
                otherGo.transform.parent = null;
            }

            otherGo = null;
        }
    }
}
