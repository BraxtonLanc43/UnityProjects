using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public GameObject[] zombiePrefabs;
    private GameObject[] spawnPoints;
    private float startTime;
    private GameObject player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        startTime = Time.time;
        spawnPoints = GameObject.FindGameObjectsWithTag("EnemySpawn");
    }

    // Update is called once per frame
    void Update()
    {
        //Don't spawn if player is dead
        if (!player.GetComponent<Player>().isDead)
            SpawnEnemy();
        else
            StopZoms();
    }


    /*
* @param 
* @returns  
* @desc : Halts all zombies
* @status : Testing
*/
    private void StopZoms()
    {
        GameObject[] zoms = GameObject.FindGameObjectsWithTag("Enemy");
        
        for (int i = 0; i < zoms.Length; i++)
        {
            zoms[i].GetComponent<Enemy>().toIdle();
            zoms[i].GetComponent<Enemy>().halt = true;
        }
    }


    /*
* @param 
* @returns  
* @desc : Spawns enemy at one of the spawn points
* @status : Completed but may need tweaking on spawn logic
*/
    private void SpawnEnemy()
    {
        //Logic for if should spawn enemy. For now, every 1.5 second(s) potentially spawn one
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (Time.time - startTime > 1.5)
        {
            startTime = Time.time;

            //Random enemy prefab chosen
            int indexZom = (int)(Mathf.Ceil(Random.Range(0, 3)));
            GameObject enemyPrefab = zombiePrefabs[indexZom];

            //Random(?) spawn point chosen
            int indexSpawn = (int)(Mathf.Ceil(Random.Range(0, spawnPoints.Length)));
            GameObject spawnPointSelected = spawnPoints[indexSpawn];

            //Spawn below map so can rise up
            Vector3 spawnPoint = new Vector3(spawnPointSelected.transform.position.x, -3.0f, spawnPointSelected.transform.position.z);
            GameObject enemySpawning = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
    }
}


