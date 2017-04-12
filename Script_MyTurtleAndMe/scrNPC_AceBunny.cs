using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrNPC_AceBunny : MonoBehaviour, scrI_NPC
{
    //Consistent properties
    public Animator animator;
    public int count_Engaged;
    public string commentary_First = "Hey newbie! My rabbit and I have been together since I was a kid. There's no better friends than us!";
    public string commentary_After = "Wow you guys are a good pair. Stick together and you'll become the best of friends!";

    //Class variables
    public GameObject player_GO;
    public scrPlayer player;
    public GameObject turtle_GO;
    public scrTurtle turtle;
    public GameObject rabbit_GO;
    public scrRabbit rabbit;
    public float playerX;
    public float playerY;
    public float sightDistance = 10.0f;
    Direction currentDir;
    public bool isMoving = false;
    public float t;
    public float walkSpeed = 1.0f;
    public bool hasAlreadyWalkedToDestination = false;

    public void Start()
    {
        //Consistent properties
        animator = GetComponent<Animator>();
        count_Engaged = 0;

        //other
        player_GO = GameObject.FindGameObjectWithTag("Player");
        player = player_GO.GetComponent<scrPlayer>();
        turtle_GO = GameObject.FindGameObjectWithTag("Turtle");
        turtle = turtle_GO.GetComponent<scrTurtle>();
        rabbit_GO = GameObject.FindGameObjectWithTag("Rabbit");
        rabbit = rabbit_GO.GetComponent<scrRabbit>();
        playerX = player_GO.gameObject.transform.position.x;
        playerY = player_GO.gameObject.transform.position.y;
        currentDir = Direction.West;
        faceWest();
    }

    public void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;

        //Update player coords
        updatePlayerCoords();

        bool isSighted = lineOfSight_Player(currentDir);
        if (isSighted && !isMoving && !hasAlreadyWalkedToDestination)
        {
            spotted();
            walkToPlayer(currentDir);
        }
            
        
    }
    

    /*
    * @param Vector3 destination this transform is going to
    * @returns Vector3 destination for companion guy
    * @desc Calculates the destination for companion
    * @status Working
    */
    public Vector3 getCompanionDestination(Vector3 thisGuysDest)
    {
        Vector3 rab_Dest = new Vector3(0, 0, 0); // default only

        //is rabbit currently north, south, east, or west of this character?
        Direction rabbitLocation = Direction.North; //default only
        if(rabbit_GO.transform.position.x - transform.position.x > 0.5)
        {
            rabbitLocation = Direction.East;
        }
        else if (rabbit_GO.transform.position.x - transform.position.x < -0.5)
        {
            rabbitLocation = Direction.West;
        }
        else if (rabbit_GO.transform.position.y - transform.position.y > 0.5)
        {
            rabbitLocation = Direction.North;
        }
        else if (rabbit_GO.transform.position.y - transform.position.y < -0.5)
        {
            rabbitLocation = Direction.South;
        }

        switch (rabbitLocation)
        {
            case Direction.North:
                rab_Dest = new Vector3(thisGuysDest.x, thisGuysDest.y + 1, thisGuysDest.z);
                break;
            case Direction.East:
                rab_Dest = new Vector3(thisGuysDest.x + 1, thisGuysDest.y, thisGuysDest.z);
                break;
            case Direction.South:
                rab_Dest = new Vector3(thisGuysDest.x, thisGuysDest.y - 1, thisGuysDest.z);
                break;
            case Direction.West:
                rab_Dest = new Vector3(thisGuysDest.x - 1, thisGuysDest.y, thisGuysDest.z);
                break;
        }

        return rab_Dest;
    }


    /*
    * @param void
    * @returns void
    * @desc Forces player and Turtle to look at NPC
    * @status Working
    */
    public void forcePlayerLookAt()
    {
        switch (currentDir)
        {
            case Direction.North:
                player.toIdleSouth();
                turtle.toIdleSouth();
                break;
            case Direction.East:
                player.toIdleWest();
                turtle.toIdleWest();
                break;
            case Direction.South:
                player.toIdleNorth();
                turtle.toIdleNorth();
                break;
            case Direction.West:
                player.toIdleEast();
                turtle.toIdleEast();
                break;
        }
    }

    /*
    * @param void
    * @returns void
    * @desc Updates player coordinates
    * @status Working
    */
    public void updatePlayerCoords()
    {
        playerX = player_GO.gameObject.transform.position.x;
        playerY = player_GO.gameObject.transform.position.y;
    }


    /*
    * Begin scrI_NPC implementation
    */

    /*
    * @param void
    * @returns void
    * @desc Logic for controlling commentary for this NPC
    * @status Not Complete
    * @interface scrI_NPC
    */
    public void engageCommentary()
    {
        //Play commentary based on how many times they've been engaged
        //increment # of times we've engaed this NPC        
        count_Engaged++;

        //based on whether it's the first time engaging or not the first, render dialog
        if (count_Engaged == 1)
        {
            //Play original commentary
            playFirstCommentary();
        }
        else if (count_Engaged > 1)
        {
            //Play other commentary
            playAdditionalCommentary();
        }
    }

    /*
    * @param void
    * @returns void
    * @desc Flips the sprite to face east
    * @status Working
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
    * @status Working
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
    * @status Working
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
    * @status Working
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

    /*
    * @param void
    * @returns void
    * @desc 
    * @status Not Complete
    * @interface scrI_NPC
    */
    public void playAdditionalCommentary()
    {
        //Play additional commentary

    }

    /*
    * @param void
    * @returns void
    * @desc 
    * @status Not Complete
    * @interface scrI_NPC
    */
    public void playFirstCommentary()
    {
        //Play first commentary

    }

    /*
     * @author Braxton Lancial
     * @param Direction direction the engager (callee) is facing
     * @return void
     * @desc Forces the NPC to look at the engager (likely the player)
     * @interface scrI_NPC
     * @status Working
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
     * @author Braxton Lancial
     * @param Direction current direction
     * @return bool if they've spotted the player or not
     * @desc Checks straight ahead for whether the player has been spotted by the NPC
     * @interface scrI_NPC
     * @status Working
     */
    public bool lineOfSight_Player(Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.North:
                if ((Mathf.Abs(playerX - gameObject.transform.position.x) < 0.5f) && (playerY - gameObject.transform.position.y) < sightDistance && (playerY - gameObject.transform.position.y) > 0)
                {
                    return true;
                }
                break;
            case Direction.South:
                if ((Math.Abs(playerX - gameObject.transform.position.x) < 0.5f) && (gameObject.transform.position.y - playerY) < sightDistance && (gameObject.transform.position.y - playerY) > 0)
                {
                    return true;
                }
                break;
            case Direction.East:
                if ((Mathf.Abs(playerY - gameObject.transform.position.y) < 0.5f) && (playerX - gameObject.transform.position.x) < sightDistance &&(playerX - gameObject.transform.position.x) > 0)
                {
                    return true;
                }
                break;
            case Direction.West:
                if ((Mathf.Abs(playerY - gameObject.transform.position.y) < 0.5f) && (gameObject.transform.position.x - playerX) < sightDistance && (gameObject.transform.position.x - playerX) > 0)
                {
                    return true;
                }
                break;
        }

        return false;
    }

    /*
     * @author Braxton Lancial
     * @param void
     * @return void
     * @desc Puts an "!" above sprite's head (similar to pokemon)
     * @interface scrI_NPC
     * @status Not Complete
     */
    public void spotted()
    {
        //Flash an exclamation point above sprite's head or something
        player.lockedIn = true;
    }

    /*
     * @author Braxton Lancial
     * @param Direction current direction
     * @return void
     * @desc Walks to the player
     * @interface scrI_NPC
     * @status Tested for West (partially assume other directions work too)
     */
    public void walkToPlayer(Direction currentDirection)
    {
        //find end position
        Vector3 destination = new Vector3(0, 0, 0);
        Vector3 originalSpot = gameObject.transform.position;

        //lerp 1 spot at a time until make it to end position
        //play correct animation based on direction
        switch (currentDirection)
        {
            case Direction.North:
                toRunNorth();
                destination = new Vector3(Mathf.Round(playerX), Mathf.Round(playerY) - 1, gameObject.transform.position.z);
                StartCoroutine(Move(transform, destination));
                rabbit.walkToSpot(getCompanionDestination(destination), currentDirection);
                break;
            case Direction.South:
                toRunSouth();
                destination = new Vector3(Mathf.Round(playerX), Mathf.Round(playerY) + 1, gameObject.transform.position.z);
                StartCoroutine(Move(transform, destination));
                rabbit.walkToSpot(getCompanionDestination(destination), currentDirection);
                break;
            case Direction.East:
                toRunEast();
                destination = new Vector3(Mathf.Round(playerX) -1, Mathf.Round(playerY), gameObject.transform.position.z);
                StartCoroutine(Move(transform, destination));
                rabbit.walkToSpot(getCompanionDestination(destination), currentDirection);
                break;
            case Direction.West:
                toRunWest();
                destination = new Vector3(Mathf.Round(playerX) + 1, Mathf.Round(playerY), gameObject.transform.position.z);
                StartCoroutine(Move(transform, destination));
                rabbit.walkToSpot(getCompanionDestination(destination), currentDirection);
                break;
        }

        

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
        forcePlayerLookAt();
        yield return 0;
    }


    /*
     * @author Braxton Lancial
     * @param void
     * @return void
     * @desc Plays animation for running north
     * @interface scrI_NPC
     * @status Working
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
     * @status Working
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
     * @status Working
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

    public void walkToSpot(Vector3 dest, Direction dir)
    {
        throw new NotImplementedException();
    }
    /*
*End scrI_NPC implementation
*/
}


