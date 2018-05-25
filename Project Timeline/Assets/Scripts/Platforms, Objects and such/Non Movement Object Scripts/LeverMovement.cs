using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMovement : MonoBehaviour {
    //A
    private TimeLine timeLine;
    private Linker linker;
    private TimeManagerScript timeManager;
    public Transform lever;
    public bool enableDebug;

    public float leverTime = 0.0f;
    float timer = 0;


    public bool active = false;


    public float lerpSpeed = 10.0f;
    private Quaternion initialRot;
    private Quaternion targetRot;// = Quaternion.identity;
    private float t = 0;
    public float targetAngle = 133.0f;

	// Use this for initialization
	void Start () {

        timeLine = GetComponent<TimeLine>();
        linker = GetComponent<Linker>();
        timeManager = TimeManagerScript.timeManager;

        initialRot = lever.localRotation;
        targetRot = Quaternion.Euler(targetAngle, 0, 0);
        

    }
	
	// Update is called once per frame
	void Update () {

        

        t = Mathf.Clamp01(t);

        if(leverTime > 0.0f)
        {
            if (active)
            {
                timer += Mathf.Abs(timeLine.scaledDT);

                if (timer >= leverTime)
                {
                    if(!timeLine.isEventOverlapping())
                    timeEventSetInactive();

                    linker.activate = false;
                    timer = 0;
                }
            }
            else timer = 0;
        }

        if (active)
        {
            t += lerpSpeed * Mathf.Abs(timeLine.scaledDT);
            linker.active = true;
        }
        else
        {
            t -= lerpSpeed * Mathf.Abs(timeLine.scaledDT);
            linker.active = false;
            timer = 0;//redundancy just in case
        }


        lever.localRotation = Quaternion.Slerp(initialRot, targetRot, t);

	}

    public void setActive()
    {
        if(enableDebug)Debug.Log("Se ha llamado a setactive");
        active = true; 
    }

    public void setInactive()
    {
        if(enableDebug)Debug.Log("Se ha llamado a setInactive");
        active = false;
    }

    public void timeEventSetActive()
    {
        //Create event where forward = setActive && backwards = setInactive. Repeatable = true.
        timeLine.createTimeEvent(
        true,
        delegate {
            setActive();
        },
        delegate {
            setInactive();
        });
        
    }

    public void timeEventSetInactive()
    {
        //Create event where forward = setInactive && backwards = setActive. Repeatable = true.
        timeLine.createTimeEvent(
            true,
            delegate {
                setInactive();
            },
            delegate {
                setActive();
            });

        
    }

}
