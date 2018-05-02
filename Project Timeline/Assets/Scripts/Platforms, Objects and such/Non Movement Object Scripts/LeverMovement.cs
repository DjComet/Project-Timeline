using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverMovement : MonoBehaviour {
    //A
    private ObjectTimeLine objectTimeLine;
    private Linker linker;

    public Transform lever;

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

        objectTimeLine = GetComponent<ObjectTimeLine>();
        linker = GetComponent<Linker>();

        initialRot = lever.localRotation;
        targetRot = Quaternion.Euler(targetAngle, 0, 0);
        

    }
	
	// Update is called once per frame
	void Update () {

        setActive();

        t = Mathf.Clamp01(t);

        if(leverTime > 0.0f)
        {
            if (active)
            {
                timer += objectTimeLine.scaledDT;

                if (timer >= leverTime)
                {
                    active = false;
                    timer = 0;
                }
            }
            else timer = 0;
        }

        if (active)
        {
            t += lerpSpeed * objectTimeLine.scaledDT;
            linker.active = true;
        }
        else
        {
            t -= lerpSpeed * objectTimeLine.scaledDT;
            linker.active = false;
            timer = 0;//redundancy just in case
        }


        lever.localRotation = Quaternion.Slerp(initialRot, targetRot, t);

	}

    void setActive()
    {
        if(linker.active)
        {
            active = true;
        }
        else
        {
            active = false;
        }
    }
}
