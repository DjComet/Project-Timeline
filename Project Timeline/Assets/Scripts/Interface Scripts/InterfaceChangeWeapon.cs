using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceChangeWeapon : MonoBehaviour
{
    private Inputs inputs;
    private MainPlayerController playerController;
    public GameObject weaponPanel;
    private GameObject[] weaponPanelOptions = new GameObject [8];
    public Vector3 SelectorPos = new Vector3(0,0,0);
    public GameObject weaponSelector;
    public float counter;    
    public int weaponActive;
    public int weaponSelected;
    private bool transitionFinish;
    private float t;


    //public Transform startPositionLerp;
    //public Transform endPositionLerp;
    float starTime;
    float distanceToDestination;


    // Use this for initialization
    void Start()
    {        
        inputs = GameObject.FindGameObjectWithTag("Player").GetComponent<Inputs>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerController>();
        weaponPanel = GameObject.Find("WeaponPanel");

        for (int i = 0; i < weaponPanel.transform.childCount; i++)
        {
            Debug.Log(i);
            GameObject child = weaponPanel.transform.GetChild(i).gameObject;
            if (child.name == "WeaponIcon01")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponIcon02")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponIcon03")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponIcon04")
            {
                weaponPanelOptions[i] = child;
            }
            else if (child.name == "WeaponSelector")
            {
                weaponSelector = child;
            }

        }
        counter = 0;
        weaponActive = 0;
        SelectorPos = weaponPanelOptions[weaponActive].transform.position;
        transitionFinish = true;     
        starTime = Time.time;   

    }

    // Update is called once per frame
    void Update()
    {
        weaponSelected = playerController.weaponSelector;
        if (counter != 0)
        {
            weaponPanel.SetActive(true);
            counter--;
        }
        else
        {
            weaponPanel.SetActive(false);
        }

        checkInputs();
        checkSelectorPos();       
        
    }

    void checkInputs()
    {
        
        if(inputs.mouseScroll != 0 || inputs.weap1 || inputs.weap2 || inputs.weap3 || inputs.weap4)
        {
            counter = 2000.0f;
        }
    }

    void checkSelectorPos()
    {        
        if(weaponSelected != weaponActive)
        {
            Debug.Log("Start Transition");
            changePosSelector(weaponSelector.transform, weaponPanelOptions[weaponSelected].transform);
            if(transitionFinish)
            {
                weaponActive = playerController.weaponSelector;
            }
        }
        else
        {
            Debug.Log("Finish Transition");
            starTime = Time.time;
        }
    }

    /*
    void checkSelectorPos()
    {
        int weaponSelected = playerController.weaponSelector;

        if(weaponActive != weaponSelected)
        {
            Debug.Log("asasd");
            changePosSelector(weaponSelector.transform,weaponPanelOptions[weaponSelected].transform);
        }
        else
        {
            starTime = Time.time;
            weaponActive = playerController.weaponSelector;
        }

    }*/

    void changePosSelector(Transform startPositionLerp,Transform endPositionLerp)
    {
        float currentDuration = Time.time - starTime;
        distanceToDestination = Vector3.Distance(weaponPanelOptions[weaponActive].transform.position, weaponPanelOptions[playerController.weaponSelector].transform.position);
        float journeyFraction = currentDuration / distanceToDestination;
        if(journeyFraction <= 1)
        {
            transitionFinish = false;
            weaponSelector.transform.position = Vector3.Lerp(startPositionLerp.position, endPositionLerp.position, journeyFraction);
        }
        else
        {
            transitionFinish = true;
        }
    }

}
