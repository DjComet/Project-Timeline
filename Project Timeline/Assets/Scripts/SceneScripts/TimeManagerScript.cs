﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagerScript : MonoBehaviour {
    //A
    protected Inputs inputs;
    protected GameObject player;
    public Clock mainClock;

    public static TimeManagerScript timeManager;

    public float currentTime;
    public float ownTimeScale;
    public float[] timeValues;
    public int i_timeValues;

    private void Awake()
    {
        timeManager = this;
    }

    // Use this for initialization
    void Start() {
        
        player = GameObject.FindGameObjectWithTag("Player");
        inputs = player.GetComponent<Inputs>();
        mainClock = GetComponent<Clock>();
        timeValues = mainClock.timeValues;
        i_timeValues = mainClock.i;
    }

    // Update is called once per frame
    void Update() {
        ownTimeScale = mainClock.ownTimeScale;
        currentTime += ownTimeScale * Time.deltaTime;

        currentTime = Mathf.Clamp(currentTime, 0, Mathf.Infinity);
    }

}
