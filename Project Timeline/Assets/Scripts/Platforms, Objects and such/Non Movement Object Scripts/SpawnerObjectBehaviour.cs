using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjectBehaviour : MonoBehaviour {
    //A
    TimeLine timeLine;
    public GameObject objectToSpawn;
    public GameObject connectedTo;
    public bool active;
    bool activated = false;
    public bool renderThisObjectOnPlay;

	// Use this for initialization
	void Start () {
        timeLine = GetComponent<TimeLine>();
        if (!renderThisObjectOnPlay)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                GameObject child = gameObject.transform.GetChild(i).gameObject;
                child.GetComponent<MeshRenderer>().enabled = false;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if(connectedTo != null)
        active = connectedTo.GetComponent<Linker>().active;
        
        //if(objectTimeline.ownTimeScale != 0)

        if(active && !activated)
        {
            Instantiate(objectToSpawn, transform.position, transform.rotation);
            
            activated = true;
        }
        else if(!active)
        {
            activated = false;
        }
        

	}
}
