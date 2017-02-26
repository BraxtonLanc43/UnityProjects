using UnityEngine;
using System.Collections;

public class scr_Banana : MonoBehaviour {

    //MAY NOT NEED TO BE IMPLEMENTED?
    public GameObject playerObject;

    void Update()
    {
        if (isOffScreenLeft())
        {
            DestroyObject(this.gameObject);
        }
    }


    /*
    * @param none
    * @returns boolean
    * Desc: Checks to see if it is off of the screen to the right. If it is, returns true, else false.
    */
    private bool isOffScreenLeft()
    {
        //get world point
        Vector3 banana_WorldPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 player_WorldPosition = Camera.main.WorldToScreenPoint(playerObject.transform.position);

        if (banana_WorldPosition.x < (player_WorldPosition.x - (Screen.width / 4)))
        {
            return true;
        }

        return false;
    }

}
