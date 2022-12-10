using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public WolfController m_WolfController;
    public BabaYagaController m_BYController;

    // -------- Movement --------
    public Vector2 m_Speed;
    public Vector2 m_Movement;

    // ---- Health and other ------
    public float m_Health = 100f;
    public float m_Power = 10f;

    public GameObject m_HealthObj;
    public GameObject m_PowerObj;

    private Text m_HealthNumText;
    private Text m_PowerNumText;

    // ------ Damaging --------
    public GameObject m_DamageCollider;
    private bool m_IsColliderActive = false;
    private float m_ColliderTimePassed = 0f;
    private float m_ColliderMaxTime = 0.4f;


    private void Awake()
    {
        m_DamageCollider.SetActive(false);

        // ---- Health -----
        m_HealthObj.SetActive(true);
        m_PowerObj.SetActive(true);

        m_HealthNumText = m_HealthObj.GetComponent<Text>();
        m_PowerNumText = m_PowerObj.GetComponent<Text>();

        m_HealthNumText.text = m_Health.ToString("F1");
        m_PowerNumText.text = m_Power.ToString("F1");
    }

    void Update()
    {
        // Attack (Damage)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_DamageCollider.SetActive(true);
            m_IsColliderActive = true;
        }

        if (m_IsColliderActive)
            m_ColliderTimePassed += Time.deltaTime;

        if (m_ColliderTimePassed > m_ColliderMaxTime)
        {
            m_ColliderTimePassed = 0f;
            m_IsColliderActive = false;
            m_DamageCollider.SetActive(false);
        }

        // Input
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = Input.GetAxis("Vertical");

        // Movement
        Vector3 translate = new Vector3(m_Movement.x * m_Speed.x, m_Movement.y * m_Speed.y, 0f) * Time.deltaTime;
        transform.Translate(translate);

        // Health
        m_HealthNumText.text = m_Health.ToString("F1");
        m_PowerNumText.text = m_Power.ToString("F1");

        if (m_Health <= 0f)
        {
            GameOver();
        }

    }

    private void GameOver()
    {
        this.tag = "Untagged";

        m_WolfController.TurnEnemyOff();
        m_BYController.TurnEnemyOff();
    }
}
