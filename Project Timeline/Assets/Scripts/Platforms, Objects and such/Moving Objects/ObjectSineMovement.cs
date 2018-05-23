using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSineMovement : MonoBehaviour {
    //A
    float dt;

    //Linear movement, sine movement, axis of movement
    private PlayerTimeScaleControl timeScale;
    private TimeLine objectTimeLine;
    private ApplyMovementToPlayer applyMovementToPlayer;
    public bool worldSpace;

    public float sineFrequency = 1f;
    public float sineMagnitude = 10f;
    public bool inverted = false;
    public bool isRight = true;
    public bool isUp = false;
    public bool isForward = false;
    float realtime;
    public float delay = 0f;
    Vector3 axis;
    Vector3 position;
    public float speedMagnitude = 0.0f;
    public Vector3 speedVector = Vector3.zero;


    // Use this for initialization
    void Start () {
        if (!gameObject.GetComponent<ApplyMovementToPlayer>())
        {
            gameObject.AddComponent<ApplyMovementToPlayer>();
        }
        applyMovementToPlayer = gameObject.GetComponent<ApplyMovementToPlayer>();

        objectTimeLine = gameObject.GetComponent<TimeLine>();
        position = transform.position;
        
        realtime = sineMagnitude / 2;
        realtime += delay;
        if (inverted)
            sineFrequency *= -1;
    }
	
	// Update is called once per frame
	void Update () {
        int right, up, forward; 
        right = (isRight ? 1 : 0);
        up = (isUp ? 1 : 0);
        forward = (isForward ? 1 : 0);

        if (objectTimeLine != null)
            dt = objectTimeLine.scaledDT;
        else
            dt = Time.deltaTime;

        if(worldSpace)
        {
            axis = Vector3.right * right + Vector3.up * up + Vector3.forward * forward;
        }
        else
        {
            axis = transform.right * right + transform.up * up + transform.forward * forward;
        }
        
        sine();

        applyMovementToPlayer.speedMagnitude = speedMagnitude ;
        applyMovementToPlayer.speedVector = speedVector ;
	}
    void sine()
    {
        realtime += dt;

        transform.position = position + axis * Mathf.Sin(realtime* 2 * Mathf.PI * sineFrequency) * sineMagnitude;
        speedMagnitude = 2 * Mathf.PI * sineFrequency * sineMagnitude * Mathf.Cos(2 * Mathf.PI * sineFrequency * realtime) * Mathf.Abs(objectTimeLine.ownTimeScale); //Derivada de la funcion seno. No queremos velocidades negativas al rebobinar así que cogemos el valo absoluto.
        speedVector = axis * speedMagnitude;
    }
}

