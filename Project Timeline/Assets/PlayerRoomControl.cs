using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomControl : MonoBehaviour {

    public GameObject player;
    public HierarchyClock roomClock;

    bool clockSet = false;

	// Use this for initialization
	void Start () {
        roomClock = transform.GetChild(0).GetComponent<HierarchyClock>();
	}
	
	// Update is called once per frame
	void Update () {
		if(player && !clockSet)
        {
            player.GetComponent<PlayerTimeScaleControl>().clock = roomClock;
            clockSet = true;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            player = other.gameObject;
            clockSet = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
        }
    }
}
