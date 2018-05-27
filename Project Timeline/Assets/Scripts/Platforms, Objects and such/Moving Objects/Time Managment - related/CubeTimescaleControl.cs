using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CubeTimescaleControl : MonoBehaviour {

    private Clock mainClock;
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
        mainClock = timeLine.clock;
    }

    private void LateUpdate()
    {      
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
                    mainClock.goRewind();
                    applied = true;
                }
                break;
            case 1:
                if (!applied)
                {
                    mainClock.goPause();
                    applied = true;
                }
                break;
            case 2:
                if (!applied)
                {
                    mainClock.goSlow();
                    applied = true;
                }
                break;
            case 3:
                if (!applied)
                {
                    mainClock.resetToNormal();
                    applied = true;
                }
                break;
            case 4:
                if (!applied)
                {
                    mainClock.goAccel();
                    applied = true;
                }
                break;
            default:
                if (!applied)
                {
                    mainClock.resetToNormal();
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
            mainClock.resetToNormal();
            applied = false;
        }
        
    }
}
