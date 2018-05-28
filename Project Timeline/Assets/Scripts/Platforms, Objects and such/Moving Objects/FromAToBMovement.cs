using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromAToBMovement : MonoBehaviour {
    //A
    private TimeLine timeLine;
    public GameObject connectedTo;

    private Vector3 pointA;
    [Tooltip("Insert Point B here as Transform, if it exists")]
    public Transform pointBTransform;
    public Vector3 pointB;
    public float delay = 0.0f;

    
    private float t;
    private float timer = 0;
    private bool timerReachsDelay = false;
    private bool resetTimer = false;

    public float speed = 4.0f;
    [Tooltip("Starts and Stops with acceleration")]
    public bool easingIn_Out = false;

    private bool setOffTimer = false;
    public  bool active = false;
    private bool previousActive = false;

	// Use this for initialization
	void Start () {
        timeLine = GetComponent<TimeLine>();
        pointA = transform.position;
		if(pointBTransform != null)
        {
            pointB = pointBTransform.position;
        }
	}
	
	// Update is called once per frame
	void Update () {
        

        setActive();

        if(active)
        {
            if(previousActive!= active)
            {
                setOffTimer = true;
            }
            previousActive = active;
        }
        else
        {
            if (previousActive != active)
            {
                setOffTimer = true;
            }
            previousActive = active;
        }
        
        

        timer += Mathf.Abs(timeLine.scaledDT);
        timer = Mathf.Clamp(timer, 0, delay);

        if (timer >= delay)
        {
            timerReachsDelay = true;
        }

        if (setOffTimer)
        {
            timer = 0;
            timerReachsDelay = false;
            setOffTimer = false;
        }

        if (timerReachsDelay && active)
        {
            t += speed * Mathf.Abs(timeLine.scaledDT);
        }
        else if (timerReachsDelay && !active)
        {
            t -= speed * Mathf.Abs(timeLine.scaledDT);
        }
        t = Mathf.Clamp01(t);

        if (easingIn_Out)
        {
            transform.position = Vector3.Slerp(pointA, pointB, t);
        }
        else transform.position = Vector3.Lerp(pointA, pointB, t);



	}

    void setActive()
    {
        active = connectedTo.GetComponent<Linker>().active;
    }
}
