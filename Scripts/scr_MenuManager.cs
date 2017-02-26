using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class scr_MenuManager : MonoBehaviour {

	public void PlayButton()
    {
        Debug.Log("Test play");
        SceneManager.LoadScene("Game");
    }

    public void QuitButton()
    {
        Debug.Log("Test quit");
        Application.Quit();
    }

    public void HowToPlayButton()
    {
        //pull up new scene with directions?
        SceneManager.LoadScene("HowToPlay");
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
