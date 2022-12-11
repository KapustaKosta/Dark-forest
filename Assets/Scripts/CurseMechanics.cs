using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseMechanics : MonoBehaviour
{
    private PlayerController m_PCScript;
    public BabaYagaController m_BYController;

    struct PLayerStatsSave
    {
        public Vector2 m_PlayerSpeed;
        public float m_PlayerPower;
    }
    private PLayerStatsSave m_PlayerStats;

    private float m_CurseCoeff = 0.95f;
    private float m_CurseTime = 0f; // для ui
    public bool m_IsCurseActive = false;
    private const float m_TimeToWait = 2f;
    private float m_TimePassed = 0f; // для проклятья 

    private bool m_IsHidingNegativeEffects = true;
    private bool m_IsHidingObjects = true;
    private bool m_IsHealingObjectActive = false;

    public GameObject m_CurseObject;
    public GameObject m_CurseTimeObj; // показывает время действия проклятья
    public GameObject m_CurseParticlesObj;
    public GameObject m_HealingObj; // для уничтожения проклятья
    public GameObject m_HealingPos;
    private Transform[] m_HealingPosTransforms;
    public ParticleSystem m_ParticleSystem; // искускает частицы
    public SpriteRenderer m_PlayerSR;

    private Text m_CurseTimeText;

    private void Awake()
    {
        m_PCScript = GetComponent<PlayerController>();
        m_PlayerSR.color = new Vector4(1f, 1f, 1f, 1f);

        m_CurseTimeText = m_CurseTimeObj.GetComponent<Text>();
        m_CurseTimeObj.SetActive(false);
        m_CurseParticlesObj.SetActive(false);
        m_HealingObj.SetActive(false);

        m_CurseObject.SetActive(false);

        m_HealingPosTransforms = new Transform[8];
        for (int i = 0; i < 8; i++)
            m_HealingPosTransforms[i] = m_HealingPos.transform.GetChild(i).transform;

    }


    private void Update()
    {
        if (m_IsCurseActive)
        {
            // Показываем время действия проклятья
            m_CurseTime += Time.deltaTime;
            m_CurseTimeText.text = m_CurseTime.ToString("F1");

            // считаем интервалы действия проклятья
            // каждый - 2 секунды
            // потом увеличиваем действие проклятья
            m_TimePassed += Time.deltaTime;
            if (m_TimePassed > m_TimeToWait)
            {
                m_TimePassed = 0f;
                IncreaseCurse();
            }
        }

        if(m_CurseTime > 10f && m_IsHidingObjects)
        {
            // прятать источник проклятья и объект очищения
            // мы просто вернем игроку изначальный цвет
            m_PlayerSR.color = Color.white;

            m_IsHidingObjects = false;
        }


        if(m_CurseTime > 20f && m_IsHidingNegativeEffects)
        {
            // Hide negative effects
            m_ParticleSystem.Stop();

            m_IsHidingNegativeEffects = false;
        }

        if (m_IsHealingObjectActive)
        {
            SpawnHealingObject();
        }

    }

    //
    public void ActivateCurse()
    {
        Debug.Log("Curse activated");

        // сохраняем некоторую статистику у игрока
        m_PlayerStats.m_PlayerSpeed = m_PCScript.m_Speed;
        m_PlayerStats.m_PlayerPower = m_PCScript.m_Power;

        m_IsCurseActive = true;
        m_CurseTimeObj.SetActive(true);
        m_CurseObject.SetActive(true);
        m_CurseParticlesObj.SetActive(true);
        m_ParticleSystem.Play();

        // сразу уменьшаем скорость игрока
        m_PCScript.m_Speed *= m_CurseCoeff;
        m_PlayerSR.color = new Vector4(0.3f, 0.41f, 0.3f, 1f);

        StartCoroutine(ActivateHealingObj());
    }

    private IEnumerator ActivateHealingObj()
    {
        // всегда случайное время появления)
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        m_HealingObj.SetActive(true);
        m_HealingObj.GetComponent<SpriteRenderer>().color = new Vector4(1f, 0f, 0f, 1f);
        m_IsHealingObjectActive = true;
    }

    private void SpawnHealingObject()
    {
        if (m_PCScript.m_Movement.x > 0f && m_PCScript.m_Movement.y == 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[6].position;
        else if (m_PCScript.m_Movement.x < 0f && m_PCScript.m_Movement.y == 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[2].position;
        else if (m_PCScript.m_Movement.x == 0f && m_PCScript.m_Movement.y > 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[4].position;
        else if (m_PCScript.m_Movement.x == 0f && m_PCScript.m_Movement.y < 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[0].position;
        else if (m_PCScript.m_Movement.x > 0f && m_PCScript.m_Movement.y > 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[5].position;
        else if (m_PCScript.m_Movement.x > 0f && m_PCScript.m_Movement.y < 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[7].position;
        else if (m_PCScript.m_Movement.x < 0f && m_PCScript.m_Movement.y < 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[1].position;
        else if (m_PCScript.m_Movement.x < 0f && m_PCScript.m_Movement.y > 0f)
            m_HealingObj.transform.position = m_HealingPosTransforms[3].position;
        else
            m_HealingObj.transform.position = m_PCScript.transform.position;
    }

    private void IncreaseCurse()
    {
        // Уменьшаем скорость
        m_PCScript.m_Speed *= m_CurseCoeff;
        
        // Рандомно уменьшаем здоровье
        if (Random.Range(0, 2) == 1)
            m_PCScript.m_Health -= Random.Range(1f, 2f);

        // И мощь игрока
        m_PCScript.m_Power -= Random.Range(0f, 1f);
    }

    public void DisactivateCurse()
    {
        Debug.Log("Curse disactivated");

        m_IsCurseActive = false;
        m_CurseTimeObj.SetActive(false);
        m_CurseObject.SetActive(false);
        m_CurseParticlesObj.SetActive(false);
        m_ParticleSystem.Stop();

        // восстанавливаем некоторые изначальные значения
        m_PCScript.m_Speed = m_PlayerStats.m_PlayerSpeed;
        m_PCScript.m_Power = m_PlayerStats.m_PlayerPower * 0.95f; // меньше изначального                                                          

        m_PlayerSR.color = Color.white;
        m_CurseTime = 0f;

        m_IsHealingObjectActive = false;
        m_HealingObj.SetActive(false);

        m_BYController.ChasePlayer(); // баба яга снова начинает следовать за игроком!!
    }
}
