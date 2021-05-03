using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    //player movement

    private CharacterController controller;
    private float verticalVelocity;
    private float horizontalVelocity;
    private Vector3 mousePos;

    public float moveSpeed = 1.0f;

    private void Update()
    {
        //referenced API for reminder on getting sprite to follow mouse:
        //https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = Vector2.Lerp(transform.position, mousePos, moveSpeed);

        //Vector2 movement = Vector2.zero;

        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{

        //    movement.x += 1;
        //}
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //{

        //    movement.x -= 1;
        //}
        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //{

        //    movement.y += 1;
        //}
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //{

        //    movement.y -= 1;
        //}


        //controller.Move(mousePos);
    }


}
