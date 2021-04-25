using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void InterpoltionDR(float t)
    {

        Vector3 pos = new Vector3(0, 0);
        Vector3 posP = new Vector3(0, 0);
        Vector3 pos0 = new Vector3(0, 0);
        Vector3 velocityb = new Vector3(0, 0);
        Vector3 velocity = new Vector3(0, 0);
        Vector3 velocityP = new Vector3(0, 0);
        Vector3 accelerationP = new Vector3(0, 0);

        if(t > 1)
        {

            t = 1;
        }

        velocityb = velocity + (velocityP - velocity) * t;
        pos0 = pos + velocityb * t + .5f * accelerationP * t * t;
        posP = posP + velocityP * t + .5f * accelerationP * t * t;
        pos = pos0 + (posP - pos0) * t;

    }
}
