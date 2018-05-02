using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTimeLine : MonoBehaviour {
//A
    public TimeManagerScript timeManagerScript;

    public float actualTarget;
    public float previousTarget;
    public float ownTimeScale;
    public float scaledDT;


	// Use this for initialization
	void Start () {
        if (gameObject.layer != 9)
        {
            gameObject.layer = 9;
        }
    }
	
	// Update is called once per frame
	void Update () {

        actualTarget = timeManagerScript.timeScaleControl.targetValue;
        previousTarget = timeManagerScript.timeScaleControl.previousTargetValue;
        ownTimeScale = timeManagerScript.ownTimeScale;
        scaledDT = timeManagerScript.ownTimeScale * Time.deltaTime;

	}
}
