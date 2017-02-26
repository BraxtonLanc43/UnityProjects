using UnityEngine;
using System.Collections;

public class scr_Enemy : MonoBehaviour {

    private float leftSpeed = 3.0f;
    Rigidbody rigidbody;
    GameObject playerObject;
    private bool isDead;
    Animator animator;
    private AudioSource audioSource;
    public AudioClip deathSound;
    public bool keepWalking;

    // Use this for initialization
    void Start () {
        keepWalking = true;
        isDead = false;
        rigidbody = GetComponent<Rigidbody>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = deathSound;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!keepWalking)
        {
            animator.Stop();
            return;
        }
            

        //MoveLeft(); //shouldn't need to move left cuz player and screen are moving right
        animator.Play("gorilla_walk");

        //if off screen to left, destroy
        if (isOffScreenLeft())
        {
            isDead = true;
            DestroyObject(this.gameObject);
        }

    }


    public void playCaughtSound()
    {
        audioSource.Play();
    }
    

    /*
    * @param none
    * @returns boolean
    * Desc: Checks to see if it is off of the screen to the right. If it is, returns true, else false.
    */
    private bool isOffScreenLeft()
    {
        //get world point
        Vector3 enemy_WorldPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 player_WorldPosition = Camera.main.WorldToScreenPoint(playerObject.transform.position);

        if (enemy_WorldPosition.x < (player_WorldPosition.x - (Screen.width / 4)))
        {
            return true;
        }

        return false;
    }


    /*
    * @param none
    * @returns none
    * Desc: Moves gameobject left at a constant speed
    */
    void MoveLeft()
    {
        Debug.Log("Moving Right");
        //  rigidbody.velocity = transform.forward * speed;
        rigidbody.velocity = -(transform.forward * leftSpeed);
    }

}
