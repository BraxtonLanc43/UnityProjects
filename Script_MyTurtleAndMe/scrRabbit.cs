using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrRabbit : MonoBehaviour, scrI_NPC {

    //Consistent properties
    public Animator animator;
    public int count_Engaged;
    public string commentary_First = "sniffy sniff";
    public string commentary_After = "sniff....";

    //Class variables
    Direction currentDir;
    public bool isMoving = false;
    public float t;
    public float walkSpeed = 1.0f;
    public bool hasAlreadyWalkedToDestination = false;


    // Use this for initialization
    void Start()
    {
        //Consistent properties
        animator = GetComponent<Animator>();
        count_Engaged = 0;

        //other
        currentDir = Direction.West;
        faceWest();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
    }


    /*
    * @param void
    * @returns void
    * @desc Logic for controlling commentary for this NPC
    * @status Not Complete
    * @interface scrI_NPC
    */
    public void engageCommentary()
    {
        
    }

    /*
    * @author Braxton Lancial
    * @param Direction direction the engager (callee) is facing
    * @return void
    * @desc Forces the NPC to look at the engager (likely the player)
    * @interface scrI_NPC
    * @status Untested in this script
    */
    public void faceDirection(Direction dirEngagerIsFacing)
    {
        Direction toFace = Direction.North;

        switch (dirEngagerIsFacing)
        {
            case Direction.North:
                toFace = Direction.South;
                break;
            case Direction.South:
                toFace = Direction.North;
                break;
            case Direction.East:
                toFace = Direction.West;
                break;
            case Direction.West:
                toFace = Direction.East;
                break;
        }

        switch (toFace)
        {
            case Direction.North:
                faceNorth();
                break;
            case Direction.South:
                faceSouth();
                break;
            case Direction.East:
                faceEast();
                break;
            case Direction.West:
                faceWest();
                break;
        }
    }

    /*
    * @param void
    * @returns void
    * @desc Flips the sprite to face east
    * @status Untested
    * @interface scrI_NPC
    */
    public void faceEast()
    {
        //Flip to East sprite
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleEast", true);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunWest", false);
        currentDir = Direction.East;
    }

    /*
    * @param void
    * @returns void
    * @desc Flips the sprite to face north
    * @status Untested
    * @interface scrI_NPC
    */
    public void faceNorth()
    {
        //Flip to North sprite
        animator.SetBool("toIdleNorth", true);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunWest", false);
        currentDir = Direction.North;
    }

    /*
    * @param void
    * @returns void
    * @desc Flips the sprite to face south
    * @status Untested
    * @interface scrI_NPC
    */
    public void faceSouth()
    {
        //Flip to South sprite
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleSouth", true);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunWest", false);
        currentDir = Direction.South;
    }

    /*
    * @param void
    * @returns void
    * @desc Flips the sprite to face west
    * @status Untested
    * @interface scrI_NPC
    */
    public void faceWest()
    {
        //Flip to West sprite
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleWest", true);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunWest", false);
        currentDir = Direction.West;
    }


    public bool lineOfSight_Player(Direction dir)
    {
        throw new NotImplementedException();
    }

    /*
    * @param void
    * @returns void
    * @desc Plays additional commentary
    * @status Not Complete
    * @interface scrI_NPC
    */
    public void playAdditionalCommentary()
    {
        throw new NotImplementedException();
    }

    /*
    * @param void
    * @returns void
    * @desc Plays the first commentary
    * @status Not Complete
    * @interface scrI_NPC
    */
    public void playFirstCommentary()
    {
        throw new NotImplementedException();
    }

    public void spotted()
    {
        throw new NotImplementedException();
    }

    /*
    * @author Braxton Lancial
    * @param void
    * @return void
    * @desc Plays animation for running north
    * @interface scrI_NPC
    * @status Untested
    */
    public void toRunNorth()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", true);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunWest", false);
    }


    /*
     * @author Braxton Lancial
     * @param void
     * @return void
     * @desc Plays animation for running south
     * @interface scrI_NPC
     * @status Untested
     */
    public void toRunSouth()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunSouth", true);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunWest", false);
    }


    /*
     * @author Braxton Lancial
     * @param void
     * @return void
     * @desc Plays animation for running east
     * @interface scrI_NPC
     * @status Untested
     */
    public void toRunEast()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunEast", true);
        animator.SetBool("toRunWest", false);
    }


    /*
     * @author Braxton Lancial
     * @param void
     * @return void
     * @desc Plays animation for running west
     * @interface scrI_NPC
     * @status Working
     */
    public void toRunWest()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleEast", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunWest", true);
    }

    public void walkToPlayer(Direction dir)
    {
        throw new NotImplementedException();
    }

    /*
    * @param Transform
    * @returns null/0
    * @desc Moves NPC via Lerp/Time.deltaTime
    * @status Working
    */
    public IEnumerator Move(Transform entity, Vector3 endPos)
    {
        isMoving = true;
        Vector3 startPos = entity.position;
        t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * walkSpeed;
            entity.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        switch (currentDir)
        {
            case Direction.North:
                faceNorth();
                break;
            case Direction.West:
                faceWest();
                break;
            case Direction.South:
                faceSouth();
                break;
            case Direction.East:
                faceEast();
                break;
        }

        isMoving = false;
        hasAlreadyWalkedToDestination = true;
        yield return 0;
    }

   /*
   * @param Vector3 destination
   * @returns null/0
   * @desc Moves NPC to a spot using the Move function, and animates accordingly
   * @status Working
   */
    public void walkToSpot(Vector3 dest, Direction dir)
    {
        switch (dir)
        {
            case Direction.North:
                toRunNorth();
                break;
            case Direction.East:
                toRunEast();
                break;
            case Direction.South:
                toRunSouth();
                break;
            case Direction.West:
                toRunWest();
                break;
        }

        StartCoroutine(Move(transform, dest));
    }
}
