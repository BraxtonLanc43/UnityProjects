using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    private float score = 0.0f;
    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 10;
    private int scoreToNextLevel = 10;
    private bool isDead = false;

    public DeathMenuManager menu;

    public Text scoreText;
	
	// Update is called once per frame
	void Update () {

        if (isDead)
        {
            return;
        }

        if(score >= scoreToNextLevel)
        {
            levelUp();
        }

        score += Time.deltaTime * difficultyLevel;
        scoreText.text = ((int)score).ToString();
	}

    void levelUp()
    {
        if(difficultyLevel == maxDifficultyLevel)
        {
            return;
        }

        scoreToNextLevel *= 2;
        difficultyLevel+=2;

        GetComponent<PlayerMotor>().setSpeed(difficultyLevel);
    }

    public void onDeath()
    {
        isDead = true;
        if (score > PlayerPrefs.GetFloat("HighScore"))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }
        else
        {
            PlayerPrefs.SetFloat("HighScore", PlayerPrefs.GetFloat("HighScore"));
        }
        
        menu.toggleEndMenu(score);
    }
}
