using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAndInteract : MonoBehaviour {
    //A
    private Inputs inputs;
    private Camera cam;

    [SerializeField] private LayerMask name;
    public float maxDistance = 2.3f;
    private float distance;

    public bool rayHit = false;

    void Start()
    {
        inputs = GetComponent<Inputs>();
        cam = Camera.main;
        distance = maxDistance;
    }

    void Update()
    {
        RaycastHit hit;
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
                if (inputs.actionRight)
                {
                    if (hit.transform.GetComponent<Linker>())
                    {
                        bool active = hit.transform.GetComponent<Linker>().active;
                        active = !active;
                        hit.transform.GetComponent<Linker>().active = active;

                    }
                }
            }
            
        }
        else
        {
            distance = maxDistance;
            rayHit = false;
        }


        Debug.DrawRay(ray.origin, ray.direction.normalized * distance, Color.yellow);
    }
}

