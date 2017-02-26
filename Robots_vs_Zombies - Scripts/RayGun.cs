using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayGun : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    /*
* @param Collision collision
* @returns null
* @desc : Called when gameobject collides into another collider
* @status : Untested
*/
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Beam collided: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Beam")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

}
