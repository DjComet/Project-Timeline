using UnityEngine;
using System.Collections;

public class StartUpObject : MonoBehaviour
{

  /********************************************************************/

  // Create managers
  void Awake( )
  {
    Debug.Log( "Start-up Object Awake" );

    Screen.fullScreen = true;

    if( MainManager.GetInstance() == null ) {
      MainManager.CreateInstance();
    }
  }

  /********************************************************************/

  // Use this for initialization
  void Start( )
  {
  }

  /********************************************************************/

  // Update is called once per frame
  void Update( )
  {
  }

  /********************************************************************/

}
