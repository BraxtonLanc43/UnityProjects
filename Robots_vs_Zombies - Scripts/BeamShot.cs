using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamShot : MonoBehaviour {

    private Rigidbody rigidbody;
    private GameObject player;
    private float speed = 6.0f;
    private Vector3 direction;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        direction = getDirection();
	}
	
	// Update is called once per frame
	void Update () {

        //Need to get closest enemy
        GameObject[] enemies = getEnemies();
        GameObject closestEnemy = getClosestEnemy(enemies);

        //Need to make that a one-time calculation, and travel that direction until death
        travel(direction);
	}


    /*
    * @param GameObject[] moveSpots
    * @returns GameObject moveSpot 
    * @desc Gets the best moveSpot currently available, and returns it
    * @status Working
    */
    private GameObject getClosestEnemy(GameObject[] enemies)
    {
        float shortestDistance = 100.0f;
        GameObject closestEnemy = enemies[0];

        for (int i = 0; i < enemies.Length; i++)
        {
            float currentDistance = Vector3.Distance(gameObject.transform.position, enemies[i].transform.position);
            if(currentDistance < shortestDistance)
            {
                closestEnemy = enemies[i];
                shortestDistance = currentDistance;
            }
        }

        return closestEnemy;
    }


    /*
    * @param none
    * @returns GameObject[]
    * @desc Gets all existing enemies
    * @status Working
    */
    private GameObject[] getEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        return enemies;
    }



    /*
 * @param none
 * @returns null
 * @desc : Moves the beam in specified direction
 * @status : Working
 */
    private void travel(Vector3 dir)
    {
        rigidbody.velocity = dir * speed;
    }


    /*
* @param none
* @returns Vector3 direction
* @desc : Gets the direction the beam should travel, relative to where the player's facing
* @status : Working
*/
    private Vector3 getDirection()
    {
        //Not yet implemented
        return player.transform.forward;
    }


    /*
  * @param Collision collision
  * @returns null
  * @desc : Called when gameobject collides into another collider
  * @status : Working
  */
    private void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Beam collided: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Gun" || collision.gameObject.tag == "Tube" || collision.gameObject.tag == "GunBarrel")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
        else if (collision.gameObject.tag == "Zombie" || collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.hitPoints--;
           // Debug.Log("Enemy HP: " + enemy.hitPoints);
            DestroyObject(gameObject);            
        }
        else if(collision.gameObject.tag == "Wall")
        {
            DestroyObject(gameObject);
        }
    }
}
