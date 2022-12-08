using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector2 m_Speed;
    private Vector2 m_Movement;

    public GameObject m_DamageCollider;

    private bool m_IsColliderActive = false;
    private float m_ColliderTimePassed = 0f;
    private float m_ColliderMaxTime = 0.4f;


    private void Awake()
    {
        m_DamageCollider.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_DamageCollider.SetActive(true);
            m_IsColliderActive = true;

        }

        if (m_IsColliderActive)
            m_ColliderTimePassed += Time.deltaTime;

        if (m_ColliderTimePassed > m_ColliderMaxTime)
        {
            m_ColliderTimePassed = 0f;
            m_IsColliderActive = false;
            m_DamageCollider.SetActive(false);
        }

        // Input
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = Input.GetAxis("Vertical");

        Vector3 translate = new Vector3(m_Movement.x * m_Speed.x, m_Movement.y * m_Speed.y, 0f) * Time.deltaTime;
        transform.Translate(translate);
    }
}
