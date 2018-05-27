using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrower : MonoBehaviour{
    //J
    public float throwForce = 20.0f;
    public GameObject ballPrefab;
    public float delta = 0.04f;
    public int weaponActive;
    private GameObject player;

    private MainPlayerController playerController;
    public Transform trayectoria;
    Vector3 spawnPos;
    Vector3 initPosTrayectory;

    bool debugged = false;
    // Use this for initialization
    void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerController>();
        player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < player.transform.childCount; i++)
        {
            Transform child = player.transform.GetChild(i).transform;
            if (child.name == "Trayectory")
            {
                trayectoria = child;
            } 

        }

    }

    // Update is called once per frame
    void Update()
    {
        weaponActive = playerController.weaponSelector;
        spawnPos = 0.5f * Vector3.down + transform.position + transform.forward;

        if(trayectoria!=null)
        {
            if (Input.GetMouseButton(2) && weaponActive == 2)
            {
                trayectoria.transform.gameObject.SetActive(true);
                ShowTrajectory();
            }
            else
            {
                trayectoria.transform.gameObject.SetActive(false);
            }

            if (Input.GetMouseButtonUp(2) && weaponActive == 2)
            {
                ThrowGrenade();
            }

            initPosTrayectory = transform.position;
        }
        else
        {
            if (!debugged)
            {
                Debug.Log("La variable 'trayectoria' no ha sido asignada");
                debugged = true;
            }
        }

        
    }

    void ThrowGrenade()
    {
        GameObject grenade = Instantiate(ballPrefab, spawnPos, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(3 * Vector3.up + transform.forward * throwForce, ForceMode.VelocityChange);

    }

    void ShowTrajectory()
    {
        for (int i = 0; i < trayectoria.childCount; i++)
        {
            float t = delta * i;
            trayectoria.GetChild(i).position = spawnPos + (3 * Vector3.up + transform.forward * throwForce) * t + 0.5f * Physics.gravity * t * t;
        }
    }

}
