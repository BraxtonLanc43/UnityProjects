using UnityEngine;
using System.Collections;

/*
*   NOTES:
        -Rocks firing all over the place?
        -Rock not recognizing collision with enemy character
        -See toggle comment below
*/

public class scr_Rock : MonoBehaviour {

    private GameObject[] otherRocks;
    public GameObject player;
    private GameObject playerObject;
    private int rockSpeed = 2;
    public GameObject selfRock;
    Rigidbody rigidbody;
    private bool canShoot;
    float startTime;

    // Use this for initialization
    void Start ()
    {
        rigidbody = GetComponent<Rigidbody>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
	}

    void Awake()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update ()
    {
        checkStartTime();
	}

    private void checkStartTime()
    {
       // Debug.Log(startTime);
        if(Time.time - startTime > 3)
        {
            DestroyObject(this.gameObject);
        }
    }

    public void ProjectileRock(bool facingForward)
    {

        otherRocks = GameObject.FindGameObjectsWithTag("Rock");
        if(IsNullOrEmpty(otherRocks) == true)
        {
            canShoot = true;
        }
        else if(otherRocks.Length >= 3)
        {
            canShoot = false;
        }
        
        if (canShoot == false)
            return;

        playerObject = GameObject.FindGameObjectWithTag("Player");
        
        if (facingForward)
        {
            GameObject rockHere = Instantiate(selfRock, playerObject.transform.position + new Vector3(1, 0, 0), playerObject.transform.rotation) as GameObject;
            rigidbody = rockHere.GetComponent<Rigidbody>();
            rigidbody.AddForce(transform.right * 2, ForceMode.Impulse);
            //rigidbody.AddForce(transform.right * rockSpeed, ForceMode.Impulse);
        }
        else if (!facingForward)
        {
            GameObject rockHere = Instantiate(selfRock, playerObject.transform.position + new Vector3(-1, 0, 0), playerObject.transform.rotation) as GameObject;
            rigidbody = rockHere.GetComponent<Rigidbody>();
            rigidbody.AddForce(-(transform.right * 2), ForceMode.Impulse);
            //rigidbody.AddForce(-transform.right * rockSpeed, ForceMode.Impulse);
        }
    }

    private static bool IsNullOrEmpty(GameObject[] array)
    {
        return (array == null || array.Length == 0);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       // Debug.Log("Hit HERE 4" + hit.gameObject.tag);
        DestroyObject(this.gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {

        if(collision.gameObject.tag == "Enemy")
        {
            DestroyObject(collision.gameObject);
           // Debug.Log("Hit Here 3: " + collision.gameObject.tag);
        }        
    }
}
