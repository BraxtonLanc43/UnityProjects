using UnityEngine;
using System.Collections;

public class scr_SpawnManager : MonoBehaviour {

    private GameObject player;
    private bool hasSpawned;
    private Vector3 playerPosition;
    private float playerX;
    private float thisX;
    private float distance;
    public GameObject enemyPrefab;
    private GameObject enemySpawn;

	// Use this for initialization
	void Start ()
    {
        hasSpawned = false;
        player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
        playerPosition = player.transform.position;
        playerX = playerPosition.x;
        thisX = transform.position.x;
        distance = Mathf.Abs(playerX - thisX);

        //if(distance > 15)
        //{
        //    //don't spawn
        //    hasSpawned = false;
        //}
        //else 
        if (distance < 20)
        {            
            if(hasSpawned == false)
            {
                //spawn enemy
                enemySpawn = Instantiate(enemyPrefab);
                enemySpawn.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                //set hasSpawned to true
                hasSpawned = true;
            }
        }

	}
}
