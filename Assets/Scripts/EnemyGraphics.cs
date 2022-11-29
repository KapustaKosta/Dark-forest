using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // A* algorithm

public class EnemyGraphics : MonoBehaviour
{
    private AIPath m_AIPath;
    public Transform m_PlayerTransform;

    private void Start()
    {
        m_AIPath = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        // Пример остановки врага
        if(Input.GetKeyDown(KeyCode.J))
            m_AIPath.canMove = !m_AIPath.canMove;

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (m_AIPath.desiredVelocity.x >= 0.01f)
        {
            m_PlayerTransform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(m_AIPath.desiredVelocity.x <= -0.01f)
        {
            m_PlayerTransform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
