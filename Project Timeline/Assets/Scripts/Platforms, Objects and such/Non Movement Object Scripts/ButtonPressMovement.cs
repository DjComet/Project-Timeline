using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressMovement : MonoBehaviour {
    //A
    float scaledDt;

    private ObjectTimeLine objectTimeLine;
    private Linker linker;

    public Transform button;
    public Transform buttonBase;

    public float pressSpeed = 10.0f;

    Vector3 initialPos;
    Vector3 targetPos;
    public float t = 0;

    public bool active = false;

	// Use this for initialization
	void Start () {
        objectTimeLine = gameObject.GetComponent<ObjectTimeLine>();
        linker = gameObject.GetComponent<Linker>();
        initialPos = button.position;
        targetPos = buttonBase.position;
	}
	
	// Update is called once per frame
	void Update () {
        scaledDt = objectTimeLine.scaledDT;
        t = Mathf.Clamp01(t);
        linker.active = active;

        if (active)
        {          
            t += scaledDt * pressSpeed;   
        }
        else
        {   
            t -= scaledDt * pressSpeed;
        }

        button.position = Vector3.Lerp(initialPos, targetPos, t);
	}

    private void OnTriggerEnter(Collider other)
    {
        active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        active = false;
    }
}
