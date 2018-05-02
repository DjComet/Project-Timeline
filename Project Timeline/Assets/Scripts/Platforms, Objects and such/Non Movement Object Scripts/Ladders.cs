using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladders : MonoBehaviour {
    //A
    GameObject player;
    public bool enter = false;
    public bool exit = false;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<MainPlayerController>().isOnLadder = true;

            enter = true;
            exit = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.GetComponent<MainPlayerController>().isOnLadder = false;
            player = null;

            exit = true;
            enter = false;
        }
    }
}
