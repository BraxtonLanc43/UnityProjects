using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrPlayer : MonoBehaviour
{
    //Class variables
    public Direction currentDir;
    public Vector2 input;
    public Animator animator;
    public float walkSpeed = 3.0f;
    public float t;
    public bool isMoving;
    public bool isAllowedToMove;
    public Vector3 startPos;
    public Vector3 endPos;
    public GameObject turtle;
    public bool toMove;
    public bool lockedIn = false;

    // Use this for initialization
    void Start()
    {
        currentDir = Direction.South;
        toMove = true;
        animator = GetComponent<Animator>();
        isAllowedToMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Render layer based on y coordinate
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;

        if (!isMoving && lockedIn)
            return;

        //able to move unless instructed otherwise
        toMove = true;
        #region Animation handler
        //Animation handler
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            toIdleEast();
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            toIdleWest();
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            toIdleNorth();
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            toIdleSouth();
        }
        //End animation/movement handler
        #endregion

        #region Movement handler
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
        
        if (Mathf.Abs(input.x) > Mathf.Abs(input.y) && Mathf.Abs(input.x) >= 0.25)
        {
            input.y = 0;
        }
        else if(Mathf.Abs(input.y) >= 0.25)
        {
            input.x = 0;
        }
        else if(input.x > 0 && Mathf.Abs(input.x) < 0.25)
        {
            currentDir = Direction.East;
            toMove = false;
        }
        else if (input.x < 0 && Mathf.Abs(input.x) < 0.25)
        {
            currentDir = Direction.West;
            toMove = false;
        }
        else if (input.y > 0 && Mathf.Abs(input.y) < 0.25)
        {
            currentDir = Direction.North;
            toMove = false;
        }
        else if (input.y < 0 && Mathf.Abs(input.y) < 0.25)
        {
            currentDir = Direction.South;
            toMove = false;
        }

        if (!isMoving && isAllowedToMove)
        {
            if (input != Vector2.zero && toMove)
            {
                if (input.x < 0)
                {
                    currentDir = Direction.West;
                    toWalkWest();
                    turtle.GetComponent<scrTurtle>().toWalkWest();
                }
                if (input.x > 0)
                {
                    currentDir = Direction.East;
                    toWalkEast();
                    turtle.GetComponent<scrTurtle>().toWalkEast();
                }
                if (input.y < 0)
                {
                    currentDir = Direction.South;
                    toWalkSouth();
                    turtle.GetComponent<scrTurtle>().toWalkSouth();
                }
                if (input.y > 0)
                {
                    currentDir = Direction.North;
                    toWalkNorth();
                    turtle.GetComponent<scrTurtle>().toWalkNorth();
                }
                StartCoroutine(Move(transform));
                StartCoroutine(turtle.GetComponent<scrTurtle>().Move(turtle.gameObject.transform, gameObject.transform.position, currentDir.ToString()));
            }
            //else
            //{
            //    //switch (currentDir)
            //    //{
            //    //    case Direction.North:
            //    //        toIdleNorth();
            //    //        break;
            //    //    case Direction.East:
            //    //        toIdleEast();
            //    //        break;
            //    //    case Direction.South:
            //    //        toIdleSouth();
            //    //        break;
            //    //    case Direction.West:
            //    //        toIdleWest();
            //    //        break;
            //    //}
            //}
        }
        #endregion
    }


    /*
    * @param Vector2 startPos, Vector2 endPos
    * @returns bool canMove
    * @desc Checks to see if there is a collider in the way for the player to move
    * @status Working
    */
    public bool canMove(Vector2 startPos, Vector2 endPos)
    {
        if (Physics2D.OverlapPoint(endPos) != null)
        {
            Debug.Log("Blocked");
            return false;
        }


        return true;
    }



    /*
    * @param Transform
    * @returns null/0
    * @desc Moves player via Lerp/Time.deltaTime
    * @status Working
    */
    public IEnumerator Move(Transform entity)
    {
        isMoving = true;
        startPos = entity.position;
        t = 0;

        endPos = new Vector3(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y), startPos.z);

        if (canMove(startPos, endPos))
        {
            while (t < 1f)
            {
                t += Time.deltaTime * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
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
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toWalkEast", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkWest" animation
    * @status Working
    */
    public void toWalkWest()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toWalkWest", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkNorth" animation
    * @status Working
    */
    public void toWalkNorth()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toWalkNorth", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkSouth" animation
    * @status Working
    */
    public void toWalkSouth()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toWalkSouth", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "IdleEast" animation
    * @status Working
    */
    public void toIdleEast()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toIdleEast", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "IdleWest" animation
    * @status Working
    */
    public void toIdleWest()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleWest", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "IdleSouth" animation
    * @status Working
    */
    public void toIdleSouth()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleSouth", true);
    }

    /*
    * @param none
    * @returns null
    * @desc Plays "WalkEast" animation
    * @status Working
    */
    public void toIdleNorth()
    {
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleNorth", true);
    }
}

public enum Direction
{
    North,
    East,
    South,
    West
}