using UnityEngine;
using System.Collections;

public class scr_Camera : MonoBehaviour
{

    public Transform lookAt;
    private Vector3 offset = new Vector3(0, 5, -20.5f);
    private float yOffset = 5.0f;
    private float zOffset = -20.5f;

    private void LateUpdate()
    {
        if(lookAt.transform.position.x > 0)
        {
            transform.position = new Vector3(lookAt.transform.position.x, yOffset, lookAt.transform.position.z + zOffset);
        }        
        else
        {
            transform.position = new Vector3(0, yOffset, lookAt.transform.position.z + zOffset);
        }
    }
}