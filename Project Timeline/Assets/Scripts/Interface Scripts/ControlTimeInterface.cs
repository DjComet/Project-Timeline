using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class ControlTimeInterface : MonoBehaviour
{

    public float tValue;
    public float lastWheelPos;//0 = normal position, 1 = contrary position
    private Clock mainClock;
    private Inputs inputs;
    private GameObject playerCanvas;
    private GameObject timeWheel;
    private Image[] divisionsWheel;//0 =  UpLeft , 1 = UpRight, 2 =  DownLeft, 3 = DownRight
    public float divisionActive;//0 =  UpLeft , 1 = UpRight, 2 =  DownLeft, 3 = DownRight



    // Use this for initialization
    void Start()
    {
        mainClock = Clock.mainClock;
        inputs = GetComponentInParent<Inputs>();

        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
        for (int i = 0; i < playerCanvas.transform.childCount; i++)
        {
            GameObject child = playerCanvas.transform.GetChild(i).gameObject;
            if (child.name == "TimeWheel")
            {
                timeWheel = child;
            }
        }
        
        for (int i = 0; i < timeWheel.transform.childCount; i++)
        {
            GameObject child = playerCanvas.transform.GetChild(i).gameObject;
            if (child.name == "UpLeft")
            {
                divisionsWheel [0] = child.GetComponent<Image>(); ;
            }
            else if (child.name == "UpRight")
            {
                divisionsWheel[1] = child.GetComponent<Image>(); ;
            }
            else if (child.name == "DownLeft")
            {
                divisionsWheel[2] = child.GetComponent<Image>(); ;
            }
            else if (child.name == "DownRight")
            {
                divisionsWheel[3] = child.GetComponent<Image>(); ;
            }
        }

        tValue = 1;
        lastWheelPos = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        tValue = mainClock.targetValue;

        if(inputs.leftClick || inputs.rightClick)
        {
            timeWheel.SetActive(true);
        }
        else
        {
            timeWheel.SetActive(false);
        }

        checkInputs();

        checkDivisions();


    }

    void checkInputs()
    {
        if (tValue == 2 )//Accel 
        {
            divisionActive = 3;
        }
        else if (tValue == 0.2f)//Slow
        {
            divisionActive = 1;
        }
        if (tValue == -1 )//Rewind
        {
            divisionActive = 2;
        }
        else if (tValue == 0)//Pause
        {
            divisionActive = 0;
        }
    }

    void checkDivisions()
    {
        if(divisionActive == 0)
        {
            effectActive(divisionsWheel[0]);
        }
        else if (divisionActive == 1)
        {
            effectActive(divisionsWheel[1]);
        }
        else if (divisionActive == 2)
        {
            effectActive(divisionsWheel[2]);
        }
        else if (divisionActive == 3)
        {
            effectActive(divisionsWheel[3]);
        }
    }

    void effectActive(Image img)
    {
        var imageColor = img.color;
        imageColor.a = Mathf.MoveTowards(img.color.a, 0.5f, 3f * Time.deltaTime);
        img.color = imageColor;


    }
    void resetDivisons()
    {
        for(int i = 0;  i <= divisionsWheel.Length -1; i++)
        {
            var imageColor = divisionsWheel[i].color;
            imageColor.a = Mathf.MoveTowards(divisionsWheel[i].color.a, 0.0f, 3f * Time.deltaTime);
            divisionsWheel[i].color = imageColor;
        }


    }

    void activeWheelChilds()//If is neccesary
    {

    }









}
