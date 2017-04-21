using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniRace_Turtle : MonoBehaviour {

    public bool playing;
    public bool waiting;
    public KeyCode currentKey = KeyCode.Space;
    public bool isMoving;
    public float walkSpeed = 3.0f;
    public string TEST = "";
    private Animator animator;
    public GameObject dirtPath;
    public bool gameOver;
    public GameObject ui_Key;
    public GameObject finishLine;
    public GameObject rabbit;
    public MiniRace_Rabbit src_Rabbit;
    public GameObject manager;
    public MiniRace_Manager src_Manager;

    // Use this for initialization
    void Start () {
        ui_Key.SetActive(false);
        gameOver = false;
        animator = GetComponent<Animator>();
        spawnPosition();
        toIdleEast();
        playing = false;
        waiting = false;
        isMoving = false;
        src_Rabbit = rabbit.GetComponent<MiniRace_Rabbit>();
        src_Manager = manager.GetComponent<MiniRace_Manager>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (gameOver)
            return; 

        if(transform.position.x >= finishLine.transform.position.x)
        {
            gameOver = true;
            src_Rabbit.turtleFinished = true;
            toIdleEast();
            src_Manager.winner("Turtle");
        }

        Debug.Log("Current: " + TEST.ToString());
        if (playing && waiting && !isMoving)
        {
            if (Input.GetKeyDown(currentKey))
            {
                //move Turtle forward
                Vector3 endPosition = new Vector3(transform.position.x + 0.7f, transform.position.y, transform.position.z);
                StartCoroutine(Move(transform, endPosition));

                //generate next key
                ui_Key.SetActive(false);
                genKey();
            }
        }
	}


    /*
   * @param none
   * @returns null
   * @desc Spawns on the left of the scren
   * @status Working
   */
    public void spawnPosition()
    {
        Vector3 dirtpos = Camera.main.WorldToViewportPoint(dirtPath.transform.position);
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.25f, dirtpos.y, dirtpos.z));
    }


    /*
   * @param none
   * @returns null
   * @desc Plays "WalkEast" animation
   * @status Working
   */
    public void toWalkEast()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkWest" animation
    * @status Working
    */
    public void toWalkWest()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkNorth" animation
    * @status Working
    */
    public void toWalkNorth()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkSouth" animation
    * @status Working
    */
    public void toWalkSouth()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "IdleEast" animation
    * @status Working
    */
    public void toIdleEast()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "IdleWest" animation
    * @status Working
    */
    public void toIdleWest()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "IdleSouth" animation
    * @status Working
    */
    public void toIdleSouth()
    {
        animator.SetBool("toIdleNorth_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkEast" animation
    * @status Working
    */
    public void toIdleNorth()
    {
        animator.SetBool("toWalkEast_Turtle", false);
        animator.SetBool("toWalkNorth_Turtle", false);
        animator.SetBool("toIdleSouth_Turtle", false);
        animator.SetBool("toWalkSouth_Turtle", false);
        animator.SetBool("toIdleWest_Turtle", false);
        animator.SetBool("toWalkWest_Turtle", false);
        animator.SetBool("toIdleEast_Turtle", false);
        animator.SetBool("toIdleNorth_Turtle", true);
    }

    /*
   * @param Transform entityToMove, Vector3 destinationPosition
   * @returns null/0
   * @desc Moves to spot (coroutine)
   * @status Working
   */
    public IEnumerator Move(Transform entity, Vector3 endingPosition)
    {
        isMoving = true;
        toWalkEast();
        Vector3 startPos = entity.position;
        float t = 0;

        Vector3 endPos = endingPosition;

        while (t < 1f)
        {
            t += (Time.deltaTime * walkSpeed);
            entity.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
        yield return 0;
    }

    /*
   * @param none
   * @returns null
   * @desc Generates a random key
   * @status Working
   */
    public void genKey()
    {
        KeyCode curKey = getRandomKey();
        currentKey = curKey;
        displayKeyToPress(currentKey);
        playing = true;
        waiting = true;
    }

    /*
   * @param none
   * @returns KeyCode randomKey
   * @desc Chooses a key at random
   * @status Working
   */
    public KeyCode getRandomKey()
    {
        int rand = (int)Random.Range(0, 26);

        switch (rand)
        {
            case 0:
                return KeyCode.A;
            case 1:
                return KeyCode.B;
            case 2:
                return KeyCode.C;
            case 3:
                return KeyCode.D;
            case 4:
                return KeyCode.E;
            case 5:
                return KeyCode.F;
            case 6:
                return KeyCode.G;
            case 7:
                return KeyCode.H;
            case 8:
                return KeyCode.I;
            case 9:
                return KeyCode.J;
            case 10:
                return KeyCode.K;
            case 11:
                return KeyCode.L;
            case 12:
                return KeyCode.M;
            case 13:
                return KeyCode.N;
            case 14:
                return KeyCode.O;
            case 15:
                return KeyCode.P;
            case 16:
                return KeyCode.Q;
            case 17:
                return KeyCode.R;
            case 18:
                return KeyCode.S;
            case 19:
                return KeyCode.T;
            case 20:
                return KeyCode.U;
            case 21:
                return KeyCode.V;
            case 22:
                return KeyCode.W;
            case 23:
                return KeyCode.X;
            case 24:
                return KeyCode.Y;
            case 25:
                return KeyCode.Z;
            case 26:
                return KeyCode.Space;
        }

        return KeyCode.Space;
    }

    /*
   * @param KeyCode key
   * @returns null
   * @desc Displays the Key to press next to move
   * @status Working
   */
    public void displayKeyToPress(KeyCode key)
    {
        string toDisplay = "";

        switch (key)
        {
            case KeyCode.A:
                toDisplay = "A";
                break;
            case KeyCode.B:
                toDisplay = "B";
                break;
            case KeyCode.C:
                toDisplay = "C";
                break;
            case KeyCode.D:
                toDisplay = "D";
                break;
            case KeyCode.E:
                toDisplay = "E";
                break;
            case KeyCode.F:
                toDisplay = "F";
                break;
            case KeyCode.G:
                toDisplay = "G";
                break;
            case KeyCode.H:
                toDisplay = "H";
                break;
            case KeyCode.I:
                toDisplay = "I";
                break;
            case KeyCode.J:
                toDisplay = "J";
                break;
            case KeyCode.K:
                toDisplay = "K";
                break;
            case KeyCode.L:
                toDisplay = "L";
                break;
            case KeyCode.M:
                toDisplay = "M";
                break;
            case KeyCode.N:
                toDisplay = "N";
                break;
            case KeyCode.O:
                toDisplay = "O";
                break;
            case KeyCode.P:
                toDisplay = "P";
                break;
            case KeyCode.Q:
                toDisplay = "Q";
                break;
            case KeyCode.R:
                toDisplay = "R";
                break;
            case KeyCode.S:
                toDisplay = "S";
                break;
            case KeyCode.T:
                toDisplay = "T";
                break;
            case KeyCode.U:
                toDisplay = "U";
                break;
            case KeyCode.V:
                toDisplay = "V";
                break;
            case KeyCode.W:
                toDisplay = "W";
                break;
            case KeyCode.X:
                toDisplay = "X";
                break;
            case KeyCode.Y:
                toDisplay = "Y";
                break;
            case KeyCode.Z:
                toDisplay = "Z";
                break;
            case KeyCode.Space:
                toDisplay = "H";
                break;
        }


        //Set the canvas button to active and change the text on it
        waiting = true;
        TEST = toDisplay;

        //Display in UI
        GameObject can = ui_Key.transform.Find("Canvas").gameObject;
        GameObject pan = can.transform.Find("Panel").gameObject;
        GameObject txt = pan.transform.Find("Text").gameObject;        
        Text txt_Text = txt.GetComponent<Text>();
        //GameObject ui_Key_Active = Instantiate(ui_Key);
        txt_Text.text = toDisplay.ToString();
        ui_Key.SetActive(true);
    }
}
