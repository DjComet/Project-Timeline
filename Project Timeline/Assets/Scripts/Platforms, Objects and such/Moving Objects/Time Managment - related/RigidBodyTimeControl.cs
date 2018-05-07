using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public struct PointInTime
{

    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly Vector3 velocity;
    public readonly Vector3 angularVelocity;
    public readonly float timeWhenRecorded;
    public readonly int number;

    /*
    public PointInTime(Transform t, Vector3 v, Vector3 angV, float cT, int n)
    {
        position = t.position;
        rotation = t.rotation;
        velocity = v;
        angularVelocity = angV;
        timeWhenRecorded = cT;
        number = n;
    }
}*/

public class RigidBodyTimeControl : MonoBehaviour {

    #region Variables Declaration
    private Rigidbody rb;
    private ObjectTimeLine objectTimeline;
    private PositiveTimeScript positiveTimeScript;


    public bool enableDebug = false;

    //For Rewind <[][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][REW][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]>
    //General Variables----------------------------------------------------------------------------------------
    private float recordInterval = 0.1f;
    public List<PointInTime> pointsInTime;


    private float currentTime;
    public bool isRewinding = false;
    public float recordTime = 5f;
    public bool hasAppliedStop = true;
    bool applied = true;
    public float counter = 0;
    private float t = 0;
    private bool canInitializeLerp = true;

    int number = 0;//Debugging variable

    //Lerping stuff--------------------------------------------------------------------------------------------
    Vector3 initialPos;
    Quaternion initialRot;
    float initialTime;
    float targetTime;
    


    //For pause to accel movement  <[][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][POS][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][][]>
    bool first = true;
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
        pointsInTime = new List<PointInTime>();
        rb = gameObject.GetComponent<Rigidbody>();
        objectTimeline = gameObject.GetComponent<ObjectTimeLine>();
        counter = 0;
        pointsInTime.Insert(0, new PointInTime(transform, rb.velocity, rb.angularVelocity, currentTime, number));
        timeScale = _timeScale;
    }

   
	
	// Update is called once per frame
	void Update () {
        float acceleratedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[4];
        float normalTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[3];
        float slowedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[2];
        float pausedTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[1];
        float rewindTimeValue = objectTimeline.timeManagerScript.timeScaleControl.timeValues[0];


        currentTime = objectTimeline.timeManagerScript.currentTime;

        if (objectTimeline.ownTimeScale < pausedTimeValue)
            isRewinding = true;
        else if (objectTimeline.actualTarget != rewindTimeValue)
        {
            isRewinding = false;
        }



        if (isRewinding)
        {
            rb.isKinematic = true;
            Rewind();
            hasAppliedStop = false;

        }
        else if (!isRewinding)
        {
            if (!hasAppliedStop)
            {
                StopRewind();
                hasAppliedStop = true;
            }
            Record();

        }


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
            if (enableDebug) Debug.Log("Rigids are kinematic: We are in PAUSE");
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
 
        #endregion




    }



    #region Rewind Functions    

    void Record()
    {


        if (counter >= recordInterval)
        {
            pointsInTime.Insert(0, new PointInTime(transform, rb.velocity, rb.angularVelocity, currentTime, number));
            if (enableDebug) Debug.Log("PointInTimeInserted: pos " + pointsInTime[0].position + " vel " + pointsInTime[0].velocity + " ang vel " + pointsInTime[0].angularVelocity + " timeWhenRecorded " + pointsInTime[0].timeWhenRecorded + " number " + pointsInTime[0].number);

            if (currentTime - pointsInTime[pointsInTime.Count - 1].timeWhenRecorded > recordTime)//If the time elapsed between NOW and the last element on the list is greater than 5 seconds, delete it.
            {
                if (enableDebug) Debug.Log("PointRemoved with time: " + pointsInTime[pointsInTime.Count - 1].timeWhenRecorded);
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }

            number += 1;
            counter = 0;

        }
        counter += Mathf.Abs(objectTimeline.scaledDT);//Abs is a safeguard against negative scaledDeltaTimes when coming back from rewind to normal time, which shouldn't happen but hey.
    }

    void Rewind()
    {
        if (pointsInTime.Count > 1)
        {

            PointInTime pointInTime = pointsInTime[0];

            //First take the initial position and time and the target position and time
            if (currentTime != pointInTime.timeWhenRecorded && canInitializeLerp)
            {
                initialPos = transform.position;
                initialRot = transform.rotation;
                initialTime = currentTime;
                canInitializeLerp = false;
                t = 0;//t must be zero every time we begin a new lerp section
                
            }

            //Calculate T as a fraction of the current Time - initialTime divided by the time between the points A & B of the lerp. the variable Lerper can be replaced by currentTime, and the result is the same, 
            //although I think this way I have more control over what goes into the division. Maybe I'm wrong.

            t = (Mathf.Abs(currentTime - initialTime)) / Mathf.Abs(pointInTime.timeWhenRecorded - initialTime);//Calculation for t
            t = Mathf.Clamp01(t);//just in case

            if (enableDebug) Debug.Log("t:" + t);

            transform.position = Vector3.Lerp(initialPos, pointInTime.position, t);//Actual lerp for position and rotation, maybe i should lerp the velocity and the angVelocity too for smoother results... 
            transform.rotation = Quaternion.Slerp(initialRot, pointInTime.rotation, t);//Although you can't barely notice the difference with a record interval of 0.1 secs.

            if (pointsInTime != null && currentTime <= pointInTime.timeWhenRecorded)
            {
                if (enableDebug) Debug.Log("PointInTimeRemoved: pos " + pointsInTime[0].position + " vel " + pointsInTime[0].velocity + " ang vel " + pointsInTime[0].angularVelocity + " timeWhenRecorded " + pointsInTime[0].timeWhenRecorded + " number " + pointsInTime[0].number);
                pointsInTime.RemoveAt(0);
                t = 0;
                
                number -= 1;
                canInitializeLerp = true;
            }

        }
        else
        {
            PointInTime pointInTime = pointsInTime[0];

        }


    }

    public void StartRewind()
    {
        isRewinding = true;
    }

    public void StopRewind()
    {

        rb.isKinematic = false;
        ReapplyForces();
        canInitializeLerp = true;//Super important
    }

    void ReapplyForces()
    {
        if (enableDebug) Debug.Log("Applying Velocity:" + pointsInTime[0].velocity + "YOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        if (enableDebug) Debug.Log("Applying AngularVelocity:" + pointsInTime[0].angularVelocity);

        rb.velocity = pointsInTime[0].velocity * timeScale;//Multiplying by the timescale so that when we come back from rewind and timeScale is less than 1, it doesn't get multiplied to a very high number.
        rb.angularVelocity = pointsInTime[0].angularVelocity * timeScale;
        //Let's say we reapply forces here and velocity is 7, that 7 will later on the same frame be divided by timeScale on the positiveTimeScript, which will be
        //something like 0.09. 7/0.09 is 77, a lot more than what we want, and next frame it will be even bigger. Now, if instead we first multiply 7 by 0.09, and then it gets
        //carried on to the positive time script with that small value, it will get multiplied by 0.09 again which will return exactly the number we want for both vel and angVel.
    }

    #endregion

    void timeScaleCalculation()
    {

    }

    void secondaryForceStorage()
    {

    }

}
