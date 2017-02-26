using UnityEngine;
using System.Collections;

public class scr_Poo : MonoBehaviour {

    Rigidbody rigidbody;
    public GameObject selfPoo;
    private GameObject playerObject;
    private int speed = 50;
    private bool isDead;

    // Use this for initialization
    void Start ()
    {
        isDead = false;
        rigidbody = GetComponent<Rigidbody>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update ()
    {
       // Debug.Log("Width: " + Screen.width);
        if (isDead == true)
            return;
        
        MoveRight(); 
        
        if(isOffScreen())
        {
            isDead = true;
            DestroyObject(this.gameObject);
        }
        
    }

    /*
   * @param Collision collision
   * @returns null
   * Desc: Called when gameobject collides into another collider
   */
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Banana" || collision.gameObject.tag == "Poo" || collision.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if(collision.gameObject.tag == "Gorilla")
        {
            DestroyObject(gameObject);
            DestroyObject(collision.gameObject);
        }
    }
    

    /*
    * @param none
    * @returns boolean
    * Desc: Checks to see if it is off of the screen to the right. If it is, returns true, else false.
    */
    private bool isOffScreen()
    {
        //get world point
        Vector3 poo_WorldPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 player_WorldPosition = Camera.main.WorldToScreenPoint(playerObject.transform.position);

        if(poo_WorldPosition.x > (player_WorldPosition.x + Screen.width))
        {
            return true;
        }

        return false;

        //if world point > player.x + width, destroy

    }


    /*
    * @param none
    * @returns null
    * Desc: Moves gameobject right at a constant speed using rigidbody force
    */
    private void MoveRight()
    {
        rigidbody.velocity = transform.forward * speed;
    }
}
