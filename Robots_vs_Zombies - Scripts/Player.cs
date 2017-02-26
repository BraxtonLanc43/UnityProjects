using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Class variables
    public GameObject myCanvas;
    private AudioSource audioSource;
    public AudioClip hitAudio;
    public AudioClip deathAudio;
    private CharacterController controller;
    private Animator animator;
    float moveSpeed = 3.0f;
    public GameObject raygun_Obj;
    private RayGun rayGun_Scr;
    public GameObject beamPrefab;
    public GameObject gunBarrel;
    private bool canShoot;
    private float startTime;
    private bool lookingAtEnemy;
    private GameObject enemyToLookAt;
    private int hp;
    public bool isDead;
    private float timeDelay;
    private bool hasHit;
    private int temp;
    private float whenDied;
    private bool hasToggled;

    //On awake
    void Awake()
    {
        startTime = Time.time;
    }

    // Use this for initialization
    void Start()
    {
        hasToggled = false;
        audioSource = GetComponent<AudioSource>();
        hasHit = false;
        isDead = false;
        hp = 5;
        lookingAtEnemy = false;
        canShoot = true;
        temp = 1;
        //Cache components on Start()
        rayGun_Scr = raygun_Obj.GetComponent<RayGun>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        animator.SetBool("toIdle", true);
    }

    // Update is called once per frame
    void Update()
    {
        if(whenDied != 0 && (Time.time - whenDied) > 1.0f && !hasToggled)
        {
            myCanvas.GetComponent<DeathMenuManager>().toggleEndMenu();
            hasToggled = true;
        }

        if (isDead)
            return;
        
        //Cache some data
        GameObject[] enemiesActive = getEnemies();
        GameObject[] moveSpots = getMoveSpots();
        GameObject bestSpot = getBestSpot(moveSpots, enemiesActive);

        //Get how far away on 'x' and 'z' values we are from our "bestSpot" position
        float x_AwayFromBest = Mathf.Abs(bestSpot.transform.position.x - transform.position.x);
        float z_AwayFromBest = Mathf.Abs(bestSpot.transform.position.z - transform.position.z);

        //Force 'transform.position.y' to remain consistent
        forceY_Stay();

        //if we're not already in the best position...then move there
        if (x_AwayFromBest > .5f || z_AwayFromBest > .5f)    
        {
            //move to that spot
            moveToSpot(bestSpot.transform.position);   
        }
        else
        {
            //If we are currently running, but don't need to be, then stop
            if (animator.GetBool("toRun") == true)
                toIdle();
        }

        //Handling the beam shooting
        bool didWeShoot = ShootBeamHandler(enemiesActive);

        //Look at enemy if we should be
        if (lookingAtEnemy)
        {
            gameObject.transform.LookAt(enemyToLookAt.gameObject.transform);
        }

        if(timeDelay != 0.0f && (Time.time - timeDelay) > 0.5f)
        {
            animator.SetBool("toHit", false);
        }
        
    }


    /*
 * @param GameObject closestEnemy, GameObject enemiesActive
 * @returns bool 
 * @desc : Logic for whether should shoot beam or not
 * @status : Working
 */
    private bool ShootBeamHandler(GameObject[] enemiesActive)
    {
        bool didShoot = false;

        //Logic for shooting enemies
        if (canShoot && enemiesActive != null && enemiesActive.Length > 0)
        {
            //Get the nearest enemy
            GameObject en = getClosestEnemy(enemiesActive);

            //Don't want to start shooting an enemy that is not above ground yet
            if (en.transform.position.y > -.5f)
            {
                //every .5 seconds, we'll potentially shoot, so not shooting ridiculous amount of beams every frame
                if (Time.time - startTime > .5)
                {
                    didShoot = true;
                    shootBeam(en);
                    startTime = Time.time;
                }
            }
            else
                lookingAtEnemy = false;
        }
        else
            lookingAtEnemy = false;
        return didShoot;
    }



    /*
 * @param Collision collision
 * @returns null
 * @desc : Called when gameobject collides into another collider
 * @status : Untested
 */
    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Beam collided: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Beam")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if (collision.gameObject.tag == "Zombie" || collision.gameObject.tag == "Enemy")
        {
            //DEATH
            //Debug.Log("Player collided with enemy");
        }
        
    }


    /*
     * @param none
     * @returns null
     * @desc Fires a beam shot at an enemy that the player is facing 
     * @status Working
     */
    public void GotHit()
    {
       // Debug.Log("Got hit. HP= " + hp);
        hp--;
        if(hp == 0)
        {
            DeadAndEnd();
        }
        else
        {
            //got hit but did not die, do something
            animator.SetBool("toHit", true);
            audioSource.clip = hitAudio;
            audioSource.Play();
            timeDelay = Time.time;
        }

    }


    /*
    * @param none
    * @returns GameObject[]
    * @desc Gets all existing enemies
    * @status Not yet implemented
    */
    private void DeadAndEnd()
    {
        isDead = true;
        animator.SetBool("toDeath", true);
        audioSource.clip = deathAudio;
        audioSource.Play();
        whenDied = Time.time;
    }



    /*
     * @param none
     * @returns null
     * @desc Fires a beam shot at an enemy that the player is facing 
     * @status Working
     */
    public void shootBeam(GameObject enemyToShoot)
    {
        lookingAtEnemy = true;
        enemyToLookAt = enemyToShoot;
        gameObject.transform.LookAt(enemyToShoot.gameObject.transform);

        GameObject beam = Instantiate(beamPrefab, gunBarrel.gameObject.transform.position, Quaternion.identity);

    }


    /*
    * @param none
    * @returns null
    * @desc Cues transition from Idle to GunRun animation
    * @status Working
    */
    private void toGunRun()
    {
        animator.SetBool("toRun", true);
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
    * @param none
    * @returns null
    * @desc Cues transition from GunRun animation to idle
    * @status Working
    */
    private void toIdle()
    {
        animator.SetBool("toRun", false);
    }


    /*
    * @param none
    * @returns GameObject[]
    * @desc Gets all potential spots to move
    * @status Working
    */
    private GameObject[] getMoveSpots()
    {
        GameObject[] moveSpots = GameObject.FindGameObjectsWithTag("MoveSpot");

        return moveSpots;
    }


    /*
    * @param none
    * @returns GameObject[]
    * @desc Gets all existing enemies
    * @status Working
    */
    private GameObject[] getEnemies()
    {
        List<GameObject> toReturn = new List<GameObject>();
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies != null && enemies.Length > 0)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (!enemies[i].GetComponent<Enemy>().isDead)
                {
                    toReturn.Add(enemies[i]);
                }
            }
        }

        enemies = (toReturn.ToArray());

        return enemies;
    }


    /*
    * @param none
    * @returns GameObject enemuy
    * @desc Gets enemy closest to the player
    * @status Working
    */
    private GameObject getClosestEnemy(GameObject[] enemies)
    {
        GameObject closestEnemy = enemies[0];
        float shortestDistance = 100.0f;

        for (int i = 0; i < enemies.Length; i++)
        {
            float currentDist = Vector3.Distance(gameObject.transform.position, enemies[i].transform.position);
            if(currentDist < shortestDistance && enemies[i].transform.position.y > -.5f) //Don't want to start shooting enemy that isn't above ground yet
            {
                shortestDistance = currentDist;
                closestEnemy = enemies[i];
            }
        }

   //     Debug.Log("Shortest Distance: " + shortestDistance);

        return closestEnemy;
    }



    /*
    * @param GameObject[] moveSpots
    * @returns GameObject moveSpot 
    * @desc Gets the best moveSpot currently available, and returns it
    * @status Working
    */
    private GameObject getBestSpot(GameObject[] moveSpots, GameObject[] enemies)
    {
        float fartheststDistance = 0.0f;
        GameObject bestSpot = moveSpots[0];

        if (enemies != null && enemies.Length > 0)
        {
            for (int i = 0; i < moveSpots.Length; i++)
            {
                float sum_DistanceHere = 0.0f;
                for (int j = 0; j < enemies.Length; j++)    //go through each enemy, summing up total distance from them
                {
                    float currentDistance = Vector3.Distance(moveSpots[i].transform.position, enemies[j].transform.position);
                    sum_DistanceHere += currentDistance;
                }

                //after getting total distance for each moveSpot, see if it's better
                if (sum_DistanceHere > fartheststDistance)
                {
                    fartheststDistance = sum_DistanceHere;
                    bestSpot = moveSpots[i];
                }
            }
        }
        else
        {
            bestSpot = gameObject;  //if no enemies, don't move, return current gameObject so will stay in current position
        }

        return bestSpot;
    }

    /*
    * @param ControllerColliderHit hit
    * @returns null
    * @desc Standard Unity method for colliding
    * @status UNITY
    */
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Hit something: " + hit.gameObject.tag);
        
    }

    /*
    * @param Vector3 spotToMoveTo
    * @returns null
    * @desc Moves the player to a predetermined spot (also play with rotation?)
    * @status Working
    */
    private void moveToSpot(Vector3 spotToMoveTo)
    {
        Vector3 spotToMoveTo_AdjustedY = new Vector3(spotToMoveTo.x, transform.position.y, spotToMoveTo.z);
        Vector3 moveDiff = spotToMoveTo_AdjustedY - transform.position;
        Vector3 moveDir = moveDiff.normalized * moveSpeed * Time.deltaTime;

        if(moveDir.sqrMagnitude < moveDiff.sqrMagnitude)
        {
            toGunRun();
            controller.Move(moveDir);

            if (transform.rotation != Quaternion.LookRotation(moveDir) && !lookingAtEnemy)
            {
                transform.rotation = Quaternion.LookRotation(moveDir);
            }            
        }
        else
        {
            toGunRun();
            controller.Move(moveDiff);
            if (transform.rotation != Quaternion.LookRotation(moveDiff))
            {
                transform.rotation = Quaternion.LookRotation(moveDiff);
            }
        }
    }
}
