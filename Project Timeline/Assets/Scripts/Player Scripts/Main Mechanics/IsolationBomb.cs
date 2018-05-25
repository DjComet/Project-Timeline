using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsolationBomb : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if the objects is possible control it
        if(collision.gameObject.layer == 9)
        {
            //change var to change color
            Debug.Log("Detected");
        }
        //Destroy ball
        Destroy(gameObject);

        //Add effect of water ballon or something like this


    }
}
