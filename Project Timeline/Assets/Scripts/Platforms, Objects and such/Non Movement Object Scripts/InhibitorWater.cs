using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhibitorWater : MonoBehaviour {
    //A
    private TimeScaleControl timeScaleControl;


	// Use this for initialization
	void Start () {
        timeScaleControl = GameObject.FindGameObjectWithTag("Player").GetComponent<TimeScaleControl>();

	}
	
	// Update is called once per frame
	void Update () {
		


	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            timeScaleControl.resetToNormalTime = true;
        }
    }
}
