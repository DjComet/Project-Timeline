using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceChangeTime : MonoBehaviour {

    private GameObject cameraPlayer;
    private GameObject armsHolder;
    public GameObject pauseText;
    public GameObject slowtext;
    public GameObject rewindtext;
    public GameObject accelerateText;

    private Inputs inputs;
    private GameObject player;
    private PlayerTimeScaleControl timeScaleControl;
    private MainClock mainClock;


    private Color normalColorText = new Color(0.0f, 1.0f, 0.75f, 1.0f);
    private Color activatedColorText = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    // Use this for initialization
    void Start () {
        mainClock = MainClock.mainClock;
        
        player = GameObject.FindGameObjectWithTag("Player");
        inputs = player.GetComponent<Inputs>();
        cameraPlayer = GameObject.FindGameObjectWithTag("MainCamera");

        for (int i = 0; i < cameraPlayer.transform.childCount; i++)
        {
            GameObject child = cameraPlayer.transform.GetChild(i).gameObject;
            if (child.name == "ArmsPlaceholder")
            {
                armsHolder = child;
            }
        }

        for (int i = 0; i < armsHolder.transform.childCount; i++)
        {
            GameObject child = armsHolder.transform.GetChild(i).gameObject;
            if (child.name == "Pause3dText")
            {
                pauseText = child;
            }
            else if (child.name == "Slow3dText")
            {
                slowtext = child;
            }
            else if (child.name == "Rewind3dText")
            {
                rewindtext = child;
            }
            else if (child.name == "Accelerate3dText")
            {
                accelerateText = child;
            }
        }


    }
	
	// Update is called once per frame
	void Update () {

        if (inputs.leftClick)
        {
            pauseText.SetActive(true);
            slowtext.SetActive(true);
        }
        else if (inputs.rightClick)
        {
            rewindtext.SetActive(true);
            accelerateText.SetActive(true);
        }
        else
        {
            pauseText.SetActive(false);
            slowtext.SetActive(false);
            rewindtext.SetActive(false);
            accelerateText.SetActive(false);
        }
        if(mainClock.pauseActivated)
        {
            pauseText.GetComponent<TextMesh>().color = activatedColorText; 
        }
        else
        {
            pauseText.GetComponent<TextMesh>().color = normalColorText;
        }
        if (mainClock.slowActivated)
        {
            slowtext.GetComponent<TextMesh>().color = activatedColorText;
        }
        else
        {
            slowtext.GetComponent<TextMesh>().color = normalColorText;
        }
        if (mainClock.rewindActivated)
        {
            rewindtext.GetComponent<TextMesh>().color = activatedColorText;
        }
        else
        {
            rewindtext.GetComponent<TextMesh>().color = normalColorText;
        }
        if (mainClock.accelActivated)
        {
            accelerateText.GetComponent<TextMesh>().color = activatedColorText;
        }
        else
        {
            accelerateText.GetComponent<TextMesh>().color = normalColorText;
        }


    }
}
