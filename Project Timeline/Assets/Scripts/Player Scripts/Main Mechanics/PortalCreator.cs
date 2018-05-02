using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCreator : MonoBehaviour {

    public GameObject PortalPref;
    public GameObject portalPrefA;
    public GameObject portalPrefB;

    private Transform camera;
    private GameObject portal;
    private GameObject portalA;
    private GameObject portalB;


    // Use this for initialization
    void Start () {
        camera = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray myRay;

        if (Input.GetMouseButton(0))
        {
            myRay = new Ray(camera.position, camera.forward);

            if (Physics.Raycast(myRay, out hit))
            {
                if (hit.collider.tag == "CanCreatePortal")
                {
                    if (!portal)
                    {
                        portal = GameObject.Instantiate(PortalPref, new Vector3(hit.point.x, hit.transform.position.y, hit.point.z), Quaternion.LookRotation(hit.normal, hit.transform.up));
                    }
                    else
                    {
                        portal.transform.position = new Vector3(hit.point.x, hit.transform.position.y, hit.point.z);
                        portal.transform.rotation = Quaternion.LookRotation(hit.normal, hit.transform.up);
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (!portalA)
            {
                portalA = GameObject.Instantiate(portalPrefA, portal.transform.position, portal.transform.rotation);
                GameObject.Destroy(portal);
            }
            else if (!portalB)
            {
                portalB = GameObject.Instantiate(portalPrefB, portal.transform.position, portal.transform.rotation);
                GameObject.Destroy(portal);
            }
        }
		
	}
}
