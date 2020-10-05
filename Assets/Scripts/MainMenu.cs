using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Tooltip("Place the MainMenuCanvas here")]
    [SerializeField] GameObject mainMenuCanvas;
    [Tooltip("Place the ControlsMenuCanvas here")]
    [SerializeField] GameObject controlsMenuCanvas;
    [Tooltip("Place the CreditsMenuCanvas here")]
    [SerializeField] GameObject creditsMenuCanvas;

    GameObject curMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Loads the desired screen based on the scene index in the build order

    public void SceneSelect(int sceneIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneIndex);
    }

    //Return to the Main Menu

    public void ReturnMainMenu()
    {
        curMenu.SetActive(false);
        mainMenuCanvas.SetActive(true);
    }

    //Load the controls menu

    public void ControlsMenu()
    {
        mainMenuCanvas.SetActive(false);
        controlsMenuCanvas.SetActive(true);
        curMenu = controlsMenuCanvas;
    }

    //Load the credits menu

    public void CreditsMenu()
    {
        mainMenuCanvas.SetActive(false);
        creditsMenuCanvas.SetActive(true);
        curMenu = creditsMenuCanvas;
    }

    //Quits the game, even when playing in the editor

    public void QuitGame()
    {
        if (Application.isEditor)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
        else
        {
            Application.Quit();
        }

    }
}
