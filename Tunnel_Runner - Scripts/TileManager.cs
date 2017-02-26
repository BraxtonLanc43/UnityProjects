using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
*   NOTES:
*
*/

public class TileManager : MonoBehaviour {
    
    private float spawnX;
    private float spawnY;

    public GameObject[] tilePrefabs;
    private float spawnZ = -23.2f;
    private float tileLength = 11.6f;
    private float safeZone = 50.0f;
    private int amountTilesOnScreen = 16;
    private int lastPrefabIndex = 0;

    private List<GameObject> activeTiles;

    private Transform player;

	// Use this for initialization
	void Start () {
        activeTiles = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnX = player.transform.position.x;
        spawnY = player.transform.position.y;

        for (int i = 0; i < amountTilesOnScreen; i++)
        {
            spawnTile();            
        }
    }
	
	// Update is called once per frame
	void Update () {
	if(player.transform.position.z - safeZone > (spawnZ - amountTilesOnScreen * tileLength))
        {
            spawnTile();
            deleteTile();
        }
	}

    private void spawnTile(int prefabIndex = -1)
    {                
        GameObject go;
        int randomIndex = 0;
  
        //go = Instantiate(tilePrefabs[indexHere]);
        if(activeTiles.Count <= 3)
        {
            randomIndex = 0;
        }
        else
        {
            randomIndex = randomPrefabIndex();
        }
         
        go = Instantiate(tilePrefabs[randomIndex]);
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(spawnX, spawnY, spawnZ);
        Debug.Log("spawnX= " + spawnX);
        Debug.Log("spawnY= " + spawnY);
        Debug.Log("spawnZ= " + spawnZ);
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    private void deleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }

    private int randomPrefabIndex()
    {
        if(tilePrefabs.Length <= 1)
        {
            return 0;
        }

        int randomIndex = lastPrefabIndex;

        while(randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;

        return randomIndex;
    }
}
