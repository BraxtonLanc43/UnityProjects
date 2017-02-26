using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
* NOTES:
*   -Jumping distance could use some work
*/


public class scr_PlayerMotor : MonoBehaviour
{
    public GameObject castle_End;
    private bool finished;
    public GameObject thisPlayer;
    private CharacterController controller;
    private float inputDirection;
    bool toRemoveHeart;
    private Vector3 moveVector;
    private float xAccel = 7.0f;
    Animator animator;
    bool walkOk;
    private float jumpForce = 185.5f;
    private float verticalVelocity;
    private float gravity = 10.0f;
    private bool grounded;
    public bool facingForward;
    private float lastGroundedDirection;
    public bool isDead = false;
    public bool levelRestarted;
    //public GameObject rock;
    public scr_Rock rock;
    private float rockSpeed = 3.0f;
    private GameObject[] lives;
    private int livesLeft;
    private int live_FromPlayerPrefs;
    GameObject[] completeStuff;

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        completeStuff = GameObject.FindGameObjectsWithTag("Complete");
        for (int i = 0; i < completeStuff.Length; i++)
        {
            completeStuff[i].SetActive(false);
        }
        finished = false;
        toRemoveHeart = true;
        facingForward = true;
        grounded = true;
        walkOk = true;
        animator = gameObject.GetComponent<Animator>();
        controller = GetComponent<CharacterController>();

        lives = GameObject.FindGameObjectsWithTag("Life");
        live_FromPlayerPrefs = PlayerPrefs.GetInt("Lives", 3);

        //Debug.Log("Prefs: " + PlayerPrefs.GetInt("Lives"));

