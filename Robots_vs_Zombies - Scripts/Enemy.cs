using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hitPoints;
    private GameObject player;
    private Animator animator;
    private float moveSpeed = 2.0f;
    private CharacterController controller;
    private bool aboveGround;
    private float currentDist;
    private bool justAttacked;
    private float timeDelay;
    public bool isDead;
    private float whenDied;
    public AudioClip attackAudio;
    private AudioSource audioSource;
    public bool halt;
    private float playerDeathTime;
    public AudioClip defaultAudio;

	// Use this for initialization
	void Start () {
        halt = false;
        audioSource = GetComponent<AudioSource>();
        isDead = false;
        timeDelay = Time.time;
        justAttacked = false;
        aboveGround = false;
        hitPoints = 3;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {

        //If is dead...
        if (isDead && (Time.time - whenDied) < 0.5)
            return;
        else if (isDead && (Time.time - whenDied) > 0.5)
            DestroyObject(gameObject);
        else if (isDead)
        {
            halt = true;
            return;
        }

        //if player is dead
        if (player.GetComponent<Player>().isDead)
        {
            if (playerDeathTime == 0)
            {
                audioSource.clip = defaultAudio;
                playerDeathTime = Time.time;
            }
            else if((Time.time - playerDeathTime) > 1.5f)
                audioSource.Stop();

            toIdle();
            return;
        }

        if (!halt)
        {
            //Always be looking at the player
            gameObject.transform.LookAt(player.gameObject.transform);

            //Rise until >= 0.23 for 'y' value
            if (!aboveGround)
            {
                RiseUp();
            }
            else
            {
                forceY_Stay();
                //Walk towards the player
                WalkTowardsPlayer();
            }

            //if we're close to the player, attack animation, then wait 2 seconds before attacking again (just a delay)
            currentDist = getDistanceFromPlayer();
            if (currentDist < 3.4f && (Time.time - timeDelay) > 2)
                toAttack();

            //Death handler
            if (hitPoints == 0)
            {
                toDeath();
                whenDied = Time.time;
            }
        }
        else
        {
            audioSource.Stop();
        }
    }



    /*
   * @param none
   * @returns null
   * @desc Forces the Y value to stay consistent, so it doesn't sink or float during rotation changes.
   * @status Working
   */
    private void forceY_Stay()
    {
        transform.position = new Vector3(transform.position.x, 0.22f, transform.position.z);
    }

    /*
* @param 
* @returns float distanceHere
* @desc : Gets the distance from the gameObject to the player
* @status : Working
*/
    private float getDistanceFromPlayer()
    {
        float distanceHere = Vector3.Distance(gameObject.transform.position, player.transform.position);

        return distanceHere;     
    }



    /*
* @param 
* @returns  
* @desc : Rises up out of the ground (spawn)
* @status : Working
*/
    private void RiseUp()
    {
        Vector3 spotToMoveTo = new Vector3(gameObject.transform.position.x, 0.25f, gameObject.transform.position.z);
        Vector3 moveDiff = spotToMoveTo - transform.position;
        Vector3 moveDir = moveDiff.normalized * moveSpeed * Time.deltaTime;

        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            toIdle();
            controller.Move(moveDir);
        }
        else
        {
            toIdle();
            controller.Move(moveDiff);
        }


        if (gameObject.transform.position.y >= 0.23)
        {
            aboveGround = true;
        }
    }



    /*
* @param 
* @returns  
* @desc : Walks towards the player, also controls looking at the player
* @status : Working
*/
    private void WalkTowardsPlayer()
    {
        Vector3 spotToMoveTo_AdjustedY = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        Vector3 moveDiff = spotToMoveTo_AdjustedY - transform.position;
        Vector3 moveDir = moveDiff.normalized * moveSpeed * Time.deltaTime;
        
        if (moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            toWalk();
            controller.Move(moveDir);
        }
        else
        {
            toWalk();
            controller.Move(moveDiff);
        }
    }


    /*
    * @param none
    * @returns null
    * @desc Cues transition from GunRun animation to idle
    * @status Working
    */
    public void toIdle()
    {        
        animator.SetBool("toWalk", false);
        animator.SetBool("toDeath", false);
        animator.SetBool("toAttack", false);
        animator.SetBool("toIdle", true);
    }


    /*
    * @param none
    * @returns null
    * @desc Cues transition from GunRun animation to attack
    * @status Working
    */
    private void toAttack()
    {
        animator.SetBool("toWalk", false);
        animator.SetBool("toDeath", false);
        animator.SetBool("toAttack", true);
        animator.SetBool("toIdle", false);
        if (!isDead && !halt)
        {
            audioSource.clip = attackAudio;
            audioSource.Play();
        }
        //faking physics so don't have to go back and substitute Colliders with Rigidbodies and redo
        player.GetComponent<Player>().GotHit();

        justAttacked = true;
        timeDelay = Time.time;
    }


    /*
    * @param none
    * @returns null
    * @desc Cues transition from GunRun animation to death
    * @status Working
    */
    private void toDeath()
    {
        animator.SetBool("toWalk", false);
        animator.SetBool("toDeath", true);
        animator.SetBool("toAttack", false);
        animator.SetBool("toIdle", false);
        isDead = true;
    }


    /*
    * @param none
    * @returns null
    * @desc Cues transition from GunRun animation to walk
    * @status Working
    */
    private void toWalk()
    {
        animator.SetBool("toWalk", true);
        animator.SetBool("toDeath", false);
        animator.SetBool("toAttack", false);
        animator.SetBool("toIdle", false);
    }


    /*
* @param Collision collision
* @returns null
* @desc : Called when gameobject collides into another collider
* @status : Working
*/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Gun")
        {
            
        }
        
    }
}
