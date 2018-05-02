using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftPlatformMovement : MonoBehaviour {
    //A
    public Transform origin;

    public float maxSpeed = 0.1f;
    public float acceleration = 50.0f;
    public Vector3 speed = Vector3.zero;
    private Vector3 startingPosition;
    public Vector3 currentPlayerPos = Vector3.zero;//esto es para debuguear
    int input = 0;
    GameObject player;


    public bool move = false;
    public bool reverse = false;

    public float timer = 0.0f;

    bool hasReachedTop = false;
    bool hasReachedBottom = false;

    // Use this for initialization
    void Start () {
        startingPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;

        
        

        if (reverse)
        {
            input = -1;
        }
        

        float target = maxSpeed * input;
        float offsetSpeed = target - speed.y;
        offsetSpeed = Mathf.Clamp(offsetSpeed, -acceleration * dt, acceleration * dt);

        speed.y += offsetSpeed;
        speed.y = Mathf.Clamp(speed.y, -maxSpeed, maxSpeed);

        transform.Translate (Vector3.up * speed.y);

        if(transform.position.y >= origin.position.y)
        {
            input = 0;
            speed = Vector3.zero;
            transform.position = origin.position;
            hasReachedTop = true;
        }

        if (transform.position.y <= startingPosition.y)
        {
            reverse = false;
            input = 0;
            speed = Vector3.zero;
            transform.position = startingPosition;
            hasReachedBottom = true;
        }

        if (player != null)
        {
            player.GetComponent<CharacterController>().Move(Vector3.up * speed.y);
            currentPlayerPos = player.transform.position;

        }

        if(hasReachedTop)
        {
            timer += dt;

            if(timer >= 1)
            {
                timer = 0;
                reverse = true;
                hasReachedTop = false;
                
            }
        }

        if (hasReachedBottom && move)
        {
            timer += dt;

            if (timer >= 1)
            {
                timer = 0;
                input = 1;
                hasReachedBottom = false;
                
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            move = true;
            
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = null;
            move = false;
        }
    }
}
