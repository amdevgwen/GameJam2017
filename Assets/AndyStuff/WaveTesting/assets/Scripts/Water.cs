using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Water : MonoBehaviour
{

	//Our renderer that'll make the top of the water visible
	LineRenderer Body;

	//Our physics arrays
	List<float> xpositions = new List<float>();
	List<float> ypositions = new List<float>();
	List<float> velocities = new List<float>();
	List<float> accelerations = new List<float>();

	//Our meshes and colliders
	List<GameObject> meshobjects = new List<GameObject>();
	List<GameObject> colliders = new List<GameObject>();
	List<Mesh> meshes = new List<Mesh>();


	//Our particle system
	public GameObject splash;

	//The material we're using for the top of the water
	public Material mat;

	//The GameObject we're using for a mesh
	public GameObject watermesh;

	//All our constants
	const float springconstant = 0.02f;
	const float damping = 0.04f;
	const float spread = 0.05f;
	const float z = -1f;

	//The properties of our water
	float baseheight;
	float left;
	float bottom;
	float width;

	BoxCollider2D box;

	void Start()
	{
        
		//Spawning our water
		SpawnWater(-50, 100, -2, -10);
	}

    
	public void Splash(float xpos, float velocity)
	{
		//If the position is within the bounds of the water:
		if (xpos >= xpositions [0] && xpos <= xpositions [xpositions.Count - 1])
		{
			//Offset the x position to be the distance from the left side
			xpos -= xpositions [0];

			//Find which spring we're touching
			int index = Mathf.RoundToInt((xpositions.Count - 1) * (xpos / (xpositions [xpositions.Count - 1] - xpositions [0])));

			//Add the velocity of the falling object to the spring
			velocities [index] += velocity * 2f;

			//Set the lifetime of the particle system.
			float lifetime = 0.93f + Mathf.Abs(velocity) * 0.07f;

			//Set the splash to be between two values in Shuriken by setting it twice.
			splash.GetComponent<ParticleSystem>().startSpeed = 8 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
			splash.GetComponent<ParticleSystem>().startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(velocity), 0.5f);
			splash.GetComponent<ParticleSystem>().startLifetime = lifetime;

			//Set the correct position of the particle system.
			Vector3 position = new Vector3(xpositions [index], ypositions [index] - 0.35f, 5);

			//This line aims the splash towards the middle. Only use for small bodies of water:
			Quaternion rotation = Quaternion.LookRotation(new Vector3(xpositions [Mathf.FloorToInt(xpositions.Count / 2)], baseheight + 8, 5) - position);
            
			//Create the splash and tell it to destroy itself.
			GameObject splish = Instantiate(splash, position, rotation) as GameObject;
			Destroy(splish, lifetime + 0.3f);
		}


		// FOR TESTING!!!
		// REMEMBER TO DELETE!!!
		//MoveBackward(10);
		//MoveForward(10);
	}

	public void SpawnWater(float Left, float Width, float Top, float Bottom)
	{


		//Bonus exercise: Add a box collider to the water that will allow things to float in it.
		gameObject.AddComponent<BoxCollider2D>();
		gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(Left + Width / 2, (Top + Bottom) / 2);
		gameObject.GetComponent<BoxCollider2D>().size = new Vector2(Width, Top - Bottom);
		gameObject.GetComponent<BoxCollider2D>().isTrigger = true;


		//Calculating the number of edges and nodes we have
		int edgecount = Mathf.RoundToInt(Width) * 5;
		int nodecount = edgecount + 1;
        
		//Add our line renderer and set it up:
		Body = gameObject.AddComponent<LineRenderer>();
		Body.material = mat;
		Body.material.renderQueue = 1000;
		Body.SetVertexCount(nodecount);
		Body.SetWidth(0.1f, 0.1f);

		//Declare our physics arrays
		//xpositions = new List<float>(nodecount);
		//ypositions = new List<float>(nodecount);
		//velocities = new List<float>(nodecount);
		//accelerations = new List<float>(nodecount);
        
		//Declare our mesh arrays
		//meshobjects = new List<GameObject>(edgecount);
		//meshes = new List<Mesh>(edgecount);
		//colliders = new List<GameObject>(edgecount);

		//Set our variables
		baseheight = Top;
		bottom = Bottom;
		left = Left;
		width = Width;

		//For each node, set the line renderer and our physics arrays
		for (int i = 0; i < nodecount; i++)
		{
			ypositions.Add(Top);
			xpositions.Add(Left + Width * i / edgecount);
			Body.SetPosition(i, new Vector3(xpositions [i], Top, z));
			accelerations.Add(0);
			velocities.Add(0);
		}

		//Setting the meshes now:
		for (int i = 0; i < edgecount; i++)
		{
			//Make the mesh
			meshes.Add(new Mesh());

			//Create the corners of the mesh
			Vector3[] Vertices = new Vector3[4];
			Vertices [0] = new Vector3(xpositions [i], ypositions [i], z);
			Vertices [1] = new Vector3(xpositions [i + 1], ypositions [i + 1], z);
			Vertices [2] = new Vector3(xpositions [i], bottom, z);
			Vertices [3] = new Vector3(xpositions [i + 1], bottom, z);

			//Set the UVs of the texture
			Vector2[] UVs = new Vector2[4];
			UVs [0] = new Vector2(0, 1);
			UVs [1] = new Vector2(1, 1);
			UVs [2] = new Vector2(0, 0);
			UVs [3] = new Vector2(1, 0);

			//Set where the triangles should be.
			int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

			//Add all this data to the mesh.
			meshes [i].vertices = Vertices;
			meshes [i].uv = UVs;
			meshes [i].triangles = tris;

			//Create a holder for the mesh, set it to be the manager's child
			meshobjects.Add(Instantiate(watermesh, Vector3.zero, Quaternion.identity) as GameObject);
			meshobjects [i].GetComponent<MeshFilter>().mesh = meshes [i];
			meshobjects [i].transform.parent = transform;

			//Create our colliders, set them be our child
			colliders.Add(new GameObject());
			colliders [i].name = "Trigger";
			colliders [i].AddComponent<BoxCollider2D>();
			colliders [i].transform.parent = transform;

			//Set the position and scale to the correct dimensions
			colliders [i].transform.position = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, 0);
			colliders [i].transform.localScale = new Vector3(Width / edgecount, 1, 1);

			//Add a WaterDetector and make sure they're triggers
			colliders [i].GetComponent<BoxCollider2D>().isTrigger = true;
			colliders [i].AddComponent<WaterDetector>();

			box = transform.GetComponent<BoxCollider2D>();
		}
        
	}

	//Same as the code from in the meshes before, set the new mesh positions
	void UpdateMeshes()
	{
		for (int i = 0; i < meshes.Count; i++)
		{

			Vector3[] Vertices = new Vector3[4];
			Vertices [0] = new Vector3(xpositions [i], ypositions [i], z);
			Vertices [1] = new Vector3(xpositions [i + 1], ypositions [i + 1], z);
			Vertices [2] = new Vector3(xpositions [i], bottom, z);
			Vertices [3] = new Vector3(xpositions [i + 1], bottom, z);

			meshes [i].vertices = Vertices;
		}
	}

	//Called regularly by Unity
	void FixedUpdate()
	{
		/* // For testing porpoises.
        if (Input.GetMouseButtonDown(0))
        {
            MoveForward(10);
        }
        */

		//Here we use the Euler method to handle all the physics of our springs:
		for (int i = 0; i < xpositions.Count; i++)
		{
			float force = springconstant * (ypositions [i] - baseheight) + velocities [i] * damping;
			accelerations [i] = -force;
			ypositions [i] += velocities [i];
			velocities [i] += accelerations [i];
			Body.SetPosition(i, new Vector3(xpositions [i], ypositions [i], z));
		}

		//Now we store the difference in heights:
		float[] leftDeltas = new float[xpositions.Count];
		float[] rightDeltas = new float[xpositions.Count];

		//We make 8 small passes for fluidity:
		for (int j = 0; j < 8; j++)
		{
			for (int i = 0; i < xpositions.Count; i++)
			{
				//We check the heights of the nearby nodes, adjust velocities accordingly, record the height differences
				if (i > 0)
				{
					leftDeltas [i] = spread * (ypositions [i] - ypositions [i - 1]);
					velocities [i - 1] += leftDeltas [i];
				}
				if (i < xpositions.Count - 1)
				{
					rightDeltas [i] = spread * (ypositions [i] - ypositions [i + 1]);
					velocities [i + 1] += rightDeltas [i];
				}
			}

			//Now we apply a difference in position
			for (int i = 0; i < xpositions.Count; i++)
			{
				if (i > 0)
					ypositions [i - 1] += leftDeltas [i];
				if (i < xpositions.Count - 1)
					ypositions [i + 1] += rightDeltas [i];
			}
		}
		//Finally we update the meshes to reflect this
		UpdateMeshes();
	}


	void OnTriggerStay2D(Collider2D Hit)
	{
		if (Hit.gameObject.GetComponent<FloatingObject>())
		{
			FloatingObject k = Hit.gameObject.GetComponent<FloatingObject>();
			Rigidbody2D rbd2d = Hit.GetComponent<Rigidbody2D>();
			foreach (Transform m in k.boyancyTargets)
			{
				if (box.OverlapPoint(m.position))
				{
					rbd2d.AddForceAtPosition(Vector2.up * k.boyancy, m.position, ForceMode2D.Force);
				}
			}
		}
		//Bonus exercise. Fill in your code here for making things float in your water.
		//You might want to even include a buoyancy constant unique to each object!
	}


	// Remove verticies from the left end of the water and add them to the right.
	void MoveForward(int vertices)
	{
		// Have to destroy stuff first before removing them from the lists
		for (int i = 0; i < vertices; i++)
		{
			Destroy(meshobjects [i]);
			Destroy(colliders [i]);
			Destroy(meshes [i]);
		}

		// Clear out old values on the left
		meshobjects.RemoveRange(0, vertices);
		colliders.RemoveRange(0, vertices);
		meshes.RemoveRange(0, vertices);

		xpositions.RemoveRange(0, vertices);
		ypositions.RemoveRange(0, vertices);
		velocities.RemoveRange(0, vertices);
		accelerations.RemoveRange(0, vertices);

		// This is a thing that needs to happen for some reason.
		int edgecount = Mathf.RoundToInt(width) * 5;

		// Since there's 5 meshes per one unity "unit", according to the previous line, add .2 to left for each mesh removed.
		// Hopefully this works.
		left += 0.2f * vertices;

		// Doing this seprately because reasons
		for (int i = 0; i < vertices; i++)
		{
			int index = ypositions.Count - 1;
			ypositions.Add(baseheight);
			xpositions.Add(left + width * xpositions.Count / edgecount);
			Body.SetPosition(index, new Vector3(xpositions [index], baseheight, z));
			accelerations.Add(0);
			velocities.Add(0);
		}

        

		// Add new values to the right, hopefully.
		for (int i = 0; i < vertices; i++)
		{
			//Make the mesh
			meshes.Add(new Mesh());

			int index = meshes.Count - 1;

			//Create the corners of the mesh
			Vector3[] Vertices = new Vector3[4];
			Vertices [0] = new Vector3(xpositions [index], ypositions [index], z);
			Vertices [1] = new Vector3(xpositions [index + 1], ypositions [index + 1], z);
			Vertices [2] = new Vector3(xpositions [index], bottom, z);
			Vertices [3] = new Vector3(xpositions [index + 1], bottom, z);

			//Set the UVs of the texture
			Vector2[] UVs = new Vector2[4];
			UVs [0] = new Vector2(0, 1);
			UVs [1] = new Vector2(1, 1);
			UVs [2] = new Vector2(0, 0);
			UVs [3] = new Vector2(1, 0);

			//Set where the triangles should be.
			int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

			//Add all this data to the mesh.
			meshes [index].vertices = Vertices;
			meshes [index].uv = UVs;
			meshes [index].triangles = tris;

			//Create a holder for the mesh, set it to be the manager's child
			meshobjects.Add(Instantiate(watermesh, Vector3.zero, Quaternion.identity) as GameObject);

			//index = meshobjects.Count - 1; // should be the same as the previous index, but better safe than sorry.

			meshobjects [index].GetComponent<MeshFilter>().mesh = meshes [index];
			meshobjects [index].transform.parent = transform;

			//Create our colliders, set them be our child
			colliders.Add(new GameObject());

			//index = colliders.Count - 1; // should be the same as the previous index, but better safe than sorry.

			colliders [index].name = "Trigger";
			colliders [index].AddComponent<BoxCollider2D>();
			colliders [index].transform.parent = transform;

			//Set the position and scale to the correct dimensions
			colliders [index].transform.position = new Vector3(left + width * (index + 0.5f) / edgecount, baseheight - 0.5f, 0);
			colliders [index].transform.localScale = new Vector3(width / edgecount, 1, 1);

			//Add a WaterDetector and make sure they're triggers
			colliders [index].GetComponent<BoxCollider2D>().isTrigger = true;
			colliders [index].AddComponent<WaterDetector>();
		}
	}

	// Remove verticies from the right end of the water and add them to the left.

	void MoveBackward(int vertices)
	{
		// Need this later
		int xLength = xpositions.Count;

		// Have to destroy stuff first before removing them from the lists
		for (int i = vertices; i > 0; i--)
		{
			Destroy(meshobjects [meshobjects.Count - i]);
			Destroy(colliders [colliders.Count - i]);
			Destroy(meshes [meshes.Count - i]);
		}
        
		// Clear out old values on the left
		meshobjects.RemoveRange(meshobjects.Count - vertices, vertices);
		colliders.RemoveRange(colliders.Count - vertices, vertices);
		meshes.RemoveRange(meshes.Count - vertices, vertices);

		xpositions.RemoveRange(xpositions.Count - 1 - vertices, vertices);
		ypositions.RemoveRange(ypositions.Count - 1 - vertices, vertices);
		velocities.RemoveRange(velocities.Count - 1 - vertices, vertices);
		accelerations.RemoveRange(accelerations.Count - 1 - vertices, vertices);

		// This is a thing that needs to happen for some reason.
		int edgecount = Mathf.RoundToInt(width) * 5;

		// Since there's 5 meshes per one unity "unit", according to the previous line, remove .2 to left for each mesh removed.
		// Hopefully this works.
		left -= 0.2f * vertices;
        
		/*
        // Doing this seprately because reasons
        for (int i = 0; i < vertices; i++)
        {
            ypositions.Insert(i, baseheight);
            xpositions.Insert(i, left + width * xLength / edgecount);
            Body.SetPosition(i, new Vector3(xpositions[i], baseheight, z));
            accelerations.Insert(i, 0);
            velocities.Insert(i, 0);
        }



        // Add new values to the left, hopefully.
        for (int i = 0; i < vertices; i++)
        {
            //Make the mesh
            meshes.Insert(i, new Mesh());

            //Create the corners of the mesh
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xpositions[i], ypositions[i], z);
            Vertices[1] = new Vector3(xpositions[i + 1], ypositions[i + 1], z);
            Vertices[2] = new Vector3(xpositions[i], bottom, z);
            Vertices[3] = new Vector3(xpositions[i + 1], bottom, z);

            //Set the UVs of the texture
            Vector2[] UVs = new Vector2[4];
            UVs[0] = new Vector2(0, 1);
            UVs[1] = new Vector2(1, 1);
            UVs[2] = new Vector2(0, 0);
            UVs[3] = new Vector2(1, 0);

            //Set where the triangles should be.
            int[] tris = new int[6] { 0, 1, 3, 3, 2, 0 };

            //Add all this data to the mesh.
            meshes[i].vertices = Vertices;
            meshes[i].uv = UVs;
            meshes[i].triangles = tris;

            //Create a holder for the mesh, set it to be the manager's child
            meshobjects.Insert(i, Instantiate(watermesh, Vector3.zero, Quaternion.identity) as GameObject);

            //index = meshobjects.Count - 1; // should be the same as the previous index, but better safe than sorry.

            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.parent = transform;

            //Create our colliders, set them be our child
            colliders.Insert(i, new GameObject());

            //index = colliders.Count - 1; // should be the same as the previous index, but better safe than sorry.

            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;

            //Set the position and scale to the correct dimensions
            colliders[i].transform.position = new Vector3(left + width * (i + 0.5f) / edgecount, baseheight - 0.5f, 0);
            colliders[i].transform.localScale = new Vector3(width / edgecount, 1, 1);

            //Add a WaterDetector and make sure they're triggers
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            colliders[i].AddComponent<WaterDetector>();
        }
        */
	}



}
