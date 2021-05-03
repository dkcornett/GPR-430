using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationScript : MonoBehaviour
{

    //get simulation data
    private Vector2 startPos;
    bool direction = true;

    // Start is called before the first frame update
    void Start()
    {

        startPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //this.transform.position = new Vector2(startPos.x * Mathf.Sin(Time.deltaTime), startPos.y * Mathf.Cos(Time.deltaTime));

        //if(this.transform.position.x > 5)
        //{

        //    direction = false;
        //}

        //if(this.transform.position.x < -5)
        //{

        //    direction = true;
        //}

        //if (direction)
        //{

        //    this.transform.position= new Vector2(this.transform.position.x + .1f, this.transform.position.y);
        //}

        //else
        //{

        //    this.transform.position = new Vector2(this.transform.position.x - .1f, this.transform.position.y);
        //}
    }

    //set position of game data
    public void SetPosition(Vector2 pos)
    {

        this.transform.position = pos;
    }
}
