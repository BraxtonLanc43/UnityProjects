using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRace_Player : MonoBehaviour {

    public GameObject dirtPath;
    public Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        spawnPosition();
        toIdleEast();
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void spawnPosition()
    {
        Vector3 dirtpos = Camera.main.WorldToViewportPoint(dirtPath.transform.position);
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.1f, dirtpos.y, dirtpos.z));
    }


    /*
    * @param none
    * @returns null
    * @desc Plays "IdleEast" animation
    * @status Working
    */
    public void toIdleEast()
    {
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toWalkNorth", false);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toWalkSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toWalkWest", false);
        animator.SetBool("toWalkEast", false);
        animator.SetBool("toIdleEast", true);
    }
}
