using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndInteract : MonoBehaviour {
    //A
    private Inputs inputs;
    private Camera cam;
    public static LookAndInteract lookAndInteract;
    public RaycastHit hit;

    [SerializeField]new private LayerMask name;
    public float maxDistance = 2.3f;
    private float distance;

    public bool rayHit = false;

    private void Awake()
    {
        lookAndInteract = this;
    }

    void Start()
    {
        
        inputs = Inputs.inputsScript;
        cam = Camera.main;
        distance = maxDistance;
    }

    void Update()
    {
        
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        
        if(Physics.Raycast(ray, out hit, maxDistance, name))
        {

            if (hit.collider != null)
            {
                distance = Vector3.Magnitude(hit.point - cam.transform.position);
            }
            

            distance = Mathf.Clamp(distance, Mathf.Epsilon, maxDistance);

            if (hit.collider.tag == ("RayInteract"))
            {
                rayHit = true;
                Debug.DrawRay(ray.origin, ray.direction.normalized * distance, Color.yellow);
                if (inputs.actionRight)
                {
                    if (hit.transform.GetComponent<LeverMovement>())
                    {
                        LeverMovement leverMovement = hit.transform.GetComponent<LeverMovement>();
                        if (leverMovement.active) //DEACTIVATE
                        {
                            leverMovement.timeEventSetInactive();
                        }
                        else //ACTIVATE
                        { 
                            leverMovement.timeEventSetActive();
                        }
                    }
                }
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction.normalized * distance, Color.red);
                rayHit = false;
            }
            
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction.normalized * distance, Color.red);
            distance = maxDistance;
            rayHit = false;
        }


        
    }
}

