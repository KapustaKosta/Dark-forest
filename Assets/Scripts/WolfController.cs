using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WolfController : MonoBehaviour
{
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private AudioClip calm;

    public PlayerController m_PCScript;
    public BabaYagaController m_BYController;
    public InstantiateEnemies m_InstEnemiesScript;

    private AIPath m_AIPath;
    private AIDestinationSetter m_AIDestSetter;
    private SpriteRenderer m_SpriteRenderer;

    public GameObject m_OtherTargets;
    private Transform[] m_OTTransformArr; // отвлекают волка от игрока!

    public Transform m_PlayerTransform;

    private float m_EnemyHealth = 20f; 
    private float m_EnemyPower = 4f; // влияет на силу урона игроку

    private bool m_ExcapePlayer = false;
    private float m_TimeFromDamagePassed = 0f;
    private float m_MaxTimeFromDamage = 0f; // всегда разное

    private bool m_IsDamaged = false;
    private float m_TimeForDamageEffectPassed = 0f;
    private float m_TimeForDamageEffectMax = 0.7f; 

    private void OnEnable()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_AIPath = GetComponent<AIPath>();
        m_AIDestSetter = GetComponent<AIDestinationSetter>();

        m_OTTransformArr = new Transform[13]; // у нас 13 transforms в юнити
        for (int i = 0; i < 13; i++)
            m_OTTransformArr[i] = m_OtherTargets.transform.GetChild(i).transform;
    }

    private void Update()
    {
        // Нападение и отхождение от игрока
        if (m_ExcapePlayer)
            m_TimeFromDamagePassed += Time.deltaTime;
        
        if (m_TimeFromDamagePassed > m_MaxTimeFromDamage)
        {
            m_ExcapePlayer = false;
            m_TimeFromDamagePassed = 0f;
            m_AIDestSetter.target = m_PlayerTransform;
        }

        if(m_IsDamaged)
            m_TimeForDamageEffectPassed += Time.deltaTime;

        if(m_TimeForDamageEffectPassed > m_TimeForDamageEffectMax)
        {
            m_IsDamaged = false;
            m_TimeForDamageEffectPassed = 0f;
            m_SpriteRenderer.color = Color.white;
        }

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (m_AIPath.desiredVelocity.x >= 0f)
            m_SpriteRenderer.flipX = false;
        else if (m_AIPath.desiredVelocity.x <= 0f)
            m_SpriteRenderer.flipX = true;


        // Если закончилось здоровье - смерть!
        if (m_EnemyHealth <= 0f)
        {
            //audio.clip = calm;
            //audio.Play();
            m_PCScript.m_Power *= 1.05f;
            m_InstEnemiesScript.m_CurrentWolfAmount--;
            Destroy(this.gameObject);
        }
    }

    // когда игрок умерает, отвлекаем волка на что-то другое!
    public void TurnEnemyOff()
    {
        m_AIDestSetter.target = m_BYController.m_OtherTargets[Random.Range(0, 19)];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Player":
                if (m_AIDestSetter.target == m_PlayerTransform)
                {
                    int enemyDamage = Random.Range(3, 6);
                    m_PCScript.m_Health -= enemyDamage * m_EnemyPower;
                    m_PCScript.m_Power -= Random.Range(0.1f, m_EnemyPower / 10f);
                    m_EnemyPower += Random.Range(1.3f, 1.7f);
                }

                m_AIDestSetter.target = m_OTTransformArr[Random.Range(0, 13)];

                m_ExcapePlayer = true;
                m_MaxTimeFromDamage = Random.Range(0.8f, 1.5f);
                break;

            case "Damage":
                m_SpriteRenderer.color = Color.red;
                
                float damage = Random.Range(m_PCScript.m_Power, m_PCScript.m_Power * 2f);
                m_EnemyHealth -= damage;
                m_AIDestSetter.target = m_OTTransformArr[Random.Range(0, 13)];

                m_ExcapePlayer = true;
                m_IsDamaged = true;
                m_MaxTimeFromDamage = Random.Range(1.5f, 2f);

                break;

            case "OtherTarget":
                m_AIDestSetter.target = m_PlayerTransform;
                break;
        }
    }
}
