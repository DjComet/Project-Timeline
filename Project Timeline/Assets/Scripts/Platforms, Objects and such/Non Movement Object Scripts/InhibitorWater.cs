using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhibitorWater : MonoBehaviour {
    //A
    private MainClock mainClock;


	// Use this for initialization
	void Start () {
        mainClock = MainClock.mainClock;

	}
	
	// Update is called once per frame
	void Update () {
		


	}
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            mainClock.resetToNormalTime = true;
        }
    }
}
