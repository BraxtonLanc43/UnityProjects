using UnityEngine;
using System.Collections;

public class CameraMotor : MonoBehaviour {

    public Transform lookAt;
    private Vector3 offset = new Vector3(0, 0, -25.5f);

	private void Start()
    {
        
    }

    private void LateUpdate()
    {
        transform.position = lookAt.transform.position + offset;
    }
}
