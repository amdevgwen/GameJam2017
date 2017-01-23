using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WaveEngine : NetworkBehaviour
{
    private LineRenderer WaterLine;

    public Material mat;
    public GameObject mesh;


    public GameObject ColliderBase;

    [SerializeField]
    private int SampleSize = 120; //number of sample in the wave
    [SerializeField]
    private float WaveDistance; //expand from center

    private const float Z_DEPTH = -1.0f; //depth in 2d space
    private const float WAVE_DEPTH = -4f; //depth in water
    private const float SAMPLE_RATE = 5.0f; //how many samples it passes through
    private const float RATE_OFFSET = 2.0f;

    private const float LINE_WIDTH_START = 0.1f;
    private const float LINE_WIDTH_END = 0.1f;

    private const float COLLIDER_OFFSET = 0.2f;

    [SyncVar]
    private float CurrentTime = 0.0f;
    [SyncVar]
    public float Amplitude = Mathf.PI;
    [SyncVar]
    public float Speed = 10.0f;
    [SyncVar]
    public float Frequency = 4.0f;
    [SyncVar]
    public float TimeIncrement = 0.01f;
    [SyncVar]
    public float AmplitudeRandomNoise = 0.2f;
    [SyncVar]
    public int Choppiness = 2;

    List<WaterCollider> colliders = new List<WaterCollider>();


    List<GameObject> meshobjects = new List<GameObject>();
    List<Mesh> meshes = new List<Mesh>();

    public GameObject SplashPrefab;

    public ParticleSystem SplashParticleSystem;

    private void Start()
    {
        GameObject SpashObject = GameObject.Instantiate(SplashPrefab, transform);
        NetworkServer.Spawn(SpashObject);
        SplashParticleSystem = SpashObject.GetComponent<ParticleSystem>();
        InitializeCollider();
    }

    private void InitializeCollider()
    {

        float xLocMin = -(WaveDistance / 2) + transform.position.x;
        float yLoc = transform.position.y;
        float deltaSampleWidth = WaveDistance / SampleSize;

        WaterLine = gameObject.AddComponent<LineRenderer>();
        WaterLine.material = mat;
        WaterLine.material.renderQueue = 1000;
        WaterLine.SetVertexCount(SampleSize);
        WaterLine.SetWidth(0.1f, 0.1f);

        GameObject Previous = null;

        for (int i = 0; i < SampleSize; i++)
        {

            Vector3 colliderPos = new Vector3(xLocMin + deltaSampleWidth * i, yLoc, 0);
            GameObject collider = GameObject.Instantiate<GameObject>(ColliderBase, colliderPos, Quaternion.identity);
            collider.transform.parent = transform;
            WaterCollider watrcld = collider.GetComponent<WaterCollider>();
            watrcld.particleSystem = SplashParticleSystem;
            colliders.Add(watrcld);
            BoxCollider2D box = collider.GetComponent<BoxCollider2D>();



            //set waterline
            WaterLine.SetPosition(i, colliderPos);

            Vector2 sizeBox = new Vector2(deltaSampleWidth, 0.5f);
            box.size = sizeBox;

            NetworkServer.Spawn(collider);
            float xInit = xLocMin + deltaSampleWidth * i;
            float xNext = xLocMin + deltaSampleWidth * (i + 1);
            float yInit = yLoc;
            float yNext = yLoc;
            WaterLine.SetPosition(i, new Vector3(xInit, yInit, Z_DEPTH));
            //Make the mesh
            meshes.Add(new Mesh());



            //Create the corners of the mesh
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xInit, yInit, Z_DEPTH);
            Vertices[1] = new Vector3(xNext, yNext, Z_DEPTH);
            Vertices[2] = new Vector3(xInit, WAVE_DEPTH, Z_DEPTH);
            Vertices[3] = new Vector3(xNext, WAVE_DEPTH, Z_DEPTH);

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
            GameObject meshObject = Instantiate(mesh, Vector3.zero, Quaternion.identity) as GameObject;
            meshobjects.Add(meshObject);
            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.parent = transform;
            NetworkServer.Spawn(meshObject);
        }
        //freeze the last
        //Previous.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;

    }

    private void FixedUpdate()
    {

        float deltaSample = Frequency * Mathf.PI / SampleSize; //between each x for sine purposes

        int index = 0;

        CurrentTime += TimeIncrement;

        float[] yData = new float[colliders.Count + 1];

        foreach(var body in colliders)
        {
            Vector3 parentPosition;
            if (transform != null)
            {
                parentPosition = transform.position;
            }
            else
            {
                parentPosition = Vector3.zero;
            }
            float newY = FindY(Amplitude, Frequency, Speed, deltaSample * index, CurrentTime);
            Vector2 newPos = new Vector2(body.transform.position.x, newY);
            newPos = new Vector2(newPos.x + parentPosition.x, newPos.y + parentPosition.y);
            WaterLine.SetPosition(index, newPos);
            body.transform.position = newPos;

            yData[index] = newY;

            index++;
        }

        yData[yData.Length - 1] = FindY(Amplitude, Frequency, Speed, deltaSample * (index + 1), CurrentTime);

        int yIndex = 0;
        foreach (var meshObj in meshes)
        {
            Vector3[] Vertices = meshObj.vertices;
            Vertices[0] = new Vector3(Vertices[0].x, yData[yIndex], Z_DEPTH);
            Vertices[1] = new Vector3(Vertices[1].x, yData[yIndex + 1], Z_DEPTH);
            meshObj.vertices = Vertices;
            meshObj.RecalculateBounds();
            yIndex++;
        }
    }

    private float FindY(float amp, float freq, float speed, float pos, float time)
    {
        return Mathf.PerlinNoise(Time.time, 0f) * (Mathf.Pow(amp, Choppiness) * (Random.Range(1f, 1f + AmplitudeRandomNoise) + 0.5f)) * Mathf.Sin(2 * Mathf.PI * freq * ((pos / speed) + time));
    }

    /*
    private void InitializeWater()
    {
        float xLocMin = -(WaveDistance / 2);
        float deltaSampleWidth = WaveDistance / SampleSize;

        //if Waterline already exist we destroy it
        if(WaterLine != null)
        {
            Destroy(WaterLine);
        }

        WaterLine = gameObject.AddComponent<LineRenderer>();
        WaterLine.material = Mat;
        WaterLine.numPositions = SampleSize;
        WaterLine.startWidth = LINE_WIDTH_START;
        WaterLine.endWidth = LINE_WIDTH_END;

        //let's try a simple wave
        //y(x) = Amp * cos ( 2 * PI ( x / Lambda - freq ) )

        for (int i = 0; i < SampleSize; i++)
        {
            float xInit = xLocMin + deltaSampleWidth * i;
            float xNext = xLocMin + deltaSampleWidth * (i + 1);
            float yInit = FindY(xInit, 1, 1f, 0);
            float yNext = FindY(xNext, 1, 1f, 0);
            WaterLine.SetPosition(i, new Vector3(xInit, yInit, Z_DEPTH));
            //Make the mesh
            meshes.Add(new Mesh());

            

            //Create the corners of the mesh
            Vector3[] Vertices = new Vector3[4];
            Vertices[0] = new Vector3(xInit, yInit, Z_DEPTH);
            Vertices[1] = new Vector3(xNext, yNext, Z_DEPTH);
            Vertices[2] = new Vector3(xInit, WAVE_DEPTH, Z_DEPTH);
            Vertices[3] = new Vector3(xNext, WAVE_DEPTH, Z_DEPTH);

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
            meshobjects.Add(Instantiate(WaterMesh, Vector3.zero, Quaternion.identity) as GameObject);
            meshobjects[i].GetComponent<MeshFilter>().mesh = meshes[i];
            meshobjects[i].transform.parent = transform;

            //Create our colliders, set them be our child
            colliders.Add(new GameObject());
            colliders[i].name = "Trigger";
            colliders[i].AddComponent<BoxCollider2D>();
            colliders[i].transform.parent = transform;

            //Set the position and scale to the correct dimensions
            //colliders[i].transform.position = new Vector3(Left + Width * (i + 0.5f) / edgecount, Top - 0.5f, 0);
            //colliders[i].transform.localScale = new Vector3(Width / edgecount, 1, 1);

            //Add a WaterDetector and make sure they're triggers
            colliders[i].GetComponent<BoxCollider2D>().isTrigger = true;
            //colliders[i].AddComponent<WaterDetector>();

            //box = transform.GetComponent<BoxCollider2D>();
        }
    }

    private void FixedUpdate()
    {
        
    }

    private float FindY(float x, float amp, float wavelength, float freq, float s)
    {
        return amp * Mathf.Cos(2 * Mathf.PI * (x / wavelength - freq));
    }

    //y(x) = AMP * sin(kx + a)
    private float FindY(float x, float amp, float k, float a)
    {
        return amp * Mathf.Sin(k * x + a);
    }
    */
}
