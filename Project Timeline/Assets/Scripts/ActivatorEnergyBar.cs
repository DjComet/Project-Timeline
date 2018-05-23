using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class ActivatorEnergyBar{

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
