using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthEnergyInterfaceManager : MonoBehaviour {

    private GameObject playerCanvas;
    private GameObject energyPanel;
    private GameObject fadingHealthGO;
    private Image fadingHealth;
    private GameObject energyBarGO;
    private Image energyBar;
    private GameObject energyBarGO02;
    private Image energyBar02;

    private GameObject energyTextGO;
    private Text energyText;
    private GameObject diedTextGO;  

    private GameObject player;
    private PlayerTimeScaleControl timeScaleControl;
    private Clock clock;
    public float currentEnergy;
    public float maxEnergy;
    private float timerColor;

    private Values playerValues;

    public float playerHealth;
    public float maxPlayerHealth;
    public float currentHealth;
    public float targetHealth;
    public float origenLerp;

    private Color blue = new Color(0.0f, 0.0f, 1.0f, 0.5f);
    private Color red = new Color(1.0f, 0.0f, 0.0f,0.5f);
    private Color fullAlpha = new Color(1.0f, 0.0f, 0.0f, 0.0f);

    

    [SerializeField]
    private float t;
    private float tSave;
    public bool inTranssition;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        clock = player.GetComponent<PlayerTimeScaleControl>().clock;
        playerCanvas = GameObject.FindGameObjectWithTag("PlayerCanvas");
       

        for (int i = 0; i < playerCanvas.transform.childCount; i++)
        {
            GameObject child = playerCanvas.transform.GetChild(i).gameObject;
            if (child.name == "EnergyPanel")
            {
                energyPanel = child;
            }
            else if (child.name == "FadingHealth")
            {
                fadingHealthGO = child;
            }
            else if (child.name == "DiedText")
            {
                diedTextGO = child;
            }

        }

        for (int i = 0; i < energyPanel.transform.childCount; i++)
        {
            GameObject child = energyPanel.transform.GetChild(i).gameObject;
            if (child.name == "Health")
            {
                energyBarGO = child;
            }
            else if (child.name == "EnergyText")
            {
                energyTextGO = child;
            }
            else if (child.name == "Health01")
            {
                energyBarGO02 = child;
            }
        }
        
        energyBar = energyBarGO.GetComponent<Image>();
        energyBar02 = energyBarGO02.GetComponent<Image>();
        fadingHealth = fadingHealthGO.GetComponent<Image>();

        playerValues = player.GetComponent<Values>();

        energyText = energyTextGO.GetComponent<Text>();        

        timeScaleControl = player.GetComponent<PlayerTimeScaleControl>();
        maxEnergy = PlayerEnergy.maxEnergyAmt;

        maxPlayerHealth = playerValues.health;
        playerHealth = playerValues.health;
        currentHealth = playerHealth;
        targetHealth = playerHealth;

        t = 0;

        inTranssition = false;
    }
	
	// Update is called once per frame
	void Update () {
        clock = player.GetComponent<PlayerTimeScaleControl>().clock;
        playerHealth = playerValues.health;

        if(targetHealth != playerHealth)
        {
            targetHealth = playerHealth;
            inTranssition = true;
            t = 0;
            origenLerp = currentHealth;
        }        

        currentEnergy = PlayerEnergy.energyAmount;
        energyBar.fillAmount = (currentEnergy / 5.0f);
        //energyBar02.fillAmount = (currentEnergy / 5.0f);

        if (clock.accelActivated || clock.slowActivated || clock.rewindActivated || clock.pauseActivated)
        {
            energyBar02.color = red;
        }
        else if(currentEnergy < 5.0f)
        {
            energyBar02.color = blue;
        }
        else
        {
            energyBar02.color = fullAlpha;
        }
        if(inTranssition)
        {
            checkHealth();
        }
        
        changeAlphaHealth();

        if(currentHealth <= 0)
        {
            diedTextGO.SetActive(true);
        }
	}

    void checkHealth()
    {
        if(currentHealth != targetHealth)
        {            
            t += Time.deltaTime;
            tSave = t;
            tSave /= 2.5f;
            tSave = Mathf.Clamp01(tSave);

            currentHealth = origenLerp - Mathf.Lerp(0.0f, origenLerp - targetHealth, tSave);
            
            if(tSave>= 1)
            {
                inTranssition = false;                
                t = 0;
                tSave = 0;
            }
        }
    }

    void changeAlphaHealth()
    {
        var colorImage = fadingHealth.color;
        colorImage.a = 1.0f - currentHealth/maxPlayerHealth;
        fadingHealth.color = colorImage;
    }
}
