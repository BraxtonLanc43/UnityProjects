using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class scr_HowToPlayManager : MonoBehaviour {

    Animation animation;
    public GameObject tapUp;
    public GameObject tapDown;
    public GameObject monkey;
    public GameObject gorilla;
    public GameObject banana;
    public GameObject whatDoesEat;
    public GameObject likesBananas;
    public GameObject crazyAboutBananas;
    public GameObject arrowMonkey;
    public GameObject arrowBanana;
    public GameObject arrowGorilla;
    public GameObject monkeyHungry;
    public GameObject gorillaText;
    public GameObject wantToHelp;
    public GameObject sweet_ToTapDown;
    public GameObject whatElseElse;
    public GameObject grabBananasText;
    public GameObject ohNo;
    public GameObject letsGo;
    public GameObject pooDirectionsText;
    public GameObject pooButton;
    public GameObject pooObj;
    public GameObject arrowPoo;


	// Use this for initialization
	void Start ()
    {
        arrowPoo.SetActive(false);
        pooObj.SetActive(false);
        pooButton.SetActive(false);
        pooDirectionsText.SetActive(false);
        ohNo.SetActive(false);
        animation = GetComponent<Animation>();
        tapUp.SetActive(false);
        tapDown.SetActive(false);        
        gorilla.SetActive(false);
        banana.SetActive(false);
        likesBananas.SetActive(false);
        arrowBanana.SetActive(false);
        crazyAboutBananas.SetActive(false);
        arrowGorilla.SetActive(false);
        gorillaText.SetActive(false);
        wantToHelp.SetActive(false);
        sweet_ToTapDown.SetActive(false);
        whatElseElse.SetActive(false);
        grabBananasText.SetActive(false);
        letsGo.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        
    }



    public void to_HeLikesBananas()
    {        
        whatDoesEat.SetActive(false);
        monkeyHungry.SetActive(false);
        arrowMonkey.SetActive(false);
        likesBananas.SetActive(true);
        banana.SetActive(true);
        arrowBanana.SetActive(true);
        crazyAboutBananas.SetActive(true);
    }


    public void to_AngryGorillas()
    {
        likesBananas.SetActive(false);
        arrowBanana.SetActive(false);
        crazyAboutBananas.SetActive(false);
        gorilla.SetActive(true);
        arrowGorilla.SetActive(true);
        gorillaText.SetActive(true);
        wantToHelp.SetActive(true);
    }


    public void to_TapUp()
    {
        wantToHelp.SetActive(false);
        arrowGorilla.SetActive(false);
        gorillaText.SetActive(false);
        tapUp.SetActive(true);
        sweet_ToTapDown.SetActive(true);
    }


    public void to_TapDown()
    {
        sweet_ToTapDown.SetActive(false);
        tapUp.SetActive(false);
        tapDown.SetActive(true);
        whatElseElse.SetActive(true);
    }


    public void to_GrabBananas()
    {
        tapDown.SetActive(false);
        whatElseElse.SetActive(false);
        grabBananasText.SetActive(true);
        ohNo.SetActive(true);
    }


    public void to_PooIntro()
    {
        grabBananasText.SetActive(false);
        ohNo.SetActive(false);
        letsGo.SetActive(true);
        pooDirectionsText.SetActive(true);
        pooButton.SetActive(true);
        pooObj.SetActive(true);
        arrowPoo.SetActive(true);
    }


    public void to_Game()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
