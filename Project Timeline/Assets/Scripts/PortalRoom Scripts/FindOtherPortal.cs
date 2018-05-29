using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindOtherPortal : MonoBehaviour {

    public GameObject PortalPref;
    public Vector3 posDiferenceCopy = Vector3.zero;

    private GameObject otherP;
    private GameObject pref;
    private GameObject roomChange = null;

    // Use this for initialization
    void Start () {
        if(this.tag == "PortalA")
        {
            if(GameObject.FindGameObjectWithTag("OriginalRoom")!= null)
            {
                posDiferenceCopy = GameObject.FindGameObjectWithTag("OriginalRoom").GetComponent<CopyRoom>().pos;
                roomChange = GameObject.Instantiate(GameObject.FindGameObjectWithTag("OriginalRoomChange"),
                    posDiferenceCopy + GameObject.FindGameObjectWithTag("OriginalRoom").GetComponent<CopyRoom>().pos,
                    Quaternion.Euler(GameObject.FindGameObjectWithTag("OriginalRoom").GetComponent<CopyRoom>().rot));
                roomChange.tag = "CopyRoomChange";
            }
            
        }

	}
	
	// Update is called once per frame
	void Update () {
        if (GameObject.FindGameObjectWithTag("PortalB") && !this.GetComponent<StepThroughPortal>().otherPortal)
        {
            if (this.tag == "PortalB")
            {
                otherP = GameObject.FindGameObjectWithTag("PortalA");
            }
            if (this.tag == "PortalA")
            {
                pref = GameObject.Instantiate(PortalPref, transform.position, transform.rotation);
                transform.position += GameObject.FindGameObjectWithTag("CopyRoom").transform.position - 
                    GameObject.FindGameObjectWithTag("OriginalRoom").transform.position;
                otherP = GameObject.FindGameObjectWithTag("PortalB");
            }
            this.GetComponent<StepThroughPortal>().otherPortal = otherP.transform;
            this.GetComponent<PortalView>().pointB = otherP.transform;
        }
    }

    private void OnDestroy()
    {
        if (this.tag == "PortalA"){
            Destroy(roomChange);
            Destroy(pref);
        }
    }
}
