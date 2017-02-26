using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {
    public static LevelManager Instance { get; set; }

    private int hitpoint = 3;
    private int score = 0;
    public Transform spawnPosition;
    public Transform playerTransform;

    public Text scoreText;
    public Text hitpointText;

    //private void Awake (start)
    private void Awake()
    {
        Instance = this;
        scoreText.text = "Score : " + score.ToString();
        hitpointText.text = "Lives : " + hitpoint.ToString();
    }

    //called every frame
    private void Update()
    {
        if(playerTransform.position.y < -7.5)
        {
            playerTransform.position = spawnPosition.position;
            hitpoint--;
            hitpointText.text = "Lives : " + hitpoint.ToString();
            if (hitpoint <= 0)
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    public void Win()
    {
        if(score > PlayerPrefs.GetInt("PlayerScore"))
        {        
            PlayerPrefs.SetInt("PlayerScore", score);
            SceneManager.LoadScene("Menu");
        }
    }

    public void CollectCoin()
    {
        score++;
        scoreText.text = "Score : " + score.ToString();
    }
}
