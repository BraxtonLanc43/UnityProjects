using UnityEngine;
using System.Collections;



public class Player : MonoBehaviour {

    private float inputDirection;
    private float verticalVelocity;
    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;
    private float speed = 5.0f;
    private float jumpForce = 7.5f;
    private float gravity = 30.0f;
    bool secondJumpAvail = false;    

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();        
	}
	
	// Update is called once per frame
	void Update () {
        bool isItGrounded = IsControllerGrounded();
        moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal");

        if (IsControllerGrounded())
        {
            verticalVelocity = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //make player jump
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
            }

            moveVector.x = inputDirection;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //make player jump
                if (secondJumpAvail)
                {
                    verticalVelocity = jumpForce;
                    secondJumpAvail = false;
                }             
            }
            verticalVelocity -= gravity * Time.deltaTime;
            moveVector.x = lastMotion.x;
        }

        
        moveVector.y = verticalVelocity;
        //moveVector = new Vector3(inputDirection, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime * speed);
        lastMotion = moveVector;
	}

    private bool IsControllerGrounded()
    {
        bool isGrounded = false;
        Vector3 leftRayStart;
        Vector3 rightRayStart;
        leftRayStart = controller.bounds.center;
        rightRayStart = controller.bounds.center;

        leftRayStart.x -= controller.bounds.extents.x;
        rightRayStart.x += controller.bounds.extents.x;

        Debug.DrawRay(leftRayStart, Vector3.down, Color.red);
        Debug.DrawRay(rightRayStart, Vector3.down, Color.green);

        if(Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f))
        {
            isGrounded = true;
        }

        if(Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f))
        {
            isGrounded = true;
        }
        //Debug.Log("isItGrounded: " + isGrounded);

        return isGrounded;
    }

    private void forceGrounded()
    {
        moveVector = Vector3.zero;
        inputDirection = Input.GetAxis("Horizontal");

      
            verticalVelocity = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //make player jump
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
            }

            moveVector.x = inputDirection;
        verticalVelocity = jumpForce * 2;
        moveVector.y = verticalVelocity;
        //moveVector = new Vector3(inputDirection, verticalVelocity, 0);
        controller.Move(moveVector * Time.deltaTime * speed);
        lastMotion = moveVector;
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       //Debug.Log("We've hit something");
       // Debug.Log("Collision Flags: " + controller.collisionFlags);
       if(controller.collisionFlags == CollisionFlags.Sides || controller.collisionFlags == CollisionFlags.Below)
        {
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                moveVector = hit.normal * speed;
                verticalVelocity = jumpForce;
                secondJumpAvail = true;
            }
        }

        //collectables
        switch (hit.gameObject.tag)
        {
            case "Coin":
                LevelManager.Instance.CollectCoin();
                Destroy(hit.gameObject);
                break;
            case "Teleporter":
                transform.position = hit.transform.GetChild(0).position;
                break;
            case "Winbox":
                LevelManager.Instance.Win();
                break;
            case "Jumppad":
                //         Debug.Log("Before: " + verticalVelocity);   
                forceGrounded();                        
               
      //          Debug.Log("After: " + verticalVelocity);
      //          Debug.Log("Jumppad Hit");
                break;
            default:
                break;
        }          
    }
}
