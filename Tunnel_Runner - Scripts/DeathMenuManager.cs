using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeathMenuManager : MonoBehaviour {

    public Text scoreText;
    public Image backgroundImage;

    private bool isShown = false;
    private float transition = 0.0f;

	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isShown)
        {
            return;
        }

        transition += Time.deltaTime;
        backgroundImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);
	}

    public void toggleEndMenu(float score)
    {
        gameObject.SetActive(true);
        scoreText.text = ((int)(score)).ToString();
        isShown = true;
    }

    public void playAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
