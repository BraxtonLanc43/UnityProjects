using UnityEngine;
using System.Collections;

public class scr_Background : MonoBehaviour {

    GameObject camera;
    private float cameraX;

	// Use this for initialization
	void Start ()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update ()
    {
        cameraX = camera.transform.position.x;
        gameObject.transform.position = new Vector3(cameraX, gameObject.transform.position.y, gameObject.transform.position.z);
	}
}
