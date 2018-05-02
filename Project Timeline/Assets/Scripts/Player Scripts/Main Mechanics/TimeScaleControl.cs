using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//A
public class EnergyMeter {

    
    public float energyAmount = 0.0f;
    public float maxEnergyAmt = 5.0f;
    public float minEnergyAmt = 0.0f;
    public float energyRegenRate = 1.0f;
    [HideInInspector]
    public Slider slider;

    public float rewindReductionAmt = 1.0f;
    public float pauseReductionAmt = 0.7f;
    public float slowReductionAmt = 0.5f;
    public float accelReductionAmt = 0.4f;
}

public class TimeScaleControl : MonoBehaviour {
    float dt;
    private Inputs inputs;
    public EnergyMeter energy;
    public bool enableDebug = false;
    public bool resetToNormalTime = false;

    //************************************ TIME TARGETS, ARRAY AND CONTROL ***************************************************************************************************
    public float[] timeValues;
    public float targetValue;
    public float ownTimeScale = 1.0f;
    private float maxTimeMultiplierValue = 2.0f;//this should be equals to the value in the biggest position on the timeValues array (i = 4)
    private float minTimeMultiplierValue = -1.0f;//same as above but with i = 0;
    public float previousTargetValue;
    public float increment = 0.5f;
    public float minValueToRewind = -1.0f;
    public float maxValueToRewind = -5.0f;

    public int i = 3; //3 is the position of the value 1, which implies normal time flow (ownTimeScale is multiplied by 1).

    //************************************ TIME SELECTOR SLIDER **************************************************************************************************************
    public float maxSlidingSpeed = 1f;
    public float slidingAcceleration = 2.5f;
    public float slidingSpeed = 0.0f;


    //************************************ TIME TARGET BOOLEANS **************************************************************************************************************
    public bool accelHasBeenPressed = false;
    public bool slowHasBeenPressed = false;
    public bool pauseHasBeenPressed = false;
    public bool rewindHasBeenPressed = false;
    public bool notSet = true;
    private bool trigger = false;
    bool canExtend = true;

    // Use this for initialization
    void Start () {
        i = 3;//Normal Time Target Value
        inputs = gameObject.GetComponent<Inputs>();
        energy = new EnergyMeter();
        energy.energyAmount = energy.maxEnergyAmt;
        targetValue = timeValues[i];
    }
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;

        float acceleratedTimeValue = timeValues[4];
        float normalTimeValue = timeValues[3];
        float slowedTimeValue = timeValues[2];
        float pausedTimeValue = timeValues[1];
        float rewindTimeValue = timeValues[0];

        minTimeMultiplierValue = rewindTimeValue;
        maxTimeMultiplierValue = acceleratedTimeValue;

        //--------------------------------------------------Accelerated movement of the OwnTimescale SELECTOR-------------------

        if (inputs.actionRight && inputs.rightClick)//ACCEL
        {
            previousTargetValue = targetValue;
            //i = 3;
            i = 4;
            if (accelHasBeenPressed && i == 4)
            {   //Return to normal time if action is pressed again while active
                previousTargetValue = targetValue;
                i = 3;
                accelHasBeenPressed = false;
            }
            else if (i == 4)
            {
                accelHasBeenPressed = true;
                slowHasBeenPressed = false;
                pauseHasBeenPressed = false;
                rewindHasBeenPressed = false;
            }
        } 
        else if(inputs.actionRight && inputs.leftClick)//SLOW
        {
            previousTargetValue = targetValue;
            //i = 3;
            i = 2;//previously i += 1;
            if (slowHasBeenPressed && i == 2)
            {   //Return to normal time if action is pressed again while active
                previousTargetValue = targetValue;
                i = 3;
                slowHasBeenPressed = false;
            }
            else if (i == 2)
            {
                accelHasBeenPressed = false;
                slowHasBeenPressed = true;
                pauseHasBeenPressed = false;
                rewindHasBeenPressed = false;
            }
        }

        if (inputs.actionLeft && inputs.rightClick && !trigger)//REWIND
        {
            
            previousTargetValue = targetValue;
            trigger = true;
            //i = 3;
            i = 0;
            if (rewindHasBeenPressed && i == 0)
            {   //Return to normal time if action is pressed again while active
                previousTargetValue = targetValue;
                i = 3;
                
                rewindHasBeenPressed = false;
            }
            else if (i == 0)
            {
                accelHasBeenPressed = false;
                slowHasBeenPressed = false;
                pauseHasBeenPressed = false;
                rewindHasBeenPressed = true;
                notSet = true;
            }
        }
        else if (inputs.actionLeft && inputs.leftClick)//PAUSE
        {
            previousTargetValue = targetValue;
            //i = 3;
            i = 1;//previously i-= 1;
            if (pauseHasBeenPressed && i == 1)
            {   //Return to normal time if action is pressed again while active
                previousTargetValue = targetValue;
                i = 3;
                pauseHasBeenPressed = false;
            }
            else if (i == 1)
            {
                accelHasBeenPressed = false;
                slowHasBeenPressed = false;
                pauseHasBeenPressed = true;
                rewindHasBeenPressed = false;
            }
        }
        
        if(inputs.actionLeftPressed && inputs.rightClickPressed && trigger && rewindHasBeenPressed)//Must do it better so that it can't be activated
        {
            
            timeValues[0] += -increment * dt;
            timeValues[0] = Mathf.Clamp(timeValues[0], maxValueToRewind, minValueToRewind);
        }
        else if(!inputs.actionLeftPressed || !inputs.rightClickPressed)
        {
            trigger = false;
        }

        i = Mathf.Clamp(i, 0, 4);
        targetValue = timeValues[i];


        
        if (Mathf.Sign(targetValue - previousTargetValue) != Mathf.Sign(maxSlidingSpeed))//reverse maxSlidingSpeed sign if the direction has changed
            maxSlidingSpeed *= -1;


        if(targetValue >= pausedTimeValue && previousTargetValue == rewindTimeValue && notSet) //Everytime we go from rewind to anything else ownTimeScale is automatically set to 0, skipping negative numbers (accelerating from -1 to 0)
        {
            timeValues[0] = minValueToRewind;
            if(enableDebug)Debug.Log("Energy Exhausted, going to normal Time YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
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

        energyCalculation();

        if(resetToNormalTime)
        {
            resetToNormal();
            resetToNormalTime = false;//Safeguard to not reset more than once
        }
        
    }

    void energyCalculation()
    {
       switch(i)
       {
           case 0:
                energy.energyAmount += energy.rewindReductionAmt * dt * ownTimeScale;
                if (enableDebug) Debug.Log("Energy Amount: " + energy.energyAmount);
               break;
           case 1:
               energy.energyAmount -= energy.pauseReductionAmt * dt;
               break;
           case 2:
               energy.energyAmount -= energy.slowReductionAmt * dt;
               break;
           case 4:
               energy.energyAmount -= energy.accelReductionAmt * dt;
               break;
           case 3:
               energy.energyAmount += energy.energyRegenRate * dt;
                accelHasBeenPressed = false;
                slowHasBeenPressed = false;
                pauseHasBeenPressed = false;
                rewindHasBeenPressed = false;
                break;
       }
        energy.energyAmount = Mathf.Clamp(energy.energyAmount, energy.minEnergyAmt, energy.maxEnergyAmt);
        if(energy.energyAmount <= energy.minEnergyAmt)
        {
            resetToNormal();
        }
    }
    
    void resetToNormal()
    {
        i = 3;

        previousTargetValue = targetValue;
    }
}
