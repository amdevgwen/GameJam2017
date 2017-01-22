using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{

    public GameObject Origin;
    public GameObject BrickPrefab;
    // Use this for initialization
    void Start()
    {
        Vector3 OriginPosition = Origin.transform.position;
        GameObject Previous = Origin;
        for(int i = 0; i< 40; i++)
        {
            Vector3 NewPos = OriginPosition;
            NewPos.x += (i + 1) * 4;
            GameObject Current = Instantiate(BrickPrefab, NewPos,Quaternion.identity);

            SpringJoint2D ac = Current.AddComponent<SpringJoint2D>();
            ac.connectedBody = Previous.GetComponent<Rigidbody2D>();
            //ac.autoConfigureConnectedAnchor = true;
            ac.autoConfigureConnectedAnchor = false;
            ac.autoConfigureDistance = false;
            ac.dampingRatio = 0.1f;
            Previous = Current;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
