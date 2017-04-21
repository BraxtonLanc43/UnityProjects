using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniRace_Manager : MonoBehaviour
{

    public GameObject playCanvas;
    public GameObject turtle;
    public MiniRace_Turtle scr_Turtle;
    public GameObject rabbit;
    public MiniRace_Rabbit scr_Rabbit;
    public GameObject winnerCanvas;


    // Use this for initialization
    void Start()
    {
        winnerCanvas.SetActive(false);
        scr_Turtle = turtle.GetComponent<MiniRace_Turtle>();
        scr_Rabbit = rabbit.GetComponent<MiniRace_Rabbit>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void startRace()
    {
        playCanvas.SetActive(false);
        scr_Turtle.genKey();
        scr_Rabbit.run();
    }


    public void winner(string win)
    {
        string whoWon = "";

        switch (win)
        {
            case "Turtle":
                whoWon = "Winner!";
                break;
            case "Rabbit":
                whoWon = "You Lost!";
                break;
            default:
                whoWon = "NOBODY WINS";
                break;
        }

        GameObject pan = winnerCanvas.transform.Find("Panel").gameObject;
        GameObject txt = pan.transform.Find("Text").gameObject;
        Text winText = txt.GetComponent<Text>();
        winText.text = whoWon;
        winnerCanvas.SetActive(true);
    }


}
