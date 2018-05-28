using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceChangeWeapon : MonoBehaviour
{
    private Inputs inputs;
    private MainPlayerController playerController;
    public GameObject weaponSelectedIcon;
    public GameObject[] weaponIcons = new GameObject[4];
    private GameObject playerCanvas;
    public GameObject weaponPanel;
    public GameObject energyPanel;
    private GameObject[] weaponPanelOptions = new GameObject [8];
    public Vector3 SelectorPos = new Vector3(0,0,0);
    public GameObject weaponSelector;
    public float counter;    
    public int weaponActive;
    public int weaponSelected;
    public bool inTranssition;
    private float t;
    //private float tColor;
    //private bool transitionColorFinish;
    //public Transform[] transfoms;


    private Color imageBeforeColor;
    private Color imageNextColor;

    //public Transform startPositionLerp;
    //public Transform endPositionLerp;

    float distance;
    float distanceToDestination;


    // Use this for initialization
    void Start()
    { 
        
        inputs = GameObject.FindGameObjectWithTag("Player").GetComponent<Inputs>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<MainPlayerController>();
        //weaponPanel = GameObject.Find("WeaponPanel");
       
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");        

        for (int i = 0; i < playerCanvas.transform.childCount; i++)
        {
            GameObject child = playerCanvas.transform.GetChild(i).gameObject;
            if (child.name == "WeaponPanel")
            {
                weaponPanel = child;
            }
            else if(child.name == "EnergyPanel")
            {
                energyPanel = child;
            }
        }

        for (int i = 0; i < energyPanel.transform.childCount; i++)
        {
            //Debug.Log(i);
            GameObject child = energyPanel.transform.GetChild(i).gameObject;
            if (child.name == "WeaponSelectedIcon")
            {
                weaponSelectedIcon = child;
            }
        }

        for (int i = 0; i < weaponSelectedIcon.transform.childCount; i++)
        {
            //Debug.Log(i);
            GameObject child = weaponSelectedIcon.transform.GetChild(i).gameObject;
            if (child.name == "Weapon01")
            {
                weaponIcons[i] = child;
            }
            else if (child.name == "Weapon02")
            {
                weaponIcons[i] = child;
            }
            else if (child.name == "Weapon03")
            {
                weaponIcons[i] = child;
            }
            else if (child.name == "Weapon04")
            {
                weaponIcons[i] = child;
            }
        }

        for (int i = 0; i < weaponPanel.transform.childCount; i++)
        {
            //Debug.Log(i);
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

        distance = Vector3.Distance(weaponPanelOptions[0].transform.position, weaponPanelOptions[1].transform.position);
        counter = 0;
        weaponActive = 0;
        weaponSelector.transform.position = weaponPanelOptions[weaponActive].transform.position;     

    }

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenu.GameIsPaused)
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
            changeWeaponIcon();
        }
      
        
    }

    void checkInputs()
    {
        
        if(inputs.mouseScroll != 0 || inputs.weap1 || inputs.weap2 || inputs.weap3 || inputs.weap4)
        {
            counter = 90.0f;
            t = 0;             
            inTranssition = true;                                
        }
    }

    
    void checkSelectorPos()
    {
        if (inTranssition)
        {
            Debug.Log(weaponActive);
            changePosSelector(weaponSelector.transform, weaponPanelOptions[weaponSelected].transform);
            //changeColorPanelOption();
            /*if (transitionFinish)
            {
                Debug.Log("Finish Transition");
                weaponActive = playerController.weaponSelector;
                t = 0;
            }*/
        }
    }

    void changePosSelector(Transform startPositionLerp,Transform endPositionLerp)
    {
        
        distanceToDestination = Vector3.Distance(weaponPanelOptions[weaponActive].transform.position, weaponPanelOptions[playerController.weaponSelector].transform.position);
            
        t += Time.deltaTime / (2.0f /*(distanceToDestination/distance)*/);
        if(t <= 1)
        {
            
            weaponSelector.transform.position = Vector3.Lerp(startPositionLerp.position, endPositionLerp.position, t);
        }
        else
        {
            inTranssition = false;
            weaponActive = playerController.weaponSelector;
            t = 0;
        }
    }
    
    void changeWeaponIcon()
    {
        for (int i = 0; i < weaponIcons.Length; i++)
        {
            if(i == playerController.weaponSelector)
            {
                weaponIcons[i].SetActive(true);
            }
            else
            {
                weaponIcons[i].SetActive(false);
            }
        }
    }
    /*
    void changeColorPanelOption()
    {
        Debug.Log("colorCambiando");
        imageBeforeColor = weaponPanelOptions[weaponActive].GetComponent<Image>().color;
        imageNextColor = weaponPanelOptions[weaponSelected].GetComponent<Image>().color;

        imageBeforeColor.a = Mathf.Lerp(imageBeforeColor.a, 0, t);
        imageNextColor.a = Mathf.Lerp(imageNextColor.a, 200, t);

        weaponPanelOptions[weaponActive].GetComponent<Image>().color = imageBeforeColor;
        weaponPanelOptions[weaponSelected].GetComponent<Image>().color = imageNextColor;
    }
    */
}
