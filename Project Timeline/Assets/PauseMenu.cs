using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameIsPaused = false;

    public bool inOptionsMenu = false;
    public bool inControlsMenu = false;

    public GameObject PauseMenuUI;
    public GameObject OptionsMenuUI;
    public GameObject ControlsMenuUI;
    // Update is called once per frame
    void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused)
            {
                if (inOptionsMenu)
                {
                    PauseMenuUI.SetActive(true);
                    OptionsMenuUI.SetActive(false);
                    inOptionsMenu = false;
                }
                else if (inControlsMenu)
                {
                    PauseMenuUI.SetActive(true);
                    ControlsMenuUI.SetActive(false);
                    inControlsMenu = false;
                }
                else
                {
                    Resume();
                }             
                                  
                
            }
            else
            {                
                Pause();
            }
        }
	}


    public void Resume()
    {
        Cursor.visible = false;
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        Cursor.visible = true;
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Quit()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("Menu");
    }

    public void InControlsMenu()
    {
        inControlsMenu = true;
    }

    public void InOptionsMenu()
    {
        inOptionsMenu = true;
    }

    public void OutControlsMenu()
    {
        inControlsMenu = false;
    }

    public void OutOptionsMenu()
    {
        inOptionsMenu = false;
    }
}
