using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tramMovement : MonoBehaviour {
    //A
    #region public
    public float accel = 0.0f;
    public float maxSpeed = 0.0f;
    public float angularMult = 0.0f;
    public float speed = 0;

    public Transform[] wayPoints;
    #endregion

    #region private;
    private int wayPointsIndex = 0;
    private float angleSpeed = 0;
    private float direction = 0;
    private float lastDirection = 1;
    private float dt;

    #endregion

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        dt = Time.deltaTime;

        updateSpeed();

        if (inTarget() && speed >= 0) {
            wayPointsIndex = (wayPointsIndex + 1) % wayPoints.Length;
        }
        if (inTarget() && speed < 0)
        {
            wayPointsIndex = (wayPointsIndex + wayPoints.Length - 1) % wayPoints.Length;
        }
        else
        {

            if (lastDirection != direction)
            {
                if (direction < 0)
                {
                    wayPointsIndex = (wayPointsIndex + wayPoints.Length - 1) % wayPoints.Length;
                }

                if (direction > 0)
                {
                    wayPointsIndex = (wayPointsIndex + 1) % wayPoints.Length;
                }
            }
        }
        if (direction > 0 || direction < 0)
        {
            lastDirection = direction;
        }

    }

    bool inTarget()
    {
        if (Mathf.Abs(wayPoints[wayPointsIndex].position.x - transform.position.x) < 1.0f &&
            Mathf.Abs(wayPoints[wayPointsIndex].position.y - transform.position.y) < 1.0f &&
            Mathf.Abs(wayPoints[wayPointsIndex].position.z - transform.position.z) < 1.0f)
        {
            return true;
        }
        else return false;
    }

    void updateSpeed()
    {
        float forward = Input.GetAxisRaw("Vertical1");

        float targetSpeed = maxSpeed * forward;
        float offsetSpeed = targetSpeed - speed;

        offsetSpeed = Mathf.Clamp(offsetSpeed, -accel * dt, accel * dt);

        speed += offsetSpeed;

        if (speed > 0)
        {
            direction = 1;
        }
        else if (speed < 0)
        {
            direction = -1;
        }
        else direction = 0;
        

        angleSpeed = speed * angularMult;

        Vector3 targetPos = wayPoints[wayPointsIndex].position;

        Vector3 newPointDir = targetPos - transform.position;
        float angle = Vector3.Angle(transform.forward * direction , newPointDir);

        if (angle != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(newPointDir), angleSpeed * dt);
        }
        transform.position += transform.forward * speed * dt;
    }
}
