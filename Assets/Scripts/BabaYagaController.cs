using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding; // A* algorithm


public class BabaYagaController : MonoBehaviour
{

    private AIDestinationSetter m_DestSetter;

    public GameObject m_CurseObject; // later will be a Prefab
    public Transform m_PlayerTransform;
    public GameObject m_EndsOfMap;

    private Transform[] m_OtherTargets;
    private bool m_IsFlyAway = false;
    private float m_FlyingTimePassed = 0f;
    private float m_MaxFlyingTime = 0f; // always random



    private void Awake()
    {
        m_CurseObject.SetActive(false);

        m_DestSetter = GetComponent<AIDestinationSetter>();

        m_OtherTargets = new Transform[19]; 
        for (int i = 0; i < 19; i++)
        {
            m_OtherTargets[i] = m_EndsOfMap.transform.GetChild(i).transform;
        }

    }

    private void Update()
    {
        if (m_IsFlyAway)
            m_FlyingTimePassed += Time.deltaTime;

        if(m_FlyingTimePassed > m_MaxFlyingTime)
        {
            m_FlyingTimePassed = 0f;
            m_IsFlyAway = false;

            m_DestSetter.target = m_PlayerTransform;
        }
    }

    private void ThrowCurse()
    {
        m_CurseObject.SetActive(true);
        m_CurseObject.GetComponent<Transform>().position = m_PlayerTransform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            ThrowCurse();
            m_IsFlyAway = true;

            m_DestSetter.target = m_OtherTargets[Random.Range(0, 19)]; // Это от игрока отвлекает 
        }
    }
}
