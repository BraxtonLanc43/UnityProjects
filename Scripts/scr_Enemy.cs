using UnityEngine;
using System.Collections;

public class scr_Enemy : MonoBehaviour
{
    private Transform mainCamera;
    public scr_PlayerMotor player;
    //Animator animator;
    Transform enemy;
    public GameObject enemyHere;
    private float speed = 2.0f;
    Rigidbody rigidbody;
    public bool keepMoving;
    private bool facingLeft;
    private SpriteRenderer spriteRenderer;
    public bool dontMove;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        facingLeft = true;
        keepMoving = true;
        GameObject go_Cam = GameObject.FindGameObjectWithTag("MainCamera");
        mainCamera = go_Cam.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isDead == true || keepMoving == false || dontMove == true)
            return;

        // Debug.Log("Player Dead: " + player.isDead);
        
        if (facingLeft)
            MoveEnemeyLeft();
        else if (!facingLeft)
            MoveEnemeyRight();
    }


    void MoveEnemeyLeft()
    {
        //var moveSpeed = speed * Time.deltaTime;
        //transform.position = transform.position + (Vector3.left * moveSpeed);
        rigidbody = enemyHere.GetComponent<Rigidbody>();

        rigidbody.AddForce(transform.right * speed, ForceMode.Impulse);

        float distance = mainCamera.transform.position.x - transform.position.x;

        if (Mathf.Abs(distance) > 40)   //Destory enemy when far enough off the screen
        {
            DestroyObject(gameObject);
        }
    }

    void MoveEnemeyRight()
    {
        //var moveSpeed = speed * Time.deltaTime;
        //transform.position = transform.position + (Vector3.left * moveSpeed);
        rigidbody = enemyHere.GetComponent<Rigidbody>();

        rigidbody.AddForce(-(transform.right * speed), ForceMode.Impulse);

        float distance = mainCamera.transform.position.x - transform.position.x;

        //if (Mathf.Abs(distance) > 40)   //Destory enemy when far enough off the screen
        //{
        //    DestroyObject(gameObject);
        //}
    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 currentPos = transform.position;
        // Debug.Log("Hit here 1" + hit.gameObject.tag);
        //collectables
        switch (hit.gameObject.tag)
        {
            case "Player":
                stepBack(currentPos);
                player.PauseEverything();
               // Debug.Log("Hit Player");
                break;
            default:
                break;
        }
    }

    private void stepBack(Vector3 pos)
    {
        if (player.facingForward == true)
        {
            transform.position = pos + new Vector3(30, 0, 0);
        }
        else if (player.facingForward == false)
        {
            transform.position = pos - new Vector3(30, 0, 0);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collision: " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Player")
        {
            GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int i = 0; i < allEnemies.Length; i++)
            {
                scr_Enemy s = allEnemies[i].GetComponent<scr_Enemy>();
                s.keepMoving = false;
            }
            keepMoving = false;
        }
        else if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Earth")
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            facingLeft = !facingLeft;
        }
        else if(collision.gameObject.tag == "Water")
        {
            DestroyObject(transform.gameObject);
        }
    }
}

