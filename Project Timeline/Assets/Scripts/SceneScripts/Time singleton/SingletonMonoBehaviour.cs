using UnityEngine;
using System.Collections;

/// <summary>
/// Be aware this will not prevent a non singleton constructor
///   such as 'T myT = new T();'
/// To prevent that, add 'protected T () {}' to your singleton class.
/// 
/// As a note, this is made as MonoBehaviour because we need Coroutines.
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
  /********************************************************************/

  private static T      mInstance             = null;
  private static object mLock                 = new object();
  private static bool   applicationIsQuitting = false;

  /********************************************************************/
  /********************************************************************/

  public static T GetInstance( )
  {
    if( applicationIsQuitting )
    {
      Debug.LogWarning( "[SingletonMonoBehaviour] Instance '" + typeof( T ) +
        "' already destroyed on application quit." +
        " Won't create again - returning null." );
      return null;
    }

    if( FindObjectsOfType( typeof( T ) ).Length > 1 ) {
      Debug.LogError( "[SingletonMonoBehaviour] Something went really wrong " +
        " - there should never be more than 1 singleton!" +
        " Reopening the scene might fix it." );
    }

    return mInstance;
  }

  /********************************************************************/

  public static void CreateInstance( )
  {
    lock ( mLock ) {
      if( mInstance == null ) {
        Debug.Log( "[SingletonMonoBehaviour] Creating Instance" );

        GameObject singleton = new GameObject();
        mInstance = singleton.AddComponent<T>();
        singleton.name = "(singleton) " + typeof( T ).ToString();

        DontDestroyOnLoad( singleton );

        Debug.Log( "[SingletonMonoBehaviour] An instance of " + typeof( T ) +
          " is needed in the scene, so '" + singleton +
          "' was created with DontDestroyOnLoad." );
      }
      else {
        Debug.LogError( "[SingletonMonoBehaviour] Instance already created!! " +
          mInstance.gameObject.name );
      }
    }
  }

  /********************************************************************/

    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
  public void OnDestroy( )
  {
    applicationIsQuitting = true;
  }

  /********************************************************************/
}