        if (live_FromPlayerPrefs == 2)
        {
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            //lives[2].SetActive(false);
        }
        else if (live_FromPlayerPrefs == 1)
        {
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            //lives[2].SetActive(false);
            //lives[0].SetActive(false);
        }
        else if (live_FromPlayerPrefs == 0)
        {
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            //lives[0].SetActive(false);
        }
        // Debug.Log(live_FromPlayerPrefs);

    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x >= castle_End.transform.position.x)
        {
            finished = true;
            EndLevel();
        }
        
        if (finished == true)
            return;

        if (isDead == true)
            return;

        livesLeft = 0;
        lives = GameObject.FindGameObjectsWithTag("Life");
        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[i].activeInHierarchy == true)
            {
                livesLeft++;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // ThrowRock();
            rock.ProjectileRock(facingForward);
        }



        grounded = IsControllerGrounded();
        inputDirection = Input.GetAxis("Horizontal");

        //move this to when IsGrounded()
        if (grounded)
        {
            walkOk = true;
            verticalVelocity = 0;

            //get x
            lastGroundedDirection = Input.GetAxisRaw("Horizontal");
            FlipPlayer(lastGroundedDirection);
        }
        else
        {
            walkOk = false;
        }

        moveVector = Vector3.zero;
        moveVector.x = lastGroundedDirection * xAccel;

        //jump controls
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Debug.Log("Grounded?: " + grounded);
            verticalVelocity = jumpForce;
            animator.Play("Archer1_Jump");
            moveVector.y = verticalVelocity;
            moveVector.x = inputDirection;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        //move character
        if (moveVector.x != 0 && walkOk == true)     //Toggle 'walkOk' when jumping or shooting
        {
            //If we're moving, then walk
            animator.Play("Archer1_Walk");
        }
        else  //else no animation
        {
            animator.Play("Archer1_Idle");
        }

        controller.Move(moveVector * Time.deltaTime);


    }

    private void EndLevel()
    {        
        Debug.Log("completeStuff.length: " + completeStuff.Length);
        for (int i = 0; i < completeStuff.Length; i++)
        {
            completeStuff[i].SetActive(true);
        }
        Debug.Log("Congrats you won");
    }

    private void ThrowRock()
    {
        GameObject rockHere = Instantiate(rock, this.transform.position, this.transform.rotation) as GameObject;
        // Debug.Log("Rock is at: " + rockHere.transform.position);
        rockHere.transform.position += Time.deltaTime * rockSpeed * transform.forward;
        // Debug.Log("Rock should have moved: " + rockHere.transform.position);
    }

    private static bool IsNullOrEmpty(GameObject[] array)
    {
        return (array == null || array.Length == 0);
    }

    private void FlipPlayer(float rawAxis)
    {
        if (rawAxis == -1 && facingForward == true)
        {
            //flip left
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            facingForward = false;
        }
        else if (rawAxis == 1 && facingForward == false)
        {
            //flip right
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
            facingForward = true;
        }
    }

    private bool IsControllerGrounded()
    {
        bool isGrounded = false;
        Vector3 leftRayStart;
        Vector3 rightRayStart;
        leftRayStart = controller.bounds.center;
        rightRayStart = controller.bounds.center;

        leftRayStart.x -= controller.bounds.extents.x;
        rightRayStart.x += controller.bounds.extents.x;

        Debug.DrawRay(leftRayStart, Vector3.down, Color.red);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.green);

        if (Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f))
        {
            isGrounded = true;
        }

        if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f))
        {
            isGrounded = true;
        }
        //Debug.Log("isItGrounded: " + isGrounded);

        return isGrounded;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log("Hit this: " + hit.gameObject.tag);

        //collectables
        switch (hit.gameObject.tag)
        {
            case "Enemy":
                if (isDead == false)
                {
                    PauseEverything();
                  //  Debug.Log("Hit enemy");
                }
                break;
            case "Water":
                if(isDead == false)
                {
                    PauseEverything();
                  //  Debug.Log("Hit water");
                }
                break;
            //case "Platform":
            //    MoveTest(hit);
            //    break;
            default:
                break;
        }

    }

    //not currently in use
    private void MoveTest(ControllerColliderHit hit)
    {
        Debug.Log("Hit tag: : " + hit.transform.tag);
        if (hit.gameObject.tag == "Platform")
        {
            Debug.Log("OnCollisionsStay");
            transform.parent = hit.transform;
        }
        else {
            transform.parent = null;
        }
    }

    //not currently in use
    private void MoveWithPlatform(ControllerColliderHit platform)
    {
        Debug.Log("Should be moving with platform");
        scr_Platform plat = platform.gameObject.GetComponent<scr_Platform>();
        if (plat.movingLeft)
        {
            //move left
            MoveLeft(true, plat.speed);
        }
        else if (!plat.movingLeft)
        {
            //move right
            MoveLeft(false, plat.speed);
        }
    }

    //not currently in use
    private void MoveLeft(bool moveLeft, float speed)
    {

        //float temp_InputDirection = Input.GetAxis("Horizontal");        
        //Vector3 temp_MoveVector = Vector3.zero;
        //temp_MoveVector.x = temp_InputDirection * speed;
        //controller.Move(temp_MoveVector * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            PauseEverything();
        }
        if (collision.collider.tag == "Water")
        {
            PauseEverything();
        }
        if(collision.collider.tag == "Platform")
        {
            transform.parent = collision.transform;
        }
    }

    public void PauseEverything()
    {
        isDead = true;
        animator.Play("Archer1_Death");

      //  Debug.Log("Lives here: " + livesLeft);
      //  Debug.Log("toRemove: " + toRemoveHeart);

        //lose life
        if (toRemoveHeart && livesLeft == 3)
        {
            toRemoveHeart = false;
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            // lives[2].SetActive(false);
            livesLeft--;
        }
        else if (toRemoveHeart && livesLeft == 2)
        {
            toRemoveHeart = false;
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            //  lives[0].SetActive(false);
            livesLeft--;
        }
        else if (toRemoveHeart && livesLeft == 1)
        {
           // Debug.Log("Lives length: " + lives.Length);
            toRemoveHeart = false;
            GetHeart_FurthestRightActive(lives, livesLeft).SetActive(false);
            //lives[0].SetActive(false);
            livesLeft--;
        }

       // Debug.Log("lives left check: " + livesLeft);

        if (livesLeft == 0)
        {
            //we lost, reset lives for player prefs
            PlayerPrefs.SetInt("Lives", 3);

            //Present menu screen
            ToMainMenu();
        }
        else
        {
            //Restart scene 
            PlayerPrefs.SetInt("Lives", livesLeft);
            levelRestarted = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private GameObject GetHeart_FurthestRightActive(GameObject[] hearts, int livesRemaining)
    {
        List<GameObject> temp_Actives = new List<GameObject>();
        GameObject toDelete;

        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[i].activeInHierarchy == true)
                temp_Actives.Add(lives[i]);
        }

        if (temp_Actives.Count == 3)
        {
            toDelete = GetHighest(temp_Actives);
        }
        else if (temp_Actives.Count == 2)
        {
            toDelete = GetHighest_Two(temp_Actives);
        }
        else toDelete = temp_Actives[0];

        return toDelete;

    }

    private GameObject GetHighest(List<GameObject> hearts)
    {
        float first = hearts[0].transform.position.x;
        float second = hearts[1].transform.position.x;
        float third = hearts[2].transform.position.x;

        if (first > second && first > third)
            return hearts[0];
        else if (second > first && second > third)
            return hearts[1];
        else if (third > first && third > second)
            return hearts[2];

        return hearts[0];
    }

    private GameObject GetHighest_Two(List<GameObject> hearts)
    {
        float first = hearts[0].transform.position.x;
        float second = hearts[1].transform.position.x;
        if (first > second)
            return hearts[0];
        else if (second > first)
            return hearts[1];

        return hearts[0];
    }
}
