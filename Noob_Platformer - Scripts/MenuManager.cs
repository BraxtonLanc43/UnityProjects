using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public Text scoreText;
    private int score;

    private void Start()
    {
        score = PlayerPrefs.GetInt("PlayerScore");
        scoreText.text = "High Score : " + score.ToString();
    }

	public void ToGame()
    {
        SceneManager.LoadScene("Level_1");
    }
}
