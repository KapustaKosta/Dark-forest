using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    public Transform m_Target;
    public float m_Speed = 100f;
    public float m_NextWaypointDistance = 2f;

    Path m_Path;
    int m_CurrentWaypoint = 0;

    public Transform m_EnemyTransform;

    Seeker m_Seeker;
    Rigidbody2D m_RigidBody;

    void Start()
    {
        // Getting neccesary components 
        m_Seeker = GetComponent<Seeker>();
        m_RigidBody = GetComponent<Rigidbody2D>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if(m_Seeker.IsDone())
            m_Seeker.StartPath(m_RigidBody.position, m_Target.position, OnPathComplete);
    }

    void OnPathComplete(Path path)
    {
        if (!path.error)
        {
            m_Path = path;
            m_CurrentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (m_Path == null || m_CurrentWaypoint >= m_Path.vectorPath.Count)
            return;

        // Moving the enemy
        Vector2 direction = ((Vector2)m_Path.vectorPath[m_CurrentWaypoint] - m_RigidBody.position).normalized;
        Vector2 force = direction * m_Speed * Time.fixedDeltaTime;

        m_RigidBody.AddForce(force);

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (force.x >= 0.01f)
            m_EnemyTransform.localScale = new Vector3(-1f, 1f, 1f);
        else if (force.x <= -0.01f)
            m_EnemyTransform.localScale = new Vector3(1f, 1f, 1f);

        // Getting distance to next way point 
        float distance = Vector2.Distance(m_RigidBody.position, m_Path.vectorPath[m_CurrentWaypoint]);
        if(distance < m_NextWaypointDistance)
            m_CurrentWaypoint++;


    }
}
