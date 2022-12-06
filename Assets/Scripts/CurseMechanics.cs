using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurseMechanics : MonoBehaviour
{
    private PlayerMovement m_PlayerMovementScript;
    private PlayerHealth m_PlayerHealthScript;

    struct PLayerStatsSave
    {
        public Vector2 m_PlayerSpeed;
        public float m_PlayerPower;
    }
    private PLayerStatsSave m_PlayerStats;

    private float m_CurseCoeff = 0.92f;
    private float m_CurseTime = 0f;
    private bool m_IsCurseActive = false;
    private const float m_TimeToWait = 2f;
    private float m_TimePassed = 0f;

    public GameObject m_CurseTimeObj;
    public GameObject m_CurseParticlesObj;
    private Text m_CurseTimeText;

    private void Awake()
    {
        m_PlayerMovementScript = GetComponent<PlayerMovement>();
        m_PlayerHealthScript = GetComponent<PlayerHealth>();

        m_CurseTimeText = m_CurseTimeObj.GetComponent<Text>();
        m_CurseTimeObj.SetActive(false);
        m_CurseParticlesObj.SetActive(false);
    }

    private void Update()
    {
        if (m_IsCurseActive)
        {
            m_CurseTime += Time.deltaTime;
            m_CurseTimeText.text = m_CurseTime.ToString("F1");
        }
    }

    private void ActivateCurse()
    {
        // сохраняем некоторую статистику у игрока
        m_PlayerStats.m_PlayerSpeed = m_PlayerMovementScript.m_Speed;
        m_PlayerStats.m_PlayerPower = m_PlayerHealthScript.m_Power;
        Debug.Log(m_PlayerStats.m_PlayerPower);

        m_IsCurseActive = true;
        m_CurseTimeObj.SetActive(true);
        m_CurseParticlesObj.SetActive(true);

        // сразу уменьшаем скорость игрока
        m_PlayerMovementScript.m_Speed *= m_CurseCoeff;
    }

    private void IncreaseCurse()
    {
        // Уменьшаем скорость
        m_PlayerMovementScript.m_Speed *= m_CurseCoeff;
        
        // Рандомно уменьшаем здоровье
        if (Random.Range(0, 2) == 1)
           m_PlayerHealthScript.m_Health -= Random.Range(1f, 2f);

        // И мощность
        m_PlayerHealthScript.m_Power -= Random.Range(0f, 1f);
    }

    private IEnumerator DisactivateCurse(float time)
    {
        yield return new WaitForSeconds(time);

        m_IsCurseActive = false;
        m_CurseTimeObj.SetActive(false);
        m_CurseParticlesObj.SetActive(false);

        // восстанавливаем некоторые изначальные значения
        m_PlayerMovementScript.m_Speed = m_PlayerStats.m_PlayerSpeed;
        m_PlayerHealthScript.m_Power = m_PlayerStats.m_PlayerPower * 0.95f; // less
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Curse" && !m_IsCurseActive)
            ActivateCurse();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Curse")
        {
            m_TimePassed += Time.deltaTime;
            if (m_TimePassed > m_TimeToWait)
            {
                m_TimePassed = 0f;
                IncreaseCurse();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Curse")
        {
            StartCoroutine(DisactivateCurse(5f));
        }
    }


}
