using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalCreator : MonoBehaviour {

    public GameObject PortalPref;
    public GameObject portalPrefA;
    public GameObject portalPrefB;

    private Transform cam;
    private GameObject portal;
    private GameObject portalA;
    private GameObject portalB;

    private GameObject portalWall;

    // Use this for initialization
    void Start () {
        cam = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray myRay;

        if (Input.GetMouseButton(0))
        {
            myRay = new Ray(cam.position, cam.forward);

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

                        portalWall = hit.collider.gameObject;
                    }

                    if (portalB)
                    {
                        if(hit.collider.transform.parent.tag == "CopyRoom"){
                            hit.collider.transform.parent.tag = "OriginalRoom";
                            GameObject.FindGameObjectWithTag("OriginalRoom").tag = "CopyRoom";
                            Destroy(GameObject.FindGameObjectWithTag("OriginalRoomChange"));
                            GameObject.FindGameObjectWithTag("CopyRoomChange").tag = "OriginalRoomChange";
                        }
                        Destroy(portalA);
                        Destroy(portalB);
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
                portalA.layer = 10;
                portalB.layer = 10;

                portalWall.layer = 12;

                GameObject.Destroy(portal);
            }
        }


		
	}
}
