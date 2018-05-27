using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//A
public static class PlayerEnergy {

    
    public static float energyAmount = 0.0f;
    public static float maxEnergyAmt = 5.0f;
    public static float minEnergyAmt = 0.0f;
    public static float energyRegenRate = 0.5f;
    [HideInInspector]
    public static Slider slider;

    public static float rewindReductionAmt = 0.5f;
    public static float pauseReductionAmt = 0.4f;
    public static float slowReductionAmt = 0.25f;
    public static float accelReductionAmt = 0.2f;
}

public class PlayerTimeScaleControl : MonoBehaviour {
    float dt;
    private Inputs inputs;
    public Clock clock;

    public bool hasModifiedTime = false; //Useful to know if this object has modified time, or it was some other object.
    public bool enableDebug = false;

    //************************************ TIME TARGET BOOLEANS **************************************************************************************************************
    public bool notSet = true;
    private bool trigger = false;
    bool canExtend = true;

    //********************************** TIME ARRAY VALUES **********************************************
    public float acceleratedTimeValue;
    public float normalTimeValue;
    public float slowedTimeValue;
    public float pausedTimeValue;
    public float rewindTimeValue;

    // Use this for initialization
    void Start () {
        
        inputs = gameObject.GetComponent<Inputs>();
        clock = Clock.mainClock;

        PlayerEnergy.energyAmount = PlayerEnergy.maxEnergyAmt;
        
    }
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;

        acceleratedTimeValue = clock.timeValues[4];
        normalTimeValue = clock.timeValues[3];
        slowedTimeValue = clock.timeValues[2];
        pausedTimeValue = clock.timeValues[1];
        rewindTimeValue = clock.timeValues[0];

       

        //--------------------------------------------------Accelerated movement of the OwnTimescale SELECTOR-------------------
        
        //Input effects
        if (inputs.actionRight && inputs.rightClick)//ACCEL
        {
            clock.goAccel();            
        } 
        else if(inputs.actionRight && inputs.leftClick)//SLOW
        {
            clock.goSlow();
        }

        if (inputs.actionLeft && inputs.rightClick && !trigger)//REWIND
        {
            clock.goRewind();    
        }
        else if (inputs.actionLeft && inputs.leftClick)//PAUSE
        {
            clock.goPause();
            Debug.Log("Pause click");
        }
        
        //Stuff to be able to rewind faster and faster if the rewind input stays pressed.

        if(inputs.actionLeftPressed && inputs.rightClickPressed && clock.rewindActivated)//Must do it better so that it can't be activated
        {
            clock.keepIncreasingValue = true;
        }
        else if(!inputs.actionLeftPressed || !inputs.rightClickPressed)
        {
            clock.keepIncreasingValue = false;
        }


        energyCalculation();
      
    }

    void energyCalculation()
    {
       switch(clock.i)
       {
           case 0:
                PlayerEnergy.energyAmount += PlayerEnergy.rewindReductionAmt * dt * clock.ownTimeScale;
                if (enableDebug) Debug.Log("Energy Amount: " + PlayerEnergy.energyAmount);
               break;
           case 1:
               PlayerEnergy.energyAmount -= PlayerEnergy.pauseReductionAmt * dt;
               break;
           case 2:
               PlayerEnergy.energyAmount -= PlayerEnergy.slowReductionAmt * dt;
               break;
           case 4:
               PlayerEnergy.energyAmount -= PlayerEnergy.accelReductionAmt * dt;
               break;
           case 3:
               PlayerEnergy.energyAmount += PlayerEnergy.energyRegenRate * dt;
                clock.accelActivated = false;
                clock.slowActivated = false;
                clock.pauseActivated = false;
                clock.rewindActivated = false;
                break;
       }
        PlayerEnergy.energyAmount = Mathf.Clamp(PlayerEnergy.energyAmount, PlayerEnergy.minEnergyAmt, PlayerEnergy.maxEnergyAmt);
        if(PlayerEnergy.energyAmount <= PlayerEnergy.minEnergyAmt)
        {
            clock.resetToNormal();
        }
    }
    
    
}
