using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollider : MonoBehaviour
{
    public float PushStrength = 700;
    private float initX;
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
            Debug.Log("is boat");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition(
                new Vector2(0, PushStrength), transform.position, ForceMode2D.Force);
        }
    }

}
