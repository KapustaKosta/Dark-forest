using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateEnemies : MonoBehaviour
{
    public GameObject m_WolfPrefab;
    public GameObject m_BabaYagaPrefab;

    public DayNight m_DayNightScript;

    private int m_MaxWolfAmount = 5;
    private float m_TimeAfterSpawn = 0f;
    private float m_MaxSpawnDelay = 0f; // always random
    public int m_CurrentWolfAmount = 0;
    public bool m_IsSpawningBabaYagaAllowed = true;

    private Vector2 m_ScreenBounds;


    void Awake()
    {
        m_ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        m_MaxSpawnDelay = Random.Range(8f, 12f);
    }

    private void SpawnWolf()
    {
        if (m_CurrentWolfAmount <= m_MaxWolfAmount)
        {
            GameObject wolf = Instantiate(m_WolfPrefab) as GameObject;
            wolf.transform.position = new Vector2(m_ScreenBounds.x + Random.Range(-30f, 30f), m_ScreenBounds.y + Random.Range(-30f, 30f));
            wolf.SetActive(true);

            m_CurrentWolfAmount++;
        }
    }

    private void SpawnBabaYaga()
    {
        if (m_IsSpawningBabaYagaAllowed)
        {
            GameObject babaYaga = Instantiate(m_BabaYagaPrefab) as GameObject;
            babaYaga.transform.position = new Vector2(m_ScreenBounds.x + Random.Range(-30f, 30f), m_ScreenBounds.y + Random.Range(-30f, 30f));
            babaYaga.SetActive(true);

            m_IsSpawningBabaYagaAllowed = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(m_DayNightScript.state == State.Night)
        {
            m_TimeAfterSpawn += Time.deltaTime;
            if(m_TimeAfterSpawn > m_MaxSpawnDelay)
            {
                SpawnWolf();
                m_TimeAfterSpawn = 0f;
                m_MaxSpawnDelay = Random.Range(8f, 12f);
            }

            SpawnBabaYaga(); // only one enemy like this
        }
    }
}
