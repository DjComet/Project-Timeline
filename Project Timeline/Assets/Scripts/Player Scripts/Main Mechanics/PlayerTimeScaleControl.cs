using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//A
public static class PlayerEnergy {

    
    public static float energyAmount = 0.0f;
    public static float maxEnergyAmt = 5.0f;
    public static float minEnergyAmt = 0.0f;
    public static float energyRegenRate = 1.0f;
    [HideInInspector]
    public static Slider slider;

    public static float rewindReductionAmt = 0f;
    public static float pauseReductionAmt = 0.7f;
    public static float slowReductionAmt = 0.5f;
    public static float accelReductionAmt = 0.4f;
}

public class PlayerTimeScaleControl : MonoBehaviour {
    float dt;
    private Inputs inputs;
    private MainClock mainClock;

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
        mainClock = MainClock.mainClock;

        PlayerEnergy.energyAmount = PlayerEnergy.maxEnergyAmt;
        
    }
	
	// Update is called once per frame
	void Update () {
        dt = Time.deltaTime;

        acceleratedTimeValue = mainClock.timeValues[4];
        normalTimeValue = mainClock.timeValues[3];
        slowedTimeValue = mainClock.timeValues[2];
        pausedTimeValue = mainClock.timeValues[1];
        rewindTimeValue = mainClock.timeValues[0];

       

        //--------------------------------------------------Accelerated movement of the OwnTimescale SELECTOR-------------------
        
        //Input effects
        if (inputs.actionRight && inputs.rightClick)//ACCEL
        {
            mainClock.goAccel();
        } 
        else if(inputs.actionRight && inputs.leftClick)//SLOW
        {
            mainClock.goSlow();
        }

        if (inputs.actionLeft && inputs.rightClick && !trigger)//REWIND
        {
            mainClock.goRewind();    
        }
        else if (inputs.actionLeft && inputs.leftClick)//PAUSE
        {
            mainClock.goPause(); 
        }
        
        //Stuff to be able to rewind faster and faster if the rewind input stays pressed.

        if(inputs.actionLeftPressed && inputs.rightClickPressed && mainClock.rewindActivated)//Must do it better so that it can't be activated
        {
            mainClock.keepIncreasingValue = true;
        }
        else if(!inputs.actionLeftPressed || !inputs.rightClickPressed)
        {
            mainClock.keepIncreasingValue = false;
        }


        energyCalculation();
      
    }

    void energyCalculation()
    {
       switch(mainClock.i)
       {
           case 0:
                PlayerEnergy.energyAmount += PlayerEnergy.rewindReductionAmt * dt * mainClock.ownTimeScale;
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
                mainClock.accelActivated = false;
                mainClock.slowActivated = false;
                mainClock.pauseActivated = false;
                mainClock.rewindActivated = false;
                break;
       }
        PlayerEnergy.energyAmount = Mathf.Clamp(PlayerEnergy.energyAmount, PlayerEnergy.minEnergyAmt, PlayerEnergy.maxEnergyAmt);
        if(PlayerEnergy.energyAmount <= PlayerEnergy.minEnergyAmt)
        {
            mainClock.resetToNormal();
        }
    }
    
    
}
