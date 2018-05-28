using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRoomControl : MonoBehaviour {

    public GameObject player;
    public HierarchyClock roomClock;
    CanvasUpdater cU;

    bool clockSet = false;

	// Use this for initialization
	void Start () {
        roomClock = transform.GetChild(0).GetComponent<HierarchyClock>();
        cU = transform.GetChild(0).GetComponent<CanvasUpdater>();
    }
	
	// Update is called once per frame
	void Update () {
		if(player && !clockSet)
        {
            cU.enabled = true;
            player.GetComponent<PlayerTimeScaleControl>().clock = roomClock;
            clockSet = true;
        }
        else if(!player)
        {
            cU.enabled = false;
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
