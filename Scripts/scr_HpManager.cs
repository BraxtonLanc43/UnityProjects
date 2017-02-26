using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scr_HpManager : MonoBehaviour
{

    public GameObject target;
    public GameObject mainCam;
    private Camera cam;
    private Vector3 startingPosition;
    private Vector3 nextPoint;
    private Vector3 vectorOffset = new Vector3(-20, 0, 20);

    // Use this for initialization
    void Start()
    {
        startingPosition = target.transform.position;
        cam = mainCam.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        nextPoint = cam.ViewportToWorldPoint(startingPosition) + vectorOffset;
        target.transform.position = nextPoint;
    }
}
