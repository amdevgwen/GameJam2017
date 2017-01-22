using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitBottle : MonoBehaviour {

    public GameObject bottle;
    public GameObject water;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

        // Delete existing bottle
        // TODO: Find out what the bottle object is actually called.
        Destroy(GameObject.Find("bottle object"));

        // Delete existing water
        // TODO: Find out what the water object is actually called.
        Destroy(GameObject.Find("water object"));

        // Find the boat object.
        GameObject boat = GameObject.Find("The Shippening");

        // Disable boat rigidbodies.
        boat.GetComponent<Rigidbody2D>().simulated = false;

        // Play Animation
        // TODO: I don't actually know how to do this.

        // Renable boat rigidbodies.
        boat.GetComponent<Rigidbody2D>().simulated = true;

        // Create new water.
        Instantiate(water);

        // Create new bottle.
        Instantiate(bottle);
    }
}
