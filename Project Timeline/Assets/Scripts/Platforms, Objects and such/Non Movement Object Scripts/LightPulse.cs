using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPulse : MonoBehaviour {

    public GameObject Parent;
    private ActivatorBehaviour aB;

    public float minIntensity = 2.5f;
    public float maxIntensity = 6f;
    [HideInInspector]
    public float initMinI;
    [HideInInspector]
    public float initMaxI;
    public float pulseSpeed = 2f;
    public bool canLerpBetweenColors = false;
    public Color[] colorArray;
    Light pointLight;

    TimeLine timeLine;

    public float lerpSpeed = 0.75f;
    public float t = 0;

    int a, b;
	// Use this for initialization
	void Start () {
        pointLight = GetComponent<Light>();
        a = 0;
        b = 1;
        timeLine = GetComponent<TimeLine>();
        if(Parent != null && Parent.GetComponent<ActivatorBehaviour>())
        {
            aB = Parent.GetComponent<ActivatorBehaviour>();
        }

        initMinI = minIntensity;
        initMaxI = maxIntensity;
    }
	
	// Update is called once per frame
	void Update () {

        if(Parent!= null && aB != null && aB.active)
        {
            pingPongs();
            lerpBetweenColors();
        }
        else if(Parent != null && aB != null && !aB.active)
        {
            pointLight.intensity = 0;
        }
        else if(Parent == null)
        {
            pingPongs();
            lerpBetweenColors();
        }
        
    }

    void pingPongs()
    {
        if (timeLine)
        {
            pointLight.intensity = minIntensity + Mathf.PingPong(timeLine.clock.currentTime * pulseSpeed, maxIntensity - minIntensity);
        }
        else
        {
            pointLight.intensity = minIntensity + Mathf.PingPong(Time.time * pulseSpeed, maxIntensity - minIntensity);
        }
    }

    void lerpBetweenColors()
    {
        if (canLerpBetweenColors)
        {
            pointLight.color = Color.Lerp(colorArray[a], colorArray[b], t);

            if (t >= 1)
            {
                a++;
                b++;
                t = 0;
            }

            a = a % colorArray.Length;
            b = b % colorArray.Length;

            t += timeLine.scaledDT * lerpSpeed;
        }

        minIntensity = Mathf.Clamp(minIntensity, initMinI, initMinI+10);
        maxIntensity = Mathf.Clamp(maxIntensity, initMaxI, initMaxI+10);
    }
}
