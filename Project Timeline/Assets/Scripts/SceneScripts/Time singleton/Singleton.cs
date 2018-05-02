using UnityEngine;
using System.Collections;

public abstract class Singleton<T> where T : Singleton<T>, new()
{

  /********************************************************************/

  private static T      mInstance;
  private static object mLock = new object();

  /********************************************************************/

  protected Singleton( ) { }

  /********************************************************************/
  /********************************************************************/

  public static T     GetInstance( )
  {
    return mInstance;
  }

  /********************************************************************/

  public static void  CreateInstance( )
  {
    lock ( mLock )
    {
      if( mInstance == null )
      {
        mInstance = new T();

        Debug.Log( "[Singleton] An instance of " + typeof( T ) +
          " was needed in the scene, so it was created" );
      }
      else {
        Debug.LogError( "[Singleton] Trying to create a duplicate instance of " + typeof( T ) + "!!!" );
      }
    }
  }

  /********************************************************************/
}