using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressMovement : MonoBehaviour {
    //A
    float scaledDt;

    private TimeLine timeLine;
    private Linker linker;
    public List<Collider> colliders = new List<Collider>();

    public Transform button;
    public Transform buttonBase;

    public float pressSpeed = 10.0f;

    Vector3 initialPos;
    Vector3 targetPos;
    public float t = 0;

    public bool active = false;

	// Use this for initialization
	void Start () {
        timeLine = gameObject.GetComponent<TimeLine>();
        linker = gameObject.GetComponent<Linker>();
        initialPos = button.position;
        targetPos = buttonBase.position;
	}
	
	// Update is called once per frame
	void Update () {
        scaledDt = timeLine.scaledDT;
        t = Mathf.Clamp01(t);
        linker.active = active;

        if (active)
        {          
            t += Mathf.Abs(scaledDt) * pressSpeed;   
        }
        else
        {   
            t -= Mathf.Abs(scaledDt) * pressSpeed;
        }

        button.position = Vector3.Lerp(initialPos, targetPos, t);
	}

    private void OnTriggerEnter(Collider other)
    {
        
            if (!timeLine.isEventOverlapping())
            {//Create event where forward = setActive && backwards = setInactive. Repeatable = true.
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
                colliders.Add(other);
                
            }
        
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        //Create event where forward = setInactive && backwards = setActive. Repeatable = true.
        //timeLine.createTimeEvent(/*  args  */);
        for(int i = colliders.Count -1; i>=0; i--)
        {
            if(colliders[i] == other)
            {
                colliders.RemoveAt(i);
            }
        }

        if (colliders.Count == 0 && !timeLine.isEventOverlapping())
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
            
            
        }
        
    }

    public void setActive()
    {
        active = true;
    }

    public void setInactive()
    {
        active = false;
    }
}
