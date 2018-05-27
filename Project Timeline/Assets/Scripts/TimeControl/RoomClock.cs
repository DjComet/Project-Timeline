using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomClock : MainClock {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;

        clockCore();

	}
}
