using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabObjects : MonoBehaviour {

    public Inputs inputs;
    public LookAndInteract lookAndInteract;

    public bool grabbed = false;
    public bool hasBeenGrabbed = false;
    

    public Collider other;
    
    private Rigidbody otherRb;

	// Use this for initialization
	void Start () {
        
        lookAndInteract = LookAndInteract.lookAndInteract;
        inputs = Inputs.inputsScript;
    }
	
	// Update is called once per frame
	void Update () {

        if (lookAndInteract.rayHit && inputs.actionRight && !grabbed)
        {
            if (lookAndInteract.hit.collider.GetComponent<Rigidbody>())
            {
                grabbed = true;
                hasBeenGrabbed = false;
                if (other == null)
                {
                    other = lookAndInteract.hit.collider;
                }
            }
        }

        if (grabbed)
        {
            other.transform.parent = transform;
            other.GetComponent<Rigidbody>().isKinematic = true;
            other.gameObject.layer = 11;
            stayFacingUpwards();
            lerpTowardsCenter();
           
            if (inputs.actionRight && hasBeenGrabbed)
            {
                forceRelease();
            }
            
            hasBeenGrabbed = true;
        }
    }

    public void forceRelease()
    {
        grabbed = false;
        other.GetComponent<Rigidbody>().isKinematic = false;
        other.gameObject.layer = 0;
        other.transform.parent = null;
        other = null;
        hasBeenGrabbed = false;
    }

    void stayFacingUpwards()//Works but Bad
    {
        if(Vector3.Angle(other.transform.up, Vector3.up) > 0)
        {
            other.transform.up = Vector3.MoveTowards(other.transform.up, Vector3.up, 5 * Time.deltaTime);
        }
    }

    void lerpTowardsCenter()
    {
        Vector3 target = transform.position;
        Vector3 difference = other.transform.position - transform.position;
        if(Vector3.SqrMagnitude(difference)>0)
        {
            other.transform.position = Vector3.MoveTowards(other.transform.position, target, 15 * Time.deltaTime);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, Vector3.one * 0.3f);
    }


}
