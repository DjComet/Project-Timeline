using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

public class CameraEffectsController : MonoBehaviour {
    
    public float targetValue;
    public float previousTValue;
    GameObject player;

    public Clock clock;
    private PostProcessingProfile ppp;
    private GameObject playerCanvas;
    private Image fadingScreen;

    public float colorApplySpeed = 2f;
    private Color[] colorValues = new Color[4];//0(azul-pausa),1(rojo-rebobinar),2(amarillo-slow),3(naranja-acelerar)
    public Color color;
    private string lastColor;

    public bool hasFaded = false;
    public bool canFade = false;
    public bool notSet = true;
    public bool hasRewinded = false;

    float acceleratedTimeValue;
    float normalTimeValue;
    float slowedTimeValue;
    float pausedTimeValue;
    float rewindTimeValue;


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");

        //Colors
        colorValues[0] = new Color(0.0f,0.0f,1.0f,1.0f);
        colorValues[1] = new Color(1.0f,0.0f,0.0f, 1.0f);
        colorValues[2] = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        colorValues[3] = new Color(1.0f, 0.4f, 0.0f, 1.0f);
        ppp = GetComponent<PostProcessingBehaviour>().profile;
        clock = player.GetComponent<PlayerTimeScaleControl>().clock;

        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");       
        for (int i = 0; i < playerCanvas.transform.childCount; i++)
        {
            GameObject child = playerCanvas.transform.GetChild(i).gameObject;
            if (child.name == "FadingScreen")
            {
                fadingScreen = child.GetComponent<Image>();
            }
        }

        ppp.vignette.enabled = true;
        ppp.colorGrading.enabled = true;

        resetProfile();

        color.a = 0.0f;
        targetValue = 1;
        previousTValue = 0;
    }
	
	// Update is called once per frame
	void Update () {
        float dt = Time.deltaTime;
        clock = player.GetComponent<PlayerTimeScaleControl>().clock;
        targetValue = clock.targetValue;
        targetValue = Mathf.Clamp(targetValue, -1, acceleratedTimeValue);


        acceleratedTimeValue = clock.timeValues[4];
        normalTimeValue = clock.timeValues[3];
        slowedTimeValue = clock.timeValues[2];
        pausedTimeValue = clock.timeValues[1];
        rewindTimeValue = clock.timeValues[0];


        if (targetValue!= normalTimeValue)
        {
            if (targetValue != previousTValue)
            {
                canFade = true;
                resetProfile();
                

            }
            if(canFade)//Cada vez que se cambia de target value se reinicia el efecto de fadingScreen
            {
                controlFadingScreen();
                if(hasFaded)
                {
                    notSet = true; //La función se ejecuta hasta que la imagen ha desaparecido (el alpha llega a 0) y se resetea el bool que indica si el efecto ya ha sido activado
                    canFade = false;
                    hasFaded = false;
                }
            }
            calculateColor();
            
            var vigSettings = ppp.vignette.settings;
            vigSettings.intensity = Mathf.MoveTowards(ppp.vignette.settings.intensity,0.35f, colorApplySpeed * dt);
            ppp.vignette.settings = vigSettings;
        }
        else
        {
            
            resetColor();
            var vigSettings = ppp.vignette.settings;
            vigSettings.intensity = Mathf.MoveTowards(ppp.vignette.settings.intensity,0.0f, 0.5f * dt);
            ppp.vignette.settings = vigSettings;
           
            var fadingScreenColor = fadingScreen.color;
            fadingScreenColor.a = 0.0f;
            fadingScreen.color = fadingScreenColor;
            previousTValue = targetValue;
        }
    }

    //Set color with the player variable tValue
    void calculateColor()
    {
        if (clock.rewindActivated)
        {
                color = colorValues[1];
                var gradSettings = ppp.colorGrading.settings;
                gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 2, colorApplySpeed * Time.deltaTime);
                ppp.colorGrading.settings = gradSettings;
                lastColor = "red";
                previousTValue = targetValue;
        }
        else if (clock.pauseActivated)
        {
            color = colorValues[0];            
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.blue.z = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.blue.z, 2, colorApplySpeed * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
            lastColor = "blue";
            previousTValue = targetValue;
        }
        else if (clock.slowActivated)
        {
            color = colorValues[2];
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 2.0f, colorApplySpeed * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.green.y, 2.0f, colorApplySpeed * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;

            lastColor = "yellow";
            previousTValue = targetValue;
        }
        else if (clock.accelActivated)
        {
            color = colorValues[3];
            var gradSettings = ppp.colorGrading.settings;
            gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 2, colorApplySpeed * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.green.y, 1.4f, colorApplySpeed * Time.deltaTime);
            ppp.colorGrading.settings = gradSettings;
            lastColor = "orange";
            previousTValue = targetValue;
        }
        

        

        var vignetteColor = ppp.vignette.settings;
        vignetteColor.color = color;
        ppp.vignette.settings = vignetteColor;
        
    }


    //Set normal color when the effects end
    void resetColor()
    {
        
        var gradSettings = ppp.colorGrading.settings;
        if (gradSettings.channelMixer.red.x != 1 || gradSettings.channelMixer.green.y != 1 || gradSettings.channelMixer.blue.z != 1)
        {
            gradSettings.channelMixer.red.x = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.red.x, 1, 0.7f * Time.deltaTime);
            gradSettings.channelMixer.green.y = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.green.y, 1, 0.6f * Time.deltaTime);
            gradSettings.channelMixer.blue.z = Mathf.MoveTowards(ppp.colorGrading.settings.channelMixer.blue.z, 1, 0.6f * Time.deltaTime);
            
            ppp.colorGrading.settings = gradSettings;
        }
        
    }


    //Reset to normal values the profile
    void resetProfile()
    {
        var vigSettings = ppp.vignette.settings;
        vigSettings.intensity = 0;
        ppp.vignette.settings = vigSettings;

        var gradSettings = ppp.colorGrading.settings;
        gradSettings.channelMixer.red = new Vector3(1.0f, 0.0f, 0.0f);
        gradSettings.channelMixer.green = new Vector3(0.0f, 1.0f, 0.0f);
        gradSettings.channelMixer.blue = new Vector3(0.0f, 0.0f, 1.0f);
        ppp.colorGrading.settings = gradSettings;
    }

    void controlFadingScreen()
    {
        var tempFadingScreenColor = fadingScreen.color;
        if (notSet)
        {
            tempFadingScreenColor.b = color.b;
            tempFadingScreenColor.g = color.g;
            tempFadingScreenColor.r = color.r;

            tempFadingScreenColor.a = 0.7f;
            notSet = false;
            hasFaded = false;
        }
        else
        {
            tempFadingScreenColor.b = color.b;
            tempFadingScreenColor.g = color.g;
            tempFadingScreenColor.r = color.r;

            tempFadingScreenColor.a = Mathf.MoveTowards(fadingScreen.color.a, 0.0f, 3f * Time.deltaTime);
            if(fadingScreen.color.a == 0)
            {
                hasFaded = true;
            }
        }
 
        fadingScreen.color = tempFadingScreenColor;
    }

}
