using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//prototype of deadreckoning

public class DeadReckoning : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DR()
    {

        Vector3 pos = new Vector3(0,0);
        Vector3 velocity = new Vector3(0,0);
        Vector3 acceleration = new Vector3(0,0);
        pos += velocity * Time.deltaTime + .5f * acceleration * Time.deltaTime * Time.deltaTime;
    }

    public void InterpolationDR(float t)
    {

        Vector3 posClient = new Vector3(0, 0);
        Vector3 posServer = new Vector3(0, 0);
        Vector3 posBlend = new Vector3(0, 0);
        Vector3 posNew = new Vector3(0, 0);
        Vector3 velocityB = new Vector3(0, 0);
        Vector3 velocityClient = new Vector3(0, 0);
        Vector3 velocityServer = new Vector3(0, 0);
        Vector3 accelerationServer = new Vector3(0, 0);

        if(t > 1)
        {

            t = 1;
        }

        velocityB = velocityClient + (velocityServer - velocityClient) * t;
        posBlend = posClient + velocityB * t + .5f * accelerationServer * t * t;
        posServer = posServer + velocityServer * t + .5f * accelerationServer * t * t;
        posNew = posBlend + (posServer - posBlend) * t;

    }
}
