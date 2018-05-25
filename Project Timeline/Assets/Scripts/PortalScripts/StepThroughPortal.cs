using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepThroughPortal : MonoBehaviour
{
    //A
    public Transform otherPortal;
    public float angleOfPlayer;
    public Vector3 displacement;
    public Vector3 normalizedDisplacement;

    public static float minTeleportThreshold = 0.08f;
    public static float teleportDistance = 0.09f;//must always be greater than minTeleportThreshold or else teleported object will keep teleporting between portals until its escape velocity per frame is greater than the min threshold.

    public List<Collider> objectsInPortal = new List<Collider>();

    private Vector3 portalPosition;
    private Quaternion portalRotation;

    Vector3 mirroredPos;
    Vector3 targetPos;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        for (int i = objectsInPortal.Count - 1; i >= 0; i--)
        {
            if (objectsInPortal[i].tag == "Player")
            {
                targetPos = otherPortal.TransformPoint(transform.InverseTransformPoint(objectsInPortal[i].transform.position));
                //Vector3 nextPos = objectsInPortal[i].GetComponent<Locomotion>().nextPos;
                Debug.Log("Distance to portal:" + Vector3.Magnitude(new Plane(transform.forward, transform.position).ClosestPointOnPlane(objectsInPortal[i].transform.position) - objectsInPortal[i].transform.position));
                if (Vector3.Magnitude(new Plane(transform.forward, transform.position).ClosestPointOnPlane(objectsInPortal[i].transform.position) - objectsInPortal[i].transform.position) <= minTeleportThreshold)
                {
                    teleport(objectsInPortal[i]);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetPos, 0.5f);
        Gizmos.DrawWireSphere(mirroredPos, 0.1f);
    }

    private void OnTriggerEnter(Collider other)
    {
        objectsInPortal.Add(other);    
    }
    private void OnTriggerExit(Collider other)
    {
        for (int i = objectsInPortal.Count - 1; i >= 0; i--)
        {
            if (objectsInPortal[i] == other)
            {
                objectsInPortal.RemoveAt(i);
            }
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

    public void teleport(Collider collider)
    {
        //Hay que reemplazar la variable otherPortal por un punto perpendicular a ese punto de Otherportal a X distancia
        portalPosition = otherPortal.TransformPoint(transform.InverseTransformPoint(collider.transform.position));
        
        mirroredPos = ReflectionOverPlane(portalPosition, new Plane(otherPortal.right, otherPortal.position));//---|---
                                                                                                              //-m-|-p-
        portalPosition = mirroredPos;

        portalRotation = Quaternion.LookRotation(
               otherPortal.TransformDirection(transform.InverseTransformDirection(Camera.main.transform.forward)),
               otherPortal.TransformDirection(transform.InverseTransformDirection(Camera.main.transform.up)));
        portalRotation = Quaternion.AngleAxis(180.0f, new Vector3(0, 1, 0)) * portalRotation;

        Camera.main.transform.rotation = portalRotation;

        collider.transform.position = portalPosition;
    }
}

