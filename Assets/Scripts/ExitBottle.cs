using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBottle : MonoBehaviour
{


	// Use this for initialization
	void Start()
	{
		
	}
	
	// Update is called once per frame
	void Update()
	{
        // For testing only.
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Exit();
        }
        */
    }

	void Exit()
	{
        /* TODO: 
         * Delete existing bottle
         * Delete existing water
         * Play animation
         * 
         * Other stuff maybe?
         */

        // Find the boat object.
        GameObject boat = GameObject.Find("The Shippening");

        // Set the new position of the bottle and water based on the current position of the boat.
        Vector3 bottlePosition = new Vector3(boat.transform.position.x + 40, boat.transform.position.y - 20, boat.transform.position.z);
        Vector3 waterPosition = new Vector3(boat.transform.position.x, boat.transform.position.y - 20, boat.transform.position.z);

        // Move Bottle
        GameObject.Find("Bottle").transform.position = bottlePosition;

		// Move water
		GameObject.Find("WaterManager").transform.position = waterPosition;

		
		// Disable boat rigidbodies.
		boat.GetComponent<Rigidbody2D>().simulated = false;

		// Play Animation
		// TODO: I don't actually know how to do this.

		// Renable boat rigidbodies.
		boat.GetComponent<Rigidbody2D>().simulated = true;

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		Debug.Log("Moves ");
		Exit();
	}
}
