using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CubeTimescaleControl : MonoBehaviour {

    private Clock clock;
    private TimeLine timeLine;
    [HideInInspector]
    public ActivatorBehaviour activator;

    [Tooltip("This property indicates which time value this object will set on the clock it's associated with. The time array has 5 positions: " +
        "0 - Rewind; 1 - Pause; 2 - Slow; 3 - Normal; 4 - Accelerate.")]
    [Range(0, 4)] public int targetTimeArrayPosition;
    private bool applied = false;
    public bool jiviri = false;

    private void Start()
    {
        timeLine = GetComponent<TimeLine>();
        clock = timeLine.clock;
    }

    private void LateUpdate()
    {      
        if(clock == null)
        {
            clock = timeLine.clock;
        }
        if (timeLine.OnNormal() && jiviri)
        {

            activator.active = false;
        }

        if(activator != null)
        {
            if(activator.connectedTo != null)
            if (!activator.connectedTo.GetComponent<Linker>().active)
            {
                jiviri = false;
            }
        }
        
    }

    public void applyTime()
    {
        

        switch (targetTimeArrayPosition)//Podria evaluar si (!applied) fuera del switch pero bueno ahora ya está hecho y me da palo cambiarlo
        {
            case 0: if (!applied)
                {
                    clock.goRewind();
                    applied = true;
                    if (clock.i == 0)
                    {
                        clock.accelActivated = false;
                        clock.slowActivated = false;
                        clock.pauseActivated = false;
                        clock.rewindActivated = true;
                        clock.notSet = true;
                    }
                }
                break;
            case 1:
                if (!applied)
                {
                    clock.goPause();
                    applied = true;
                    if (clock.i == 1)
                    {
                        clock.accelActivated = false;
                        clock.slowActivated = false;
                        clock.pauseActivated = true;
                        clock.rewindActivated = false;
                    }
                }
                break;
            case 2:
                if (!applied)
                {
                    clock.goSlow();
                    applied = true;
                    if (clock.i == 2)
                    {
                        clock.accelActivated = false;
                        clock.slowActivated = true;
                        clock.pauseActivated = false;
                        clock.rewindActivated = false;
                    }
                }
                break;
            case 3:
                if (!applied)
                {
                    clock.resetToNormal();
                    applied = true;
                }
                break;
            case 4:
                if (!applied)
                {
                    clock.goAccel();
                    applied = true;
                    if (clock.i == 4)
                    {
                        clock.accelActivated = true;
                        clock.slowActivated = false;
                        clock.pauseActivated = false;
                        clock.rewindActivated = false;
                    }
                }
                break;
            default:
                if (!applied)
                {
                    clock.resetToNormal();
                    applied = true;
                }
                break;
        }
        jiviri = true;
    }

    public void normalizeTime()
    {
        if(applied)
        {
            clock.resetToNormal();
            applied = false;
        }
        
    }
}
