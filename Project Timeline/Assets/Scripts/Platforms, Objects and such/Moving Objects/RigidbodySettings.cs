using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodySettings : MonoBehaviour {

    protected Rigidbody rb;
    public bool inhibited = false;

    private void Awake()
    {
       
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void makeKinematic()
    {
        if(!inhibited)
            rb.isKinematic = true;
    }

    public void makeNotKinematic()
    {
        if (!inhibited)
            rb.isKinematic = false;
    }

    public void makeIngrav()
    {
        if (!inhibited)
            rb.useGravity = false;
    }

    public void makeGrav()
    {
        if (!inhibited)
            rb.useGravity = true;
    }
}
