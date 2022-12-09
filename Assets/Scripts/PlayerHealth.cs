using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public WolfController m_WolfController;
    public BabaYagaController m_BYController;

    public float m_Health = 100f;
    public float m_Power = 10f;

    public GameObject m_HealthObj;
    public GameObject m_PowerObj;

    private Text m_HealthNumText;
    private Text m_PowerNumText;

    private void Awake()
    {
        m_HealthObj.SetActive(true);
        m_PowerObj.SetActive(true);

        m_HealthNumText = m_HealthObj.GetComponent<Text>();
        m_PowerNumText = m_PowerObj.GetComponent<Text>();

        m_HealthNumText.text = m_Health.ToString("F1");
        m_PowerNumText.text = m_Power.ToString("F1");
    }

    private void GameOver()
    {
        this.tag = "Untagged";

        m_WolfController.TurnEnemyOff();
        m_BYController.TurnEnemyOff();
    }

    private void Update()
    {
        m_HealthNumText.text = m_Health.ToString("F1");
        m_PowerNumText.text = m_Power.ToString("F1");

        if(m_Health <= 0f)
        {
            GameOver();
        }
    }

}
