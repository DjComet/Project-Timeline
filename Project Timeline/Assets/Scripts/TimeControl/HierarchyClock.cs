using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HierarchyClock : Clock {

    private GameObject parentGO;
    public List<Transform> childObjects;

    public float totalChildCount;
    private float lastChildCount;

    // Use this for initialization
    void Start()
    {
        parentGO = transform.parent.gameObject;
        timeValues = new float[] { -1f, 0f, 0.2f, 1f, 4f };

        parentGO.GetComponentsInChildren(childObjects);

        

        totalChildCount = childObjects.Count;
        cycleThroughObjects();
    }

    // Update is called once per frame
    void Update() {
        dt = Time.deltaTime;

        

        
        parentGO.GetComponentsInChildren(childObjects);

        clockCore();

        if (totalChildCount != childObjects.Count)
        {
            if (enableDebug) Debug.Log("Cycling through objects");
            cycleThroughObjects();
            totalChildCount = childObjects.Count;
        }

    }

    void cycleThroughObjects()
    {
         foreach(Transform transform in childObjects)
        {
            GameObject child = transform.gameObject;
                if (child.GetComponent<TimeLine>() != null)
                {
                    child.GetComponent<TimeLine>().clock = this;
                }
            
        }

    }
}
