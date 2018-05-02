using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepThroughPortal : MonoBehaviour
{
    //A
    public Transform otherPortal;
    private Vector3 portalPosition;
    private Quaternion portalRotation;
    public float angleOfPlayer;
    public Vector3 displacement;
    public Vector3 normalizedDisplacement;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "CanGoThroughPortals" || other.tag == "Main Camera")
        {

            angleOfPlayer = Vector3.Angle(gameObject.transform.forward, other.transform.forward);
            //other.transform.position = otherPortal.transform.position + otherPortal.transform.forward * 1.0f;
            //newTransformForward = Quaternion.AngleAxis(angleOfPlayer, otherPortal.transform.forward) * otherPortal.transform.right+otherPortal.transform.up; 

            portalPosition = otherPortal.TransformPoint(transform.InverseTransformPoint(other.transform.position));
            Vector3 mirroredPos = ReflectionOverPlane(portalPosition, new Plane(otherPortal.right, otherPortal.position));

            portalPosition = mirroredPos;





            portalRotation = Quaternion.LookRotation(
                   otherPortal.TransformDirection(transform.InverseTransformDirection(other.transform.forward)),
                   otherPortal.TransformDirection(transform.InverseTransformDirection(other.transform.up)));
            portalRotation = Quaternion.AngleAxis(180.0f, new Vector3(0, 1, 0)) * portalRotation;

            other.transform.position = portalPosition;
            other.transform.rotation = portalRotation;
        }
    }
    public Vector3 ReflectionOverPlane(Vector3 point, Plane plane)
    {
        //Vector3 N = transform.TransformDirection(plane.normal);
        //return point - 2 * N * Vector3.Dot(point, N) / Vector3.Dot(N, N);
        displacement = plane.ClosestPointOnPlane(point) - point;
        normalizedDisplacement = displacement.normalized;
        return point += displacement.magnitude * 2 * normalizedDisplacement;
    }
}

