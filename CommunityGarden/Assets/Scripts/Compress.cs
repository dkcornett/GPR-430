using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compress : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SendCompress()
    {

        Vector3 pos = new Vector3(0, 0, 0);
        Vector3 scale = new Vector3(0, 0, 0);
        Vector3 rotation = new Vector3(0, 0, 0);

        short posX = (short)(pos.x * 100);
        short posY = (short)(pos.y * 100);
        short posZ = (short)(pos.z * 100);

        short scaleX = (short)(scale.x * 100);
        short scaleY = (short)(scale.y * 100);
        short scaleZ = (short)(scale.z * 100);

        short rotationX = (short)(rotation.x * 100);
        short rotationY = (short)(rotation.y * 100);
        short rotationZ = (short)(rotation.z * 100);


    }
}
