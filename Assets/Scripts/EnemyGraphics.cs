using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // A* algorithm


public class EnemyGraphics : MonoBehaviour
{
    public PlayerHealth m_PlayerHealthScript;

    private AIPath m_AIPath;
    private AIDestinationSetter m_AIDestSetter;
    private SpriteRenderer m_EnemySR;

    public GameObject m_OtherTargets;
    private Transform[] m_OTTransformArr;
    
    public Transform m_PlayerTransform;

    private float m_EnemyHealth = 20f;
    private float m_EnemyPower = 4f;

    private bool m_ExcapePlayer = false;
    private float m_TimeFromDamagePassed = 0f;
    private float m_MaxTimeFromDamage = 0f; 

    
    private void Awake()
    {
        m_EnemySR = GetComponent<SpriteRenderer>(); 
        
        m_AIPath = GetComponent<AIPath>();
        m_AIDestSetter = GetComponent<AIDestinationSetter>();

        m_OTTransformArr = new Transform[13]; // because we have 13 transforms
        for (int i = 0; i < 13; i++) {
            m_OTTransformArr[i] = m_OtherTargets.transform.GetChild(i).transform;
        }
    }

    private void Update()
    {
        // Getting enemy back to attack the Player
        if (m_ExcapePlayer)
            m_TimeFromDamagePassed += Time.deltaTime;

        if (m_TimeFromDamagePassed > m_MaxTimeFromDamage)
        {
            m_ExcapePlayer = false;
            m_TimeFromDamagePassed = 0f;
            m_AIDestSetter.target = m_PlayerTransform;
        }

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (m_AIPath.desiredVelocity.x >= 0f)
            m_EnemySR.flipX = false;
        else if(m_AIPath.desiredVelocity.x <= 0f)
            m_EnemySR.flipX = true;


        // 
        if(m_EnemyHealth <= 0f)
        {
            Destroy(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            int enemyDamage = Random.Range(3, 6);
            m_PlayerHealthScript.m_Health -= enemyDamage * m_EnemyPower;
            m_PlayerHealthScript.m_Power -= Random.Range(0.1f, m_EnemyPower / 10f);
            m_EnemyPower += Random.Range(1.3f, 1.7f);

            m_AIDestSetter.target = m_OTTransformArr[Random.Range(0, 13)];

            m_ExcapePlayer = true;
            m_MaxTimeFromDamage = Random.Range(0.8f, 1.5f);
        }    

        if(collision.tag == "Damage")
        {
            float damage = Random.Range(m_PlayerHealthScript.m_Power, m_PlayerHealthScript.m_Power * 2f);
            m_EnemyHealth -= damage;
            m_AIDestSetter.target = m_OTTransformArr[Random.Range(0, 13)];

            m_ExcapePlayer = true;
            m_MaxTimeFromDamage = Random.Range(1.5f, 2f);
        }

        if(collision.tag == "OtherTarget")
        {
            m_AIDestSetter.target = m_PlayerTransform;
        }
    }

}
