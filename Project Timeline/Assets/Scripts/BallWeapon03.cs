using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallWeapon03 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //---------->  Jesús copia este código y pégalo en el script "IsolationBomb" en la carpeta main mechanics, please. <-----------------------
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if the objects is possible control it

        //Destroy ball
        Destroy(gameObject);

        //Add effect of water ballon or something like this

        
    }
}
