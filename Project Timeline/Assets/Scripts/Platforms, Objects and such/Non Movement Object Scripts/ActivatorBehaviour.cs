using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatorBehaviour : MonoBehaviour {

    Transform cube;
    Light pLight;
    public GameObject connectedTo;
    LightPulse cubeLightScript;
    public bool active = false;
    

    public float lerpSpeed = 2.0f;
    bool doLerp = false;

    Vector3 initPos;
    Quaternion initRot;
    Vector3 targetPos;
    Quaternion targetRot;

    float t = 0;


    CubeTimescaleControl cubeTimeControl;
    // Use this for initialization
    void Start()
    {
        if (connectedTo == null) Debug.Log("In " + gameObject.name + "connectedTo is null");
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name == "SphereLight")
            {
                pLight = child.GetComponent<Light>();
                targetPos = child.transform.position;
                targetRot = child.transform.rotation;
            }
        }
    }
	// Update is called once per frame
	void Update () {
        if(cube!= null)
            cubeTimeControl = cube.GetComponent<CubeTimescaleControl>();

        if (cubeLightScript != null)
        {
            if (active)
            {

                cubeLightScript.minIntensity = cubeLightScript.initMinI + 6;//Make cube's light brighter
                cubeLightScript.maxIntensity = cubeLightScript.initMaxI + 6;

            }
            else
            {
                cubeLightScript.minIntensity = cubeLightScript.initMinI;//Revert to normal light intensity
                cubeLightScript.maxIntensity = cubeLightScript.initMaxI;
            }
        }

        if(connectedTo!=null)
        active = connectedTo.GetComponent<Linker>().active;

        if (doLerp)
        {
            t += Time.deltaTime * lerpSpeed;
            cube.GetComponent<Rigidbody>().isKinematic = true;
            cube.GetComponent<Rigidbody>().useGravity = false;
            cube.GetComponent<RewindScript>().enabled = false;
            cube.GetComponent<PositiveTimeScript>().enabled = false;

            cube.position = Vector3.Lerp(initPos, targetPos, t);
            cube.rotation = Quaternion.Slerp(initRot, targetRot, t);
            
            
            t = Mathf.Clamp01(t);
            if (t >= 1)
            {
                doLerp = false;
                t = 0;
                //cube.GetComponent<Rigidbody>().isKinematic = false;
            }
        }

        if(cubeTimeControl!= null)
            activateCube();

        
    }
    private void LateUpdate()
    {
        
    }

    void activateCube()
    {
        
        
        if(active)
        {
            cubeTimeControl.activator = this;
            cubeTimeControl.applyTime();
        }
        else
        {    
            cubeTimeControl.normalizeTime();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "RayInteract" && other.GetComponent<CubeTimescaleControl>())
        {
            cube = other.transform;
            //cube.parent = transform;
            cubeLightScript = cube.GetChild(1).GetChild(0).GetComponent<LightPulse>();

            initPos = cube.position;
            initRot = cube.rotation;
            pLight.enabled = false;
            
            doLerp = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "RayInteract" && other.GetComponent<CubeTimescaleControl>())
        {
            if(active)
            {
                cubeLightScript.minIntensity -= 4;//Revert to normal light intensity
                cubeLightScript.maxIntensity -= 4;
            }

            cube.GetComponent<Rigidbody>().useGravity = true;
            cube.GetComponent<RewindScript>().enabled = true;
            cube.GetComponent<PositiveTimeScript>().enabled = true;
            doLerp = false;
            
            pLight.enabled = true;
            //cube.parent = null;
            cubeTimeControl.activator = this;
            cubeTimeControl = null;
            cubeLightScript = null;
            cube = null;
        }
    }
}
