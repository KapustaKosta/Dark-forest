using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Basic movement system 

    public Vector2 m_Speed;
    Vector2 m_Movement;

    void Update()
    {
        // Input
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = Input.GetAxis("Vertical"); 

        Vector3 translate = new Vector3(m_Movement.x * m_Speed.x, m_Movement.y * m_Speed.y, 0f) * Time.deltaTime;
        transform.Translate(translate);
    }
}
