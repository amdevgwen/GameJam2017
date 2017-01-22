using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public float PushStrength = 700;
    public float SplashVelocity = 50;
    private float initX;

    public ParticleSystem particleSystem;
    // Use this for initialization
    void Start()
    {
        initX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        position.x = initX;
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.tag == "Boat")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                new Vector2(0, PushStrength), transform.position, ForceMode2D.Force);

            Splash();
        }
    }

    public void Splash()
    {
        float lifetime = 0.93f + Mathf.Abs(SplashVelocity) * 0.07f;

        //Set the splash to be between two values in Shuriken by setting it twice.
        particleSystem.startSpeed = 8 + 2 * Mathf.Pow(Mathf.Abs(SplashVelocity), 0.5f);
        particleSystem.startSpeed = 9 + 2 * Mathf.Pow(Mathf.Abs(SplashVelocity), 0.5f);
        particleSystem.startLifetime = lifetime;

        Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.35f, 5);

        //This line aims the splash towards the middle. Only use for small bodies of water:
        Quaternion rotation = Quaternion.LookRotation(new Vector3(0, 0 + 8, 5) - position);
        `
        //Create the splash and tell it to destroy itself.
        GameObject splish = Instantiate(particleSystem.gameObject, position, rotation) as GameObject;
        Destroy(splish, lifetime + 0.3f);


    }

}
