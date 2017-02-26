using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class scr_Player : MonoBehaviour {

    public Text scoreText;
    public int score;
    public scr_Poo scrPoo;
    public GameObject poo;
    private bool ableToFlingPoo;
    Rigidbody rigidbody;
    private float speed = 10.0f;
    private float rightSpeed = 1.0f;
    private float originalSpeed = 1.0f;
    public GameObject selfPoo;
    private GameObject playerObject;
    Rigidbody pooRigidBody;
    private int pooSpeed = 8;
    private int originalPooSpeed = 8;
    private CharacterController controller;
    private Vector3 moveVector;
    private float upDownSpeed = 15.0f;
    private bool isPooTouch;
    private AudioSource audioSource;
    public AudioClip pooAudio;
    public bool isDead;
    public AudioClip coinAudio;
    Animation animation;
    public GameObject gameManager;

    // Use this for initialization
    void Start ()
    {
        ableToFlingPoo = true;
        animation = GetComponent<Animation>();
        score = 0;
        scoreText.text = score.ToString();
        isDead = false;
        audioSource = GetComponent<AudioSource>();        
        isPooTouch = false;
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        scrPoo = poo.GetComponent<scr_Poo>();
        pooRigidBody = poo.GetComponent<Rigidbody>();
        playerObject = gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(score >= 10 && score < 25)
        {
            rightSpeed = (originalSpeed * 1.5f);
            pooSpeed = (int)Mathf.Ceil(originalPooSpeed *1.5f);
        }   else if(score >= 25 && score < 50)
        {
            rightSpeed = (originalSpeed * 2.0f);
            pooSpeed = (int)Mathf.Ceil(originalPooSpeed * 2.0f);
        }   else if(score >= 50 && score < 100)
        {
            rightSpeed = (originalSpeed * 2.5f);
            pooSpeed = (int)Mathf.Ceil(originalPooSpeed * 2.5f);
        }   else if(score >= 100)
        {
            rightSpeed = (originalSpeed * 3.0f);
            pooSpeed = (int)Mathf.Ceil(originalPooSpeed * 3.0f);
        }

        if (isDead)
            return;

        float top = getYClampTop();
        float bottom = getYClampBottom();
       
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, bottom, top), transform.position.z);

        //keeps player moving right
        MoveRight();

        //TESTING ONLY FOR NON-MOBILE  (It works) : Mobile friendly touch controls below, can leave this with no harm
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPooTouch = true;
            ProjectileForward();
        }

        //If we click to poo button, we don't want to trigger movement too
        if (!isPooTouch)
        {
            //for mobile build
            moveVector = Vector3.zero;
            moveVector.y = Input.GetAxisRaw("Vertical") * speed;

            if (Input.GetMouseButton(0))
            {
                //are we touching top or bottom of screen?
                if (Input.mousePosition.y > Screen.height / 2 && !EventSystem.current.IsPointerOverGameObject())
                {
                    moveVector.y = upDownSpeed;
                }
                else if(!EventSystem.current.IsPointerOverGameObject())
                {
                    moveVector.y = -upDownSpeed;
                }
            }
            controller.Move(moveVector * Time.deltaTime);
        }
    }


    /*
    * When gameobject collides into another gameobject
    */
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
       Debug.Log("Hit something: " + hit.gameObject.tag);

        //collectables
        switch (hit.gameObject.tag)
        {
            case "Banana":
                audioSource.clip = coinAudio;
                audioSource.Play();
                score++;
                scoreText.text = score.ToString();
                Destroy(hit.gameObject);
                break;
            case "Gorilla":
                hit.gameObject.GetComponent<scr_Enemy>().playCaughtSound();
                StopAllGorillas();
                Death();
                break;
            default:
                break;
        }
    }


    /*
    * @param none
    * @returns null
    * @desc Stops all gorillas animations
    */
    private void StopAllGorillas()
    {
        GameObject[] gorillas = GameObject.FindGameObjectsWithTag("Gorilla");

        if(gorillas != null & gorillas.Length > 0)
        {
            for (int i = 0; i < gorillas.Length; i++)
            {
                gorillas[i].gameObject.GetComponent<scr_Enemy>().keepWalking = false;
            }
        }
    }


    /*
    * @param none
    * @returns null
    * @desc Handles functionality when the Player dies (hits a gorilla)
    */
    private void Death()
    {
        if (score > PlayerPrefs.GetInt("HighScore"))
            PlayerPrefs.SetInt("HighScore", score);
        gameManager.gameObject.GetComponent<scr_GameManager>().SetMenuActive();
        isDead = true;
        //play monkey death animation?
        //present score?
    }



    /*
    * @param none
    * @returns float y
    * @desc gets the y position to clamp for the top of the screen, based on the screen size
    */
    private float getYClampBottom()
    {
        Vector3 worldBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, -20.5f));

        // Debug.Log("yClamp: " + yClamp);
        //  Debug.Log("temp: " + temp);
        // Debug.Log("temp2: " + temp2.y);
        // Debug.Log("Result: " + (temp2.y - 2));

        return worldBottom.y;
    }


    /*
    * @param none
    * @returns float y
    * @desc gets the y position to clamp for the bottom of the screen, based on the screen size
    */
    private float getYClampTop()
    {
        Vector3 worldTop = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, -20.5f));

        return worldTop.y - GetComponent<CharacterController>().height;
    }
    

    /*
    * @param none
    * @returns boolean
    * Desc: Throws 'poo' projectiley forward at a constant speed
    */
    public void ProjectileForward()
    {
        GameObject[] poosInExistence = GameObject.FindGameObjectsWithTag("Poo");
        if (poosInExistence != null && poosInExistence.Length == 1)
            ableToFlingPoo = false;
        else if (poosInExistence == null || poosInExistence.Length == 0)
            ableToFlingPoo = true;

        if (isDead || ableToFlingPoo == false)
            return;

        GameObject rockHere = Instantiate(selfPoo, playerObject.transform.position + new Vector3(2, 1, 0), playerObject.transform.rotation) as GameObject;
        audioSource.clip = pooAudio;
        audioSource.Play();
        isPooTouch = false; //reset isPooTouch so next click can resume moving
    }


    /*
    * @param none
    * @returns boolen
    * Desc: Moves gameobject right at a constant speed
    */
    void MoveRight()
    {
        animation.Play("monkey_walk");        
        controller.Move(new Vector3(rightSpeed / 5, 0, 0));
    }

    /*
    * @param none
    * @returns boolen
    * Desc:  Moves gameobject up at a constant speed
    */
    void MoveUp()
    {
        rigidbody.velocity = transform.forward * speed;
    }


    /*
    * @param none
    * @returns boolen
    * Desc:  Moves gameobject down at a constant speed
    */
    void MoveDown()
    {
        rigidbody.velocity = transform.forward * speed;
    }

}
