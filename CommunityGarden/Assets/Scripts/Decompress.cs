using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decompress : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReceiveCompress(short posX, short posY, short posZ, short scaleX, short scaleY, short scaleZ, short rotationX, short rotationY, short rotationZ)
    {

        Vector3 pos = new Vector3(posX / 100, posY/ 100, posZ / 100);
        Vector3 scale = new Vector3(scaleX/ 100, scaleY / 100, scaleZ / 100);
        Vector3 rotation = new Vector3(rotationX, rotationY, rotationZ);


    }
}
