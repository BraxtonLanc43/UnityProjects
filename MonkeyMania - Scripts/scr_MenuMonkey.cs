using UnityEngine;
using System.Collections;

public class scr_MenuMonkey : MonoBehaviour {

    private Animation animation;

    // Use this for initialization
    void Start()
    {
        animation = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
       // Debug.Log("Should be animating");
        animation.Play("monkey_eat");

    }
}
