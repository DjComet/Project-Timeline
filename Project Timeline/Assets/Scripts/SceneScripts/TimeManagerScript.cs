using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour {
    //A
    protected Inputs inputs;
    protected GameObject player;
    public TimeScaleControl timeScaleControl;

    public float currentTime;
    public float ownTimeScale;
    public float[] timeValues;
    public int i_timeValues;

    // Use this for initialization
    void Start () {
        
        player = GameObject.FindGameObjectWithTag("Player");
        inputs = player.GetComponent<Inputs>();
        timeScaleControl = player.GetComponent<TimeScaleControl>();
        timeValues = timeScaleControl.timeValues;
        i_timeValues = timeScaleControl.i;
	}
	
	// Update is called once per frame
	void Update () {
        ownTimeScale = timeScaleControl.ownTimeScale;
        currentTime += ownTimeScale * Time.deltaTime;

        currentTime = Mathf.Clamp(currentTime, 0, Mathf.Infinity);
	}
}
