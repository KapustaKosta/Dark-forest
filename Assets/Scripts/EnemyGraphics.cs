using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // A* algorithm
using UnityEngine.U2D;

public class EnemyGraphics : MonoBehaviour
{
    private AIPath m_AIPath;
    public Transform m_PlayerTransform;
    public SpriteRenderer m_EnemySR;

    private void Start()
    {
        m_AIPath = GetComponent<AIPath>();
        // Пусть враг пока стоит, не мешается!
        m_AIPath.canMove = false;
    }


    // Update is called once per frame
    void Update()
    {
        // Пример остановки врага
        if(Input.GetKeyDown(KeyCode.J))
            m_AIPath.canMove = !m_AIPath.canMove;

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (m_AIPath.desiredVelocity.x >= 0f)
            m_EnemySR.flipX = false;
        else if(m_AIPath.desiredVelocity.x <= 0f)
            m_EnemySR.flipX = true;
    }
}
