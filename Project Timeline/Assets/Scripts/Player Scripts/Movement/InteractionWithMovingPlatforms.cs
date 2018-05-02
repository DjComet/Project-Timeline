using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionWithMovingPlatforms : MonoBehaviour
{
    //A  [DEPRECATED]
    Locomotion locomotion;
    GameObject otherPlatform;

    public Vector3 currentPlatformSpeed = Vector3.zero;
    public bool hasTouchedIt = false;


    // Use this for initialization
    void Start()
    {
        locomotion = gameObject.GetComponent<Locomotion>();

    }

    // Update is called once per frame
    void Update()
    {








    }


}
