using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    //holds game data
    Vector2 pos;
    bool direction = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (this.transform.position.x > 5)
        {

            direction = false;
        }

        if (this.transform.position.x < -5)
        {

            direction = true;
        }

        if (direction)
        {

            this.transform.position = new Vector2(this.transform.position.x + .1f, this.transform.position.y);
        }

        else
        {

            this.transform.position = new Vector2(this.transform.position.x - .1f, this.transform.position.y);
        }

    }

    //get simulation data
    public Vector2 getPosition()
    {

        return this.transform.position;
    }
}
