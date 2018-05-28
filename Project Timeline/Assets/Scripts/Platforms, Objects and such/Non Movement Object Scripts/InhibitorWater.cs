using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhibitorWater : MonoBehaviour {
    //A
    Clock mainClock;
    TimeLine timeLine;
    int i = 0;
    public List<GameObject> destroyedGameObjects;
    public bool isTriggered = false;

    GameObject toDestroy;

    public bool canDestroy = false;
    bool doingAnimation;

    // Use this for initialization
    void Start () {

        timeLine = GetComponent<TimeLine>();
        mainClock = timeLine.clock;

	}
	
	// Update is called once per frame
	void Update () {

        if(canDestroy && toDestroy != null)
        {
            destroyGameObject(toDestroy.transform.gameObject);
        }
        
        
	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerTimeScaleControl>().hasModifiedTime)
                mainClock.resetToNormalTime = true;
        }
        else if(other.CompareTag("RayInteract")) //add "&& ohter.GetComponent<Rigidbody>()" to check if it's actually a movable object and not a lever or smth like that if that poses a problem in the future.
        {
            if(!isTriggered)
            {
                canDestroy = true;
                toDestroy = other.gameObject;
                toDestroy.GetComponent<OnObjectPickedUp>().grabber.forceRelease();
                timeLine.createTimeEvent
                (
                    false,

                    delegate//FWD
                    {

                    },
                    delegate//BKWD
                    {
                        instantiate();
                        Debug.Log("Has instantiated");
                    }

                );
                isTriggered = true;
                i++;
            }
                
            
            Debug.Log("Cube entered the trigger");
        }
        
    }

    void instantiate()
    {
        if(destroyedGameObjects.Count > 0)
        {
            destroyedGameObjects[0].SetActive(true);
            Debug.Log("Instantiated");
            isTriggered = false; //Gotta change it so that it doesn't collide on exit cause extreme madness (multiple time events created as it enters and exits too fast)
            destroyedGameObjects.RemoveAt(0);
        }     
    }

    void doDestroyAnimation(GameObject gameObjToDestroy)
    {
        gameObjToDestroy.transform.localScale -= Vector3.one * 0.75f * timeLine.scaledDT; 
        doingAnimation = true;
        if (gameObjToDestroy.transform.localScale.x <= Mathf.Epsilon)
        {
            doingAnimation = false;
        }
        
    }

    void destroyGameObject(GameObject gameObjToDestroy)
    {
        doDestroyAnimation(gameObjToDestroy);
        if(!doingAnimation)
        {
            destroyedGameObjects.Add(gameObjToDestroy);
            Debug.Log("Destroyed");
            canDestroy = false;
            gameObjToDestroy.SetActive(false);
            toDestroy = null;
        }
        
    }

   
}
