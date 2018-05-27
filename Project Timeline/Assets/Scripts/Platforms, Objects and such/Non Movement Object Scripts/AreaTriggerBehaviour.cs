using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaTriggerBehaviour : MonoBehaviour {

    private TimeLine timeLine;
    private Linker linker;

    public bool active = false;
    public bool playerInsideTrigger = false;
    private bool first = true;

    // Use this for initialization
    void Start () {
        timeLine = GetComponent<TimeLine>();
        linker = GetComponent<Linker>();
	}
	
	// Update is called once per frame
	void Update () {

        if (first)
        {
            if (playerInsideTrigger)
            {
                timeLine.createTimeEvent(
               false,
               delegate
               {
                   setActive();
               },
               delegate
               {
                   setInactive();
               });
                first = false;
            }
            else
            {
                timeLine.createTimeEvent(
               false,
               delegate
               {
                   setInactive();
               },
               delegate
               {
                   setActive();
               });
                first = false;
            }
        }

        linker.active = active;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerIn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerOut();
        }
    }

    void setActive()
    {
        active = true;
    }

    void setInactive()
    {
        active = false;
    }

    void playerIn()
    {
        playerInsideTrigger = true;
        first = true;
    }

    void playerOut()
    {
        playerInsideTrigger = false;
        first = true;
    }
}
