using UnityEngine;
using System.Collections;

public class scr_HowToMonkey : MonoBehaviour {
    Animation animation;
    private bool collectBanana;

    // Use this for initialization
    void Start ()
    {
        collectBanana = false;
        animation = GetComponent<Animation>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (collectBanana)
        {
            //walk to banana

        }
        

        animation.Play("monkey_walk");
    }
}
