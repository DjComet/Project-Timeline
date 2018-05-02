using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRoom : MonoBehaviour {

    public Vector3 pos = Vector3.zero;
    public Vector3 rot = Vector3.zero;

    private GameObject roomCopy = null;


	// Use this for initialization
	void Start () {
        if(this.tag == "OriginalRoom")
        {
            this.tag = "CopyRoom";
            roomCopy = GameObject.Instantiate(this.gameObject, transform.position + pos, Quaternion.Euler(rot));
            roomCopy.active = false;

            this.tag = "OriginalRoom";
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        if (roomCopy) {
            if(!roomCopy.active && GameObject.FindGameObjectWithTag("PortalB")) 
            {
                roomCopy.active = true;
            }
        }
	}
}
