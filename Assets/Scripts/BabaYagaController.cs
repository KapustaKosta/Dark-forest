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
    public GameObject m_EndsOfMap; // баба яга будет отлетать туда!
    private SpriteRenderer m_SpriteRenderer;

    public CurseMechanics m_CurseMechanicsScript;

    public Transform[] m_OtherTargets; // массив позиций m_EndsOfMap
    private bool m_IsStartChasingPlayer = false;



    private void Awake()
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

        // Это будет поворачивать врага влево или вправо,
        // в зависимости от направления движения
        if (m_AIPath.desiredVelocity.x >= 0f)
            m_SpriteRenderer.flipX = true;
        else if (m_AIPath.desiredVelocity.x <= 0f)
            m_SpriteRenderer.flipX = false;
    }

    // когда игрок умер, меняем цель бабы яги! 
    public void TurnEnemyOff()
    {
        m_DestSetter.target = m_OtherTargets[Random.Range(0, 19)];
    }

    // Для скрипта CurseMechanics 
    public void ChasePlayer(bool chase)
    {
        m_IsStartChasingPlayer = chase;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //m_AudioSource.clip = fight;
            //m_AudioSource.Play();
            m_CurseMechanicsScript.ActivateCurse();
            m_DestSetter.target = m_OtherTargets[Random.Range(0, 19)]; // Это от игрока отвлекает 
        }
    }
}
