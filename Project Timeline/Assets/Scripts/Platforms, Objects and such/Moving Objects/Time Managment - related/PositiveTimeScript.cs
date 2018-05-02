using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositiveTimeScript : MonoBehaviour {
    //A [SUFRIMIENTO]
    #region Variables Declaration
    public bool enableDebug;
    bool first = true;
    bool applied = true;
    private Rigidbody rb;
    private ObjectTimeLine objectTimeline;
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
        objectTimeline = gameObject.GetComponent<ObjectTimeLine>();
        rewindScript = gameObject.GetComponent<RewindScript>();
        timeScale = _timeScale;
    }


    void Update()
    {
        direction = Mathf.Sign(objectTimeline.actualTarget - objectTimeline.previousTarget);


        if(enableDebug)
        {
            Debug.Log("ownTimescale: " + objectTimeline.ownTimeScale);
            Debug.Log("Timescale: " + timeScale);
            Debug.Log("Velocity: " + rb.velocity);
            Debug.Log("AngVelocity: " + rb.angularVelocity);
        }
        
        //                        i=    0       1      2     3          4
        //Different TimeScale values: Rewind, Pause, Slow, Normal, Accelerated.

        float acceleratedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[4];
        float normalTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[3];
        float slowedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[2];
        float pausedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[1];
        float rewindTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[0];


        #region Calculations of TimeScale relative to the ownTimeScale values

        if (objectTimeline.ownTimeScale == normalTimeValue)
        {//             Normal
            rb.isKinematic = false;
            rb.useGravity = true;
            timeScale = 1;
        }//                                           0                                           1                                           1             
        else if (((objectTimeline.ownTimeScale > pausedTimeValue && objectTimeline.ownTimeScale < normalTimeValue) || objectTimeline.ownTimeScale > normalTimeValue))
        {//                                                     Slow                                                                            Fast         
            rb.useGravity = false;
            rb.isKinematic = false;

            if (objectTimeline.ownTimeScale > 0.01f)
            {
                                                        //This is to avoid crazy velocity spikes when the fraction x/0.0000y(<-TimeScale) returns something too big (when going back from 0 to normal time, the divisor is smaller than the dividend, 
                timeScale = objectTimeline.ownTimeScale;//and being the divisor smaller than 1, this causes extremely high numbers when very close to 0).
                temporalTS = timeScale;
            }
            

            if (enableDebug)
            {
                if (direction < 0)
                    Debug.Log("Slow Accel going to pause/rewind");
                else if (direction > 0) Debug.Log("Slow Accel coming from rewind");
            }
            
        }
        else if (objectTimeline.ownTimeScale == pausedTimeValue)
        {//                   Pause
            if(enableDebug) Debug.Log("Rigids are kinematic: We are in PAUSE");
            rb.isKinematic = true;
            applied = false;
        }
        #endregion


        #region Forces storage and application

        //Store rigidbody's velocity, angVelocity and mass before making it kinematic on pause...
        if (direction < 0 && objectTimeline.actualTarget == pausedTimeValue && objectTimeline.ownTimeScale > pausedTimeValue)
        {
            if (enableDebug) Debug.Log("Storing velocities for pause and stuff");
            previousVelocity = rb.velocity;
            previousAngVelocity = rb.angularVelocity;
            previousMass = rb.mass;
        }
        else if (direction > 0 && objectTimeline.previousTarget == pausedTimeValue)
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