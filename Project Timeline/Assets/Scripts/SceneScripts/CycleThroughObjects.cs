using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughObjects : TimeManagerScript {
    //A
    public int timeLayer = 9;
    public List<GameObject> objectsInTimeLayer;
    
    public int iterated = 0;
	// Use this for initialization
	void Awake () {

        mainClock = MainClock.mainClock;
        GameObject[] goArray = FindObjectsOfType<GameObject>();
        for(int i = 0; i < goArray.Length; i++)
        {
            if(goArray[i].layer == timeLayer)
            {
                objectsInTimeLayer.Add(goArray[i]);
                iterated++;
            }
        }
        
}
	
	// Update is called once per frame
	void Update () {
		
        /*for(int i = 0; i< objectsInTimeLayer.Count; i++)
        {
            
            objectsInTimeLayer[i].GetComponent<ScaledDeltaTime>().scaledDT = Time.deltaTime * timeScaleControl.ownTimeScale; //Otra forma de recorrer la lista (Igual de ineficiente¿? No se...)
            
        }*/

        foreach(GameObject go in objectsInTimeLayer)
        {
            if (go.GetComponent<TimeLine>() != null)
            {

                go.GetComponent<TimeLine>().clock = MainClock.mainClock;

                
            }           
        }
	}
}
