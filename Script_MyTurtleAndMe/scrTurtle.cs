using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrTurtle : MonoBehaviour
{
    float playerOffsetY;
    float playerOffsetX;
    private Direction currentDir;
    private Vector2 input;
    private float walkSpeed = 3.0f;
    private float t;
    private bool isMoving;
    public bool isAllowedToMove;
    private Vector3 startPos;
    private Vector3 endPos;
    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }
    
    //Call every frame
    void Update()
    {
        //Render layer based on y coordinate
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;

        #region Animation handler
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            toWalkEast();
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            toIdleEast();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            toWalkWest();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            toIdleWest();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            toWalkNorth();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            toIdleNorth();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            toWalkSouth();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            toIdleSouth();
        }
        //End animation/movement handler
        #endregion


    }


    /*
    * @param Transform
    * @returns null/0
    * @desc Moves player via Lerp/Time.deltaTime
    * @status Working
    */
    public IEnumerator Move(Transform entity, Vector3 endingPosition, string currentDir)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;
        
        endPos = endingPosition;

        while (t < 1f)
        {
            t += Time.deltaTime * walkSpeed;
            entity.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        isMoving = false;
        yield return 0;
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
}