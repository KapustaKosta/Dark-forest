using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // A* algorithm

public class EnemyGraphics : MonoBehaviour
{
    public AIPath m_AIPath;

    // Update is called once per frame
    void Update()
    {
        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if(m_AIPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(m_AIPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
