using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickScript : MonoBehaviour {

    public Rigidbody2D body;

    public bool Moveable = false;

    private float initX;

	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
        initX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        float speed = 25f;
        if (Moveable)
        {
            //body.velocity = new Vector3(0, Input.GetAxis("Vertical") * speed, 0);
            Vector3 pos = transform.position;
            pos.y += Input.GetAxis("Vertical") * speed;
            transform.position = pos;
        }

        Vector3 position = transform.position;
        position.x = initX;
        transform.position= position;
    }
}
