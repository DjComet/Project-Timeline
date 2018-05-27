using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeEvent {



    public bool repeatable;
    public bool hasRepeatedFwd = false;
    public bool hasRepeatedBwd = false;
    
    public float time;
    

    public delegate void Forward();
    public delegate void Backwards();
    public delegate T ForwardFunc<T,G>( G input);
    public delegate T BackwardsFunc<T,G>(G input);
    
    public Forward forward;    //Function to play when Time event is active and timeline is moving forward.
    public Backwards backwards;//Function to play when Time event is active and timeline is moving backwards.
   
    
    

    public TimeEvent(bool repeatable, float time, Forward forward, Backwards backwards)
    {
        this.repeatable = repeatable;
        this.time = time;

        this.forward = forward;
        this.backwards = backwards;
        
    }

    /*
    public TimeEvent(bool repeatable, float time, ForwardFunc<> forward, BackwardsFunc<> backwards)
    {
        this.repeatable = repeatable;
        this.time = time;

        this.forward = forward;
        this.backwards = backwards;

    }*/ //hmmm



}
