using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scr_MainMenuManager : MonoBehaviour {

    public Text scoreText;

    private void Start()
    {
        scoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
    }



    /*
    *@param none
    *@returns null
    *@desc Loads main menu scene
    */
    public void toMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }


    /*
   *@param none
   *@returns null
   *@desc Loads how to play scene
   */
    public void toHowToPlay()
    {
        SceneManager.LoadScene("How To Play");
    }



    /*
    *@param none
    *@returns null
    *@desc Loads Game scene
    */
    public void toGame()
    {
        SceneManager.LoadScene("Game");
    }


    /*
    *@param none
    *@returns null
    *@desc Quits the game
    */
    public void toQuit()
    {
        Application.Quit();
    }

}
