using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class scr_GameManager : MonoBehaviour
{
    public GameObject enemy_Gorilla;
    public GameObject banana_coin;

    public scr_Player player;
    private AudioSource audioSource;
    public AudioClip gameMusic;

    public Button btn_MainMenu;
    public Button btn_PlayAgain;
    public Button btn_Quit;
    private float startTime;

    void Awake()
    {
        startTime = Time.time;
    }

    // Use this for initialization
    void Start()
    {
        btn_MainMenu.gameObject.SetActive(false);
        btn_PlayAgain.gameObject.SetActive(false);
        btn_Quit.gameObject.SetActive(false);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = gameMusic;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (player.isDead)
        {
            audioSource.Stop();
            return;
        }

        int score = player.score;
        int count_Enemies = (GameObject.FindGameObjectsWithTag("Gorilla")).Length;
        int count_Bananas = (GameObject.FindGameObjectsWithTag("Banana")).Length;

        //every .5 seconds, we'll potentially spawn things, which will be decided at random
        if(Time.time - startTime > .5)
        {
            SpawnEnemy(getYTopOfScreen(), getYBottomOfScreen(), getXRightOfScreen(), score);
            SpawnBanana(getYTopOfScreen(), getYBottomOfScreen(), getXRightOfScreen(), score);
            startTime = Time.time;
        }
        
        if (!audioSource.isPlaying && player.isDead == false)
        {
            audioSource.Play();
        }
       
    }


    /*
    * @param none
    * @returns float y
    * @desc gets the y position for the bottom of the screen, based on the screen size
    */
    private float getYTopOfScreen()
    {
        Vector3 worldTop = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -20.5f));

        return worldTop.y;
    }



    /*
    * @param none
    * @returns float y
    * @desc gets the y position for the top of the screen, based on the screen size
    */
    private float getYBottomOfScreen()
    {
        Vector3 worldBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -20.5f));
        
        return worldBottom.y;
    }



    /*
   * @param none
   * @returns float y
   * @desc gets the y position for the top of the screen, based on the screen size
   */
    private float getXRightOfScreen()
    {        
        Vector3 worldRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * 3.0f, 0, player.transform.position.z));

        return worldRight.x;
    }



    /*
    *@param none
    *@returns null
    *@desc Sets the three menu buttons active in the game scene
    */
    public void SetMenuActive()
    {
        btn_MainMenu.gameObject.SetActive(true);
        btn_PlayAgain.gameObject.SetActive(true);
        btn_Quit.gameObject.SetActive(true);
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
    *@desc Loads Game scene
    */
    public void toGame()
    {
        SceneManager.LoadScene("Game");
    }


    /*
    * @param float y_TopOfScreen (y position for the top of the screen), float y_BottomOfScreen (y position for the bottom of the screen)
    *        float x_RightOfScreen (right edge of screen), int playerScore (in case want to increase difficulty in correlation with the score)
    * @returns null
    * Desc: Spawns an enemy at a random location within the vertical bounds of the screen, but off the screen to the right
    */
    private void SpawnEnemy(float y_TopOfScreen, float y_BottomOfScreen, float x_RightOfScreen, int playerScore)
    {
        int randomToSpawnOrNot = (int)Mathf.Ceil(Random.Range(0, 10));

        if (randomToSpawnOrNot >=0 && randomToSpawnOrNot <=8)
        {
            float ySpawn = Random.Range(y_BottomOfScreen, y_TopOfScreen);
            Vector3 spawnPoint = new Vector3(x_RightOfScreen * 1.5f, ySpawn, player.transform.position.z);  //Need to decide on logic for spawning
            GameObject freshSpawn = Instantiate(enemy_Gorilla);
            freshSpawn.transform.position = spawnPoint;
        }
    }


    /*
    * @param float y_TopOfScreen (y position for the top of the screen), float y_BottomOfScreen (y position for the bottom of the screen)
    *        float x_RightOfScreen (right edge of screen), int playerScore (in case want to increase difficulty in correlation with the score)
    * @returns null
    * Desc: Spawns a banana coin at a random location within the vertical bounds of the screen, but off the screen to the right
    */
    private void SpawnBanana(float y_TopOfScreen, float y_BottomOfScreen, float x_RightOfScreen, int playerScore)
    {
        int randomToSpawnOrNot = (int)Mathf.Ceil(Random.Range(0, 2));
       
        if(randomToSpawnOrNot == 1)
        {
            float ySpawn = Random.Range(y_BottomOfScreen, y_TopOfScreen);
            Vector3 spawnPoint = new Vector3(x_RightOfScreen * 1.5f, ySpawn, player.transform.position.z);  //Need to decide on logic for spawning
            GameObject freshSpawn = Instantiate(banana_coin);
            freshSpawn.transform.position = spawnPoint;
        }
    }



    /*
    *@param none
    *@returns null
    *@desc Exits the game
    */
    public void toQuit()
    {
        Application.Quit();
    }
}



