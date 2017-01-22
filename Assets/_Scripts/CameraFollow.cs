using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform camTarget;
    public Vector2 Offset;

    public float cameraSpeed = 1;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 targetPos = new Vector3(camTarget.position.x+Offset.x, camTarget.position.y+Offset.y, transform.position.z);
        Vector3 movement = (targetPos-transform.position)*Time.deltaTime* cameraSpeed;
        transform.position += movement;
	}
}
