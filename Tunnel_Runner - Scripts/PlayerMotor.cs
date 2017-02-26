using UnityEngine;
using System.Collections;

/*
*   NOTES:
*       -No gravity right now
*
*/

public class PlayerMotor : MonoBehaviour
{
    //Troll-specific variables (not from tutorial)
    private float xAccel = 7.0f;
    //end Troll-specific variables

    private float animationDuration = 3.0f;
    private CharacterController controller;
    private float speed = 7.0f;
    private Vector3 moveVector;
    private float verticalVelocity = 0.0f;
    private float gravity = 12.0f;
    private float startTime = 0.0f;

    private bool isDead = false;

	// Use this for initialization
	void Start ()
    {
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isDead)
        {
            return;
        }

        if(Time.time - startTime < animationDuration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }

        //Piece added to restrict 'x' movement since no walls
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, -5.2f, -1.0f);
        transform.position = clampedPosition;
        //end restricting 'x' movement

        moveVector = Vector3.zero;

        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            //verticalVelocity -= gravity * Time.deltaTime;
        }

        //get x
        moveVector.x = Input.GetAxisRaw("Horizontal") * xAccel;

        //for mobile build
        if (Input.GetMouseButton(0))
        {
            //are we holding on left or right side of screen??
            if(Input.mousePosition.x > Screen.width / 2)
            {
                moveVector.x = speed;
            }
            else
            {
                moveVector.x = -speed;
            }
        }

        //get y
        moveVector.y = verticalVelocity;

        //get z
        moveVector.z = speed;

        controller.Move(moveVector * Time.deltaTime);
	}

    public void setSpeed(int modifier)
    {
        speed = 7.0f + modifier;
    }

    //it is being called everytime player collider hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.point.z > transform.position.z + controller.radius)
        {
            death();
        }
    }

    private void death()
    {
        isDead = true;
        GetComponent<ScoreManager>().onDeath();
    }
}
