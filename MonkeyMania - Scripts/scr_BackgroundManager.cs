using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class scr_BackgroundManager : MonoBehaviour {

    public GameObject backgroundFoliage;
    private List<GameObject> arr_BackgroundFoliage;
    private Vector3 lastSpawnPoint;
    private BoxCollider collider;
    private GameObject playerObject;

	// Use this for initialization
	void Start ()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        arr_BackgroundFoliage = new List<GameObject>();
        collider = backgroundFoliage.GetComponent<BoxCollider>();
        lastSpawnPoint = backgroundFoliage.transform.position;
        arr_BackgroundFoliage.Add(backgroundFoliage);
	}
	
	// Update is called once per frame
	void Update ()
    {
        GameObject[] backgrounds = GameObject.FindGameObjectsWithTag("BackgroundFoliage");
        int count_Backgrounds = backgrounds.Length;

        removeBackground(backgrounds);

        if(count_Backgrounds < 10)  //arbitrary # picked to ensure always enough background / ground
        {
            Debug.Log("trying to spawn");
            spawnBackground();
        }
        
    }


    /*
    *@param none
    *@return none
    *@desc Deletes backgrounds once they're off the left of the screen enough
    */
    private void removeBackground(GameObject[] bckgrnds)
    {
        Vector3 player_WorldPosition = Camera.main.WorldToScreenPoint(playerObject.transform.position);

        //sort by x axis
        IEnumerable<GameObject> backs = bckgrnds;
        backs = backs.OrderBy(back => back.transform.position.x);
        List<GameObject> temp = backs.ToList();

        if (temp.Count < 8) //Buffer
            return;

        GameObject secondFarthestLeft = temp[1];
        

        Vector3 background_WorldPosition = Camera.main.WorldToScreenPoint(secondFarthestLeft.transform.position);
        if (background_WorldPosition.x < (player_WorldPosition.x - (Screen.width)))
        {
            DestroyObject(secondFarthestLeft.gameObject);
        }
    }



    /*
    *@param none
    *@return none
    *@desc Spawns background prefab at appropriate space based on what has already been spawned
    */
    private void spawnBackground()
    {
        GameObject new_BackgroundFoliage = (GameObject)Instantiate(backgroundFoliage);

        float width = collider.bounds.size.x;

        float xToSpawn = lastSpawnPoint.x + width;
        Vector3 nextSpawn = new Vector3(xToSpawn, backgroundFoliage.transform.position.y, backgroundFoliage.transform.position.z);
        //Debug.Log("Last Spawn: " + lastSpawnPoint);
        //Debug.Log("Width: " + width);
        //Debug.Log("Next Spawn: " + nextSpawn);

        new_BackgroundFoliage.transform.position = nextSpawn;

        lastSpawnPoint = nextSpawn;

    }




}
