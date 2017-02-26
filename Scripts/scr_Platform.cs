using UnityEngine;
using System.Collections;

public class scr_Platform : MonoBehaviour
{

    public bool movingLeft;
    private float timeMoving;
    public bool dontMove;
    public float speed = 5.0f;

    // Use this for initialization
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (dontMove == true)
            return;

        timeMoving = Time.timeSinceLevelLoad;

        //move left for 3 seconds
        if (Mathf.FloorToInt(timeMoving / 3) % 2 == 0)
        {
            movingLeft = true;
          //  Debug.Log("Should move left");
            transform.Translate(-Vector3.right * speed * Time.deltaTime);
        }
        //move left for 3 seconds
        else if (Mathf.FloorToInt(timeMoving / 3) % 2 == 1)
        {
            movingLeft = false;
           // Debug.Log("Should move right");
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
