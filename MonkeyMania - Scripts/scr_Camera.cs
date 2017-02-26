using UnityEngine;
using System.Collections;

public class scr_Camera : MonoBehaviour
{ 
    public Transform lookAt;
    private Vector3 offset = new Vector3(0, 5, -20.5f);
    private float yOffset = 5.0f;
    private float zOffset = -20.5f;
    private float xOffset = -17.5f;

    private void LateUpdate()
    {
        transform.position = new Vector3(lookAt.transform.position.x - xOffset, 0, lookAt.transform.position.z + zOffset);
    }
}