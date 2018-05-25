using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveTimeScript : RigidbodySettings {
    //A [SUFRIMIENTO]
    #region Variables Declaration
    public bool enableDebug;
    bool first = true;
    bool applied = true;
    
    private TimeLine timeLine;
    private RewindScript rewindScript;
    public float direction;

    private Vector3 previousVelocity;
    private Vector3 previousAngVelocity;
    private float previousMass;
    float temporalTS;


    [SerializeField]
    private float _timeScale = 1;
    public float timeScale //This is where the magic happens
    {
        get { return _timeScale; }
        set
        {
            if (!first)
            {
                    //rb.mass *= timeScale;
                    rb.velocity /= timeScale;
                    rb.angularVelocity /= timeScale;

                    if (enableDebug)
                    {
                        //Debug.Log("In Timescale-Mass: " + rb.mass);
                        Debug.Log("In Timescale-Velocity: " + rb.velocity);
                        Debug.Log("In Timescale-AngVelocity: " + rb.angularVelocity);
                    }
                
            }
            first = false;

            _timeScale = Mathf.Abs(value);
          
            //rb.mass /= timeScale;
            rb.velocity *= timeScale;
            rb.angularVelocity *= timeScale;
            
        }
    }
    #endregion

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        timeLine = gameObject.GetComponent<TimeLine>();
        rewindScript = gameObject.GetComponent<RewindScript>();
        timeScale = _timeScale;
    }


    void Update()
    {
        direction = Mathf.Sign(timeLine.actualTarget - timeLine.previousTarget);


        if(enableDebug)
        {
            Debug.Log("ownTimescale: " + timeLine.ownTimeScale);
            Debug.Log("Timescale: " + timeScale);
            Debug.Log("Velocity: " + rb.velocity);
            Debug.Log("AngVelocity: " + rb.angularVelocity);
        }
        
        //                        i=    0       1      2     3          4
        //Different TimeScale values: Rewind, Pause, Slow, Normal, Accelerated.

        float acceleratedTimeValue = timeLine.clock.timeValues[4];
        float normalTimeValue = timeLine.clock.timeValues[3];
        float slowedTimeValue = timeLine.clock.timeValues[2];
        float pausedTimeValue = timeLine.clock.timeValues[1];
        float rewindTimeValue = timeLine.clock.timeValues[0];


        #region Calculations of TimeScale relative to the ownTimeScale values

        if (timeLine.ownTimeScale == normalTimeValue)
        {//             Normal
            makeNotKinematic();
            makeGrav();
            timeScale = 1;
        }//                                           0                                           1                                           1             
        else if (((timeLine.ownTimeScale > pausedTimeValue && timeLine.ownTimeScale < normalTimeValue) || timeLine.ownTimeScale > normalTimeValue))
        {//                                                     Slow                                                                            Fast         
            makeIngrav();
            makeNotKinematic();

            if (timeLine.ownTimeScale > 0.01f)
            {
                                                        //This is to avoid crazy velocity spikes when the fraction x/0.0000y(<-TimeScale) returns something too big (when going back from 0 to normal time, the divisor is smaller than the dividend, 
                timeScale = timeLine.ownTimeScale;//and being the divisor smaller than 1, this causes extremely high numbers when very close to 0).
                temporalTS = timeScale;
            }
            

            if (enableDebug)
            {
                if (direction < 0)
                    Debug.Log("Slow Accel going to pause/rewind");
                else if (direction > 0) Debug.Log("Slow Accel coming from rewind");
            }
            
        }
        else if (timeLine.ownTimeScale == pausedTimeValue)
        {//                   Pause
            if(enableDebug) Debug.Log("Rigids are kinematic: We are in PAUSE");
            makeKinematic();
            applied = false;
        }
        #endregion


        #region Forces storage and application

        //Store rigidbody's velocity, angVelocity and mass before making it kinematic on pause...
        if (direction < 0 && timeLine.actualTarget == pausedTimeValue && timeLine.ownTimeScale > pausedTimeValue)
        {
            if (enableDebug) Debug.Log("Storing velocities for pause and stuff");
            previousVelocity = rb.velocity;
            previousAngVelocity = rb.angularVelocity;
            previousMass = rb.mass;
        }
        else if (direction > 0 && timeLine.previousTarget == pausedTimeValue)
        {//... and reapply them ONCE after no longer being paused or rewinded.
            if (!applied)
            {
                if (enableDebug) Debug.Log("Applying speeds");
                rb.velocity = previousVelocity;
                rb.angularVelocity = previousAngVelocity;
                rb.mass = previousMass;
                applied = true;
            }

        }
        
        /*if (!rewindScript.isRewinding)
        {
            if (enableDebug) Debug.Log("It's getting INSIDEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
            if (!rewindScript.hasAppliedStop)
            {
                if (enableDebug)
                {
                    Debug.Log("Applying Rewind Values-------------------------------------------------------REW---------------------------------------------------------------------------------");
                    Debug.Log("Velocity applied: " + rewindScript.pointsInTime[0].velocity);
                    Debug.Log("AngularVelocity applied: " + rewindScript.pointsInTime[0].angularVelocity);
                }
                rb.velocity = rewindScript.pointsInTime[0].velocity;
                rb.angularVelocity = rewindScript.pointsInTime[0].angularVelocity;//Apply forces at the end of rewind
                rewindScript.hasAppliedStop = true;
            }
        }*/
        #endregion


    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime * timeScale;
        rb.velocity += Physics.gravity * dt;
    }
}

//Podría escribir una tesis de 100 páginas explicando únicamente cómo funciona este script y la cantidad de m***** que tuve que probar para llegar a hacerlo funcionar en todos los casos.