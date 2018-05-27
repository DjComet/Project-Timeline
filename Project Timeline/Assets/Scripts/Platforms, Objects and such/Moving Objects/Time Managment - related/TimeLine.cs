using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLine : MonoBehaviour {
    //A
    public MainClock clock;

    public bool enableDebug = false;

    public float actualTarget;
    public float previousTarget;
    public float ownTimeScale;
    public float scaledDT;

    public List<TimeEvent> timeEvents = new List<TimeEvent>();

    [HideInInspector]
    public TimeEvent currentlyCycledTE;
    private float maxRewindTime = RewindScript.recordTime;

    // Use this for initialization
    void Start () {
        if (gameObject.layer != 9)
        {
            gameObject.layer = 9;
        }

        clock = transform.parent.Find("TimeManager").GetComponent<RoomClock>();
    }
	
	// Update is called once per frame
	void Update () {

        actualTarget = clock.targetValue;
        previousTarget = clock.previousTargetValue;
        ownTimeScale = clock.ownTimeScale;
        scaledDT = clock.scaledDt;


        cycle();
	}

    #region Time state related bool functions
    public bool OnPause()
    {
        return (clock.targetValue == clock.pausedTimeValue ? true : false);
    }
    public bool OnSlow()
    {
        return (clock.targetValue == clock.slowedTimeValue ? true : false);
    }
    public bool OnRewind()
    {
        return (clock.targetValue == clock.rewindTimeValue ? true : false);
    }
    public bool OnNormal()
    {
        return (clock.targetValue == clock.normalTimeValue ? true : false);      
    }

    public bool OnAccel()
    {
        return (clock.targetValue == clock.acceleratedTimeValue ? true : false);
    }
    #endregion

    #region TimeEvent code and functions
    public void createTimeEvent(bool repeatable, TimeEvent.Forward forward, TimeEvent.Backwards backwards)
    {   
        timeEvents.Add(new TimeEvent(repeatable, clock.currentTime, forward, backwards));
    }

    /*public void createTimeEvent(bool repeatable, TimeEvent.ForwardFunc<GameObject,GameObject> forward, TimeEvent.Backwards backwards)
    {
        timeEvents.Add(new TimeEvent(repeatable, clock.currentTime, forward, backwards));
    }*/

    public void planTimeEvent(bool repeatable, float time, TimeEvent.Forward forward, TimeEvent.Backwards backwards)
    {
        timeEvents.Add(new TimeEvent(repeatable, clock.currentTime + time, forward, backwards));
    }

    public void scheduleTimeEvent(bool repeatable, float time, TimeEvent.Forward forward, TimeEvent.Backwards backwards)
    {
        timeEvents.Add(new TimeEvent(repeatable, time, forward, backwards));
    }

    public void memorizeTimeEvent(bool repeatable, TimeEvent.Forward forward, TimeEvent.Backwards backwards)
    {
        timeEvents.Add(new TimeEvent(repeatable, clock.currentTime, forward, backwards));
    }

    void cycle()//This cycles through all the time events in this gameObject, and activates the forwards or backwards functions depending on the time of the event relative to the current time.               
    {           //It also erases events when their time is out of the scope of the rewind record time.
        for (int i = timeEvents.Count - 1; i >= 0; i--)
        {
            currentlyCycledTE = currentCycledTimeEvent(timeEvents[i]);
            //Si el segundo actual es <= que el del time event y estamos por debajo de 0 (rebobinando)
            if(clock.currentTime <= timeEvents[i].time && ownTimeScale < clock.timeValues[1] && !timeEvents[i].hasRepeatedBwd)
            {
                if(enableDebug)Debug.Log("Ha llegado en rebobinado a un time event");
                if (timeEvents[i].backwards != null)
                {
                    timeEvents[i].backwards();//Do the backwards function
                    
                    timeEvents[i].hasRepeatedFwd = false;
                }
                    timeEvents[i].hasRepeatedBwd = true;

                if (!timeEvents[i].repeatable)
                {             
                    erase(i);
                }
            }

            //Si el segundo actual es superior al almacenado en el time Event y estamos por encima de 0 (tiempo positivo)
            if (clock.currentTime >= timeEvents[i].time && ownTimeScale > clock.timeValues[1] && !timeEvents[i].hasRepeatedFwd)
            {
                if(enableDebug)Debug.Log("Ha llegado en tiempo positivo a un time event");
                timeEvents[i].forward();
                timeEvents[i].hasRepeatedBwd = false;
                timeEvents[i].hasRepeatedFwd = true;
            }

            if(clock.currentTime - timeEvents[i].time > maxRewindTime)
            {
                erase(i);
            }
        }
    }
    
    public TimeEvent currentCycledTimeEvent(TimeEvent t )
    {
        return t;
    }

    public bool isEventOverlapping()
    {
        if (currentlyCycledTE.time <= 0.02 + clock.currentTime || currentlyCycledTE.time >= 0.02 + clock.currentTime) return false;
        else return true;
    }

    #region ERASE TIME EVENT
    void erase(int i)
    {
        if (enableDebug) Debug.Log(i + " timeEvent removed");
        timeEvents.RemoveAt(i);
    }

    void erase(TimeEvent timeEvent)
    {
        timeEvents.Remove(timeEvent);
    }
    #endregion

#endregion
}
