using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour
{
    public float cameraX; 

    private Transform lookAt;
    private Vector3 startOffset;
    private Vector3 moveVector;

    private float transition = 0.0f;
    private float animationDuration = 3.0f;
    private Vector3 animationOffset = new Vector3(0, 5, 5);

	// Use this for initialization
	void Start ()
    {
        lookAt = GameObject.FindGameObjectWithTag("Player").transform;
        startOffset = transform.position - lookAt.position + new Vector3(0, 0, 3);
        cameraX = lookAt.position.x;
	}
	
	// Update is called once per frame
	void Update ()
    {
        moveVector = lookAt.position + startOffset;

        //x
        moveVector.x = cameraX;

        //y
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);       

        if(transition > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            //animation at start of game
            transform.position = Vector3.Lerp(moveVector + animationOffset, moveVector, transition);
            transition += Time.deltaTime * 1 / animationDuration;
            transform.LookAt(lookAt.position + Vector3.up);
        }

        
	}
}
