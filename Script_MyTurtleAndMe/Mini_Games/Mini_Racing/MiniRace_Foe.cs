using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRace_Foe : MonoBehaviour {

    public GameObject dirtPath;
    public Animator animator;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        spawnPosition();
        faceEast();
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
    * @param void
    * @returns void
    * @desc Flips the sprite to face east
    * @status Working
    * @interface scrI_NPC
    */
    public void faceEast()
    {
        //Flip to East sprite
        animator.SetBool("toIdleNorth", false);
        animator.SetBool("toIdleEast", true);
        animator.SetBool("toIdleSouth", false);
        animator.SetBool("toIdleWest", false);
        animator.SetBool("toRunNorth", false);
        animator.SetBool("toRunEast", false);
        animator.SetBool("toRunSouth", false);
        animator.SetBool("toRunWest", false);
    }

}
