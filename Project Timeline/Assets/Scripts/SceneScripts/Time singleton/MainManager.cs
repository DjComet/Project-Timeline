using System;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using UnityEngine;

public class MainManager : SingletonMonoBehaviour<MainManager>
{

  /********************************************************************/
  /********************************************************************/

  // Protect constructor
  protected MainManager() { }

  /********************************************************************/
  /********************************************************************/

  // Awake
  void Awake()
  {
    StartUpObject startUpObject = GameObject.FindObjectOfType<StartUpObject>();
    if ( startUpObject != null )
    {
      // Create every manager needed
      //SomeManager.CreateInstance();
      //AnotherManager.CreateInstance();
    }
  }

  /********************************************************************/

  // Use this for initialization
  void Start()
  {
  
  }

  /********************************************************************/
  private void Update()
  {
      //SomeManager.GetInstance().Update();
      //AnotherManager.GetInstance().Update();
  }
}
