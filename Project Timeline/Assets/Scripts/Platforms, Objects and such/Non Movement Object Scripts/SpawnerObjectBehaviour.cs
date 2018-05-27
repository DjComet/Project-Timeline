using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerObjectBehaviour : MonoBehaviour {
    //A
    TimeLine timeLine;
    public GameObject objectToSpawn;
    public GameObject connectedTo;
    public List<GameObject> instantiatedGameObjects;
    public bool active;
    bool activated = false;
    public bool renderThisObjectOnPlay;
    public Transform parent;
    

	// Use this for initialization
	void Start () {
        timeLine = GetComponent<TimeLine>();
        if(transform.parent!= null)
        {
            parent = transform.parent;
        }

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
            instantiatedGameObjects.Add(instantiate()); //If we insert this function inside the timeEvent creation, the object will be spawned twice if we have gone backwards in time and then forwards through this timeEvent.

            timeLine.createTimeEvent(
            true,
            delegate {
                
            },
            delegate (){
                if(instantiatedGameObjects.Count>0)
                {
                    destroyInstantiated(instantiatedGameObjects[0]);
                    instantiatedGameObjects.RemoveAt(0);
                }
                
            });


            activated = true;
        }
        else if(!active)
        {
            activated = false;
        }
        

	}

    GameObject instantiate ()
    {
        var newObject = Instantiate(objectToSpawn, transform.position, transform.rotation);
        newObject.transform.parent = parent;
        return newObject;
        
    }

    void destroyInstantiated(GameObject instantiated)
    {
        GameObject toDestroy = instantiated;
        Destroy(toDestroy);
    }
}
