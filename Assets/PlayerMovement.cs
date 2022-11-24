using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Basic movement system 

    public Vector2 speed = new Vector2(2f, 1f);
    Vector2 movement;
    //public float speed = 6.0f;
    //public Rigidbody2D rigitBody2d;

    // Update is called once per frame
    void Update()
    {
        // Input
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical"); 

        Vector3 translate = new Vector3(movement.x * speed.x, movement.y * speed.y, 0f) * Time.deltaTime;
        transform.Translate(translate);
    }
}
