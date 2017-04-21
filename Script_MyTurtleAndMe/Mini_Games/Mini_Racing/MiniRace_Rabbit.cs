using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRace_Rabbit : MonoBehaviour {

    public Animator animator;
    public GameObject dirtPath;
    public GameObject finishLine;
    public float walkSpeed;
    public bool reachFinish = false;
    public bool turtleFinished = false;
    public GameObject turtle;
    public MiniRace_Turtle src_Turte;
    public GameObject manager;
    public MiniRace_Manager src_Manager;

    // Use this for initialization
    void Start () {
        walkSpeed = 0.04f;
        animator = GetComponent<Animator>();
        spawnPosition();
        faceEast();
        src_Turte = turtle.GetComponent<MiniRace_Turtle>();
        src_Manager = manager.GetComponent<MiniRace_Manager>();
    }
	
	// Update is called once per frame
	void Update () {
	
        if(turtleFinished || reachFinish)
        {
            //race over
            faceEast();
        }
        else
        {
            if(transform.position.x >= finishLine.transform.position.x)
            {
                src_Turte.gameOver = true;
                reachFinish = true;
                Debug.Log("Rabbit finished");
                src_Manager.winner("Rabbit");
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


    public void run()
    {
        toRunEast();
        StartCoroutine(Move(transform, new Vector3(finishLine.transform.position.x, transform.position.y, transform.position.z)));
    }


    /*
    * @param Transform
    * @returns null/0
    * @desc Moves NPC via Lerp/Time.deltaTime
    * @status Working
    */
    public IEnumerator Move(Transform entity, Vector3 endPos)
    {
        Vector3 startPos = entity.position;
        float t = 0;

        while (t < 1f && (!turtleFinished))
        {
            t += Time.deltaTime * walkSpeed;
            entity.position = Vector3.Lerp(startPos, endPos, t);
            yield return null;
        }

        reachFinish = true;

        yield return 0;
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
}
