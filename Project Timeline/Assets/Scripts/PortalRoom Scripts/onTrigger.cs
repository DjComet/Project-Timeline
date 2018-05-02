using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onTrigger : MonoBehaviour {

    public GameObject portal = null;
    public Vector3 pos = Vector3.zero;
    public Vector3 rot = Vector3.zero;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        GameObject portalA = GameObject.Instantiate(portal, pos, Quaternion.Euler(rot));
        this.gameObject.active = false;
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
