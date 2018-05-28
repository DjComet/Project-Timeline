using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatedPhysicsDrop : MonoBehaviour {

    TimeLine timeLine;
    Rigidbody rb;
    public GameObject connectedTo;

    private Linker linker;
    bool doneActivated = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();

        if (connectedTo!= null)
        linker = connectedTo.GetComponent<Linker>();
        rb.isKinematic = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (connectedTo != null)
        {
            if (!linker.active && !doneActivated)
            {
                rb.isKinematic = true;
            }

            if (linker.active)
            {
                rb.isKinematic = false;
                doneActivated = true;
            }
        }
	}
}
