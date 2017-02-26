using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeathMenuManager : MonoBehaviour {

    public Text gameoverText;
    public Image backgroundImage;
    public AudioClip gameOverAudio;
    private AudioSource audiosource;

    private bool isShown = false;
    private float transition = 0.0f;

    // Use this for initialization
    void Start()
    {
        audiosource = GetComponent<AudioSource>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShown)
        {
            return;
        }

        transition += Time.deltaTime;
        backgroundImage.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, transition);
    }

    public void toggleEndMenu()
    {
        gameObject.SetActive(true);
        audiosource.clip = gameOverAudio;
        audiosource.Play();
        isShown = true;
    }
}
