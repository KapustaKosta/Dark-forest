using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // A* algorithm


public class BabaYagaController : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_AudioSource;
    [SerializeField]
    private AudioClip calm;
    [SerializeField]
    private AudioClip fight;

    private AIPath m_AIPath;
    private AIDestinationSetter m_DestSetter;

    public Transform m_PlayerTransform;
    private SpriteRenderer m_SpriteRenderer;
    
    public GameObject m_EndsOfMap; // баба яга будет отлетать туда!
    private Transform[] m_OtherTargets; // массив позиций m_EndsOfMap

    public CurseMechanics m_CurseMechanicsScript;
    public InstantiateEnemies m_InstEnemiesScript;
    public PlayerController m_PCScript;

    private bool m_IsCurseActive = false;

    private float m_Health = 75f;
    private bool m_IsDamaged = false;
    private float m_TimeFromDamagePassed = 0f;
    private float m_MaxTimeFromDamage = 0f; // always random

    private float m_TimeForDamageEffectPassed = 0f;
    private float m_TimeForDamageEffect = 1f;

    private bool m_IsStartChasingPlayer = false;


    private void OnEnable()
    {
        m_DestSetter = GetComponent<AIDestinationSetter>();
        m_AIPath = GetComponent<AIPath>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        m_OtherTargets = new Transform[19];
        for (int i = 0; i < 19; i++)
            m_OtherTargets[i] = m_EndsOfMap.transform.GetChild(i).transform;
    }

    private void Update()
    {
        // делаем игрока целью Бабы Яги!
        if (m_IsStartChasingPlayer)
        {
            m_DestSetter.target = m_PlayerTransform;
            m_IsStartChasingPlayer = false;
        }

        // When getting damage
        if (m_IsDamaged)
        {
            m_TimeFromDamagePassed += Time.deltaTime;
            m_TimeForDamageEffectPassed += Time.deltaTime;
        }

        if(m_TimeFromDamagePassed > m_MaxTimeFromDamage)
        {
            m_IsDamaged = false;
            m_TimeFromDamagePassed = 0f;
            m_DestSetter.target = m_PlayerTransform;
        }

        if(m_TimeForDamageEffectPassed > m_TimeForDamageEffect)
        {
            m_TimeForDamageEffectPassed = 0f;
            m_SpriteRenderer.color = Color.white;
        }

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (m_AIPath.desiredVelocity.x >= 0f)
            m_SpriteRenderer.flipX = true;
        else if (m_AIPath.desiredVelocity.x <= 0f)
            m_SpriteRenderer.flipX = false;

        // If no health
        if(m_Health <= 0f)
        {
            m_PCScript.m_Power *= 1.1f;
            m_InstEnemiesScript.m_IsSpawningBabaYagaAllowed = true;
            m_PCScript.m_PlayerStates.HasEnemiesAround = false;
            Destroy(this.gameObject);
        }
    }

    // Для скрипта CurseMechanics 
    public void ChasePlayer()
    {
        m_IsStartChasingPlayer = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Damage":
                m_DestSetter.target = m_OtherTargets[Random.Range(0, 19)]; // Это от игрока отвлекает 
                
                m_SpriteRenderer.color = Color.red;
                
                float damage = Random.Range(m_PCScript.m_Power, m_PCScript.m_Power * 2f);
                m_Health -= damage;
                m_IsDamaged = true;
                m_MaxTimeFromDamage = Random.Range(2f, 4f);

                break;

            case "Player":
                //m_AudioSource.clip = fight;
                //m_AudioSource.Play();
                m_CurseMechanicsScript.ActivateCurse();
                m_DestSetter.target = m_OtherTargets[Random.Range(0, 19)]; // Это от игрока отвлекает 
                break;
        }
    
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "PlayerArea")
            m_PCScript.m_PlayerStates.HasEnemiesAround = true;
        else
            m_PCScript.m_PlayerStates.HasEnemiesAround = false;
    }



}
