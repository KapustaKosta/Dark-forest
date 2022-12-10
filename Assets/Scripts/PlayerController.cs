using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public WolfController m_WolfController;
    public BabaYagaController m_BYController;
    private CurseMechanics m_CurseMechanics;

    public struct PlayerState
    {
        public bool Staying; 
        public bool Running;
        public bool HasEnemiesAround; 
        public bool Fighting;
    }
    public PlayerState m_PlayerStates;

    // -------- Movement --------
    public Vector2 m_Speed;
    public Vector2 m_Movement;

    // ---- Health and other ------
    public float m_Health = 100f;
    public float m_Power = 10f;

    public GameObject m_HealthObj;
    public GameObject m_PowerObj;
    public GameObject m_GameOverCanvas;

    private bool m_IsTempVar = true;

    public bool m_IsDestroyWolfs = false;

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
        m_GameOverCanvas.SetActive(false);

        // ---- Health -----
        m_HealthObj.SetActive(true);
        m_PowerObj.SetActive(true);

        m_HealthNumText = m_HealthObj.GetComponent<Text>();
        m_PowerNumText = m_PowerObj.GetComponent<Text>();
        m_CurseMechanics = GetComponent<CurseMechanics>();

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
            this.tag = "Untagged";
            
            m_PlayerStates.Fighting = true;
        }

        if (m_IsColliderActive)
            m_ColliderTimePassed += Time.deltaTime;

        if (m_ColliderTimePassed > m_ColliderMaxTime)
        {
            m_ColliderTimePassed = 0f;
            m_IsColliderActive = false;
            m_DamageCollider.SetActive(false);

            m_PlayerStates.Fighting = false;

            this.tag = "Player";
        }

        // Input
        m_Movement.x = Input.GetAxis("Horizontal");
        m_Movement.y = Input.GetAxis("Vertical");

        if (m_Movement.x != 0f || m_Movement.y != 0f)
        {
            m_PlayerStates.Running = true;
            m_PlayerStates.Staying = false;
        }

        if (m_Movement.x == 0f && m_Movement.y == 0f)
        {
            m_PlayerStates.Running = false;
            m_PlayerStates.Staying = true;
        }


        // Movement
        Vector3 translate = new Vector3(m_Movement.x * m_Speed.x, m_Movement.y * m_Speed.y, 0f) * Time.deltaTime;
        transform.Translate(translate);

        // Health
        m_HealthNumText.text = m_Health.ToString("F1");
        m_PowerNumText.text = m_Power.ToString("F1");

        if (m_Health <= 0f && m_IsTempVar)
        {
            GameOver();
            m_IsTempVar = false;
        }

    }

    public void StartGameAgain()
    {
        this.tag = "Player";
        m_Health = 100f;

        this.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(1f, 1f, 1f, 1f);

        m_WolfController.m_DestSetter.target = transform;
        m_BYController.m_DestSetter.target = transform;
        m_BYController.CanMove(true);

        m_IsDestroyWolfs = false;

        m_GameOverCanvas.SetActive(false);
    }

    private void GameOver()
    {
        this.tag = "Untagged";
        m_CurseMechanics.DisactivateCurse();

        m_Health = 0f;
        m_HealthNumText.text = m_Health.ToString();

        this.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0f, 0f, 0f, 0f);


        m_BYController.CanMove(false);
        m_IsDestroyWolfs = true;

        m_GameOverCanvas.SetActive(true);
    }
}
