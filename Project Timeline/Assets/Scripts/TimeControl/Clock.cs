using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour {
    protected float dt;

    public bool enableDebug = false;
    public bool resetToNormalTime = false;

    //************************************ TIME TARGETS, ARRAY AND CONTROL ***************************************************************************************************
    public float[] timeValues = new float[] { -1f, 0f, 0.2f, 1f, 2f};

    public float targetValue;
    public float ownTimeScale = 1.0f;

    protected float maxTimeMultiplierValue = 2.0f;//this should be equals to the value in the biggest position on the timeValues array (i = 4)
    protected float minTimeMultiplierValue = -1.0f;//same as above but with i = 0;
    public float previousTargetValue;
    public float increment = 0.5f;
    public float minValueToRewind = -1.0f;
    public float maxValueToRewind = -5.0f;
    public bool keepIncreasingValue = false;

    public int i = 3; //3 is the position of the value 1, which implies normal time flow (ownTimeScale is multiplied by 1).

    public bool notSet = true;
    protected bool trigger = false;
    bool canExtend = true;

    //************************************ TIME SELECTOR SLIDER **************************************************************************************************************
    public float maxSlidingSpeed = 1f;
    public float slidingAcceleration = 0.09f;
    public float slidingSpeed = 0.0f;

    //********************************** TIME ARRAY VALUES **********************************************
    public float acceleratedTimeValue;
    public float normalTimeValue;
    public float slowedTimeValue;
    public float pausedTimeValue;
    public float rewindTimeValue;


    //Important stuff to access from outside

    public static Clock mainClock;

    public float currentTime = 0;
    public float scaledDt;

    public bool accelActivated = false;
    public bool slowActivated = false;
    public bool pauseActivated = false;
    public bool rewindActivated = false;

    // Use this for initialization
    void Start () {
        timeValues = new float[] { -1f, 0f, 0.2f, 1f, 2f };
        mainClock = this;
        i = 3;//Normal Time Target Value
        targetValue = timeValues[i];
    }
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;

        clockCore();

    }

    protected void clockCore()
    {
        acceleratedTimeValue = timeValues[4];
        normalTimeValue = timeValues[3];
        slowedTimeValue = timeValues[2];
        pausedTimeValue = timeValues[1];
        rewindTimeValue = timeValues[0];

        keepRewind();

        i = Mathf.Clamp(i, 0, 4);
        targetValue = timeValues[i];


        ownTimescaleSlide();

        if (resetToNormalTime)
        {
            resetToNormal();
            resetToNormalTime = false;//Safeguard to not reset more than once
        }
        scaledDt = dt * ownTimeScale;
        currentTime += scaledDt;

        currentTime = Mathf.Clamp(currentTime, 0, Mathf.Infinity);
    }

    protected void keepRewind()
    {
        //Stuff to be able to rewind faster and faster if the rewind input stays pressed.

        if (keepIncreasingValue && trigger)//Must do it better so that it can't be activated
        {
            timeValues[0] += -increment * dt;
            timeValues[0] = Mathf.Clamp(timeValues[0], maxValueToRewind, minValueToRewind);
        }
        else if (!keepIncreasingValue)
        {
            trigger = false;
        }

    }

    protected void ownTimescaleSlide()
    {
        if (Mathf.Sign(targetValue - previousTargetValue) != Mathf.Sign(maxSlidingSpeed))//reverse maxSlidingSpeed sign if the direction has changed
            maxSlidingSpeed *= -1;


        if (targetValue >= pausedTimeValue && previousTargetValue == rewindTimeValue && notSet) //Everytime we go from rewind to anything else ownTimeScale is automatically set to 0, skipping negative numbers (accelerating from -1 to 0)
        {
            timeValues[0] = minValueToRewind;
            if (enableDebug) Debug.Log("Energy Exhausted, going to normal Time YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            trigger = false;
            slidingSpeed = 0;
            ownTimeScale = 0;
            notSet = false;
        }

        if (enableDebug)
        {
            Debug.Log("OwnTimeScale: " + ownTimeScale);
            Debug.Log("Target: " + targetValue);
            Debug.Log("MaxSlidingSpeed: " + maxSlidingSpeed);
            Debug.Log("slidingSpeed: " + slidingSpeed);
        }


        if (ownTimeScale != targetValue)
        {

            float offsetSpeed = maxSlidingSpeed - slidingSpeed;
            if (enableDebug) Debug.Log("offsetSpeed: " + offsetSpeed);
            offsetSpeed = Mathf.Clamp(offsetSpeed, -slidingAcceleration * dt, slidingAcceleration * dt);
            if (enableDebug) Debug.Log("offsetSpeedAfterClamp: " + offsetSpeed);
            slidingSpeed += offsetSpeed;

            ownTimeScale += slidingSpeed;
            if (maxSlidingSpeed > 0)
            { ownTimeScale = Mathf.Clamp(ownTimeScale, minTimeMultiplierValue, targetValue); }
            else if (maxSlidingSpeed < 0)
            { ownTimeScale = Mathf.Clamp(ownTimeScale, targetValue, maxTimeMultiplierValue); }

        }
        else slidingSpeed = 0;
    }

    public void  goAccel()
    {
        previousTargetValue = targetValue;
        i = 4;

        if (accelActivated && i == 4)
        {
            //Return to normal time if action is pressed again while active
            resetToNormal();

            accelActivated = false;
        }
        else if (i == 4)
        {
            accelActivated = true;
            slowActivated = false;
            pauseActivated = false;
            rewindActivated = false;
        }

    }

    public void goSlow()
    {
        previousTargetValue = targetValue;
        i = 2;

        if (slowActivated && i == 2)
        {
            //Return to normal time if action is pressed again while active
            resetToNormal();

            slowActivated = false;
        }
        else if (i == 2)
        {
            accelActivated = false;
            slowActivated = true;
            pauseActivated = false;
            rewindActivated = false;
        }
    }

    public void goPause()
    {
        previousTargetValue = targetValue;
        i = 1;

        if (pauseActivated && i == 1)
        {
            //Return to normal time if action is pressed again while active
            resetToNormal();

            pauseActivated = false;
        }
        else if (i == 1)
        {
            accelActivated = false;
            slowActivated = false;
            pauseActivated = true;
            rewindActivated = false;
        }
    }

    public void goRewind()
    {
        previousTargetValue = targetValue;
        i = 0;

        if (rewindActivated && i == 0)
        {
            //Return to normal time if action is pressed again while active
            resetToNormal();

            rewindActivated = false;
        }
        else if (i == 0)
        {
            accelActivated = false;
            slowActivated = false;
            pauseActivated = false;
            rewindActivated = true;
            notSet = true;
        }
    }

    public void resetToNormal()
    {
        i = 3;
        previousTargetValue = targetValue;
    }
}
