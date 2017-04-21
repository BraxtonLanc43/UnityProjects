using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniRace_FinishLine : MonoBehaviour {

    public GameObject dirtPath;

	// Use this for initialization
	void Start () {
        spawnPosition();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void spawnPosition()
    {
        Vector3 dirtpos = Camera.main.WorldToViewportPoint(dirtPath.transform.position);
        transform.position = Camera.main.ViewportToWorldPoint(new Vector3(.95f, .5f, dirtpos.z));
    }
}
