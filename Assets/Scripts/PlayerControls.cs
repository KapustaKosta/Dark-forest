using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    /*
        PlayerControls - class for handling players properties, such as
        health, strength, curse handling, etc

        PlayerControls - класс для контроля статистики игрока, такой как
        здоровье, сила, а также действия проклятья
    */

    private PlayerMovement m_PlayerMovementScript;
    private Vector2 m_PlayerSpeedSave; 

    //private float m_Strength = 20f; // random value

    // For curse (Проклятье)
    // Все еще в разработке)
    /*
    1) Вокруг источника проклятия находится область, при пересечении которой активируется 
    проклятие и накладывается негативный эффект базовой величины.

    2) Пока игрок находится в зоне действия проклятия он получает раз в N секунд “стак”, 
    где каждый стак усиливает проклятие на базовую величину.

    3) Источник проклятия становится подвижным, перемещается в случайную точку окружности 
    радиуса R, где центр - это источник проклятия. При приближении игрока на 
    расстояние M единиц источник телепортируется в случайную точку окружности радиуса R
    в противоположную сторону от игрока. После чего не может телепортироваться в 
    течение T секунд.

    4) При активации проклятия появляется объект очищения (снятия проклятия) в случайном 
    месте в радиусе действия проклятия. Для активации необходимо попасть в конус, 
    расположенный в противоположную сторону от центра области источника проклятия и 
    от игрока. Параметры конуса настраиваются геймдизайнером.

    Должны обрабатываться разные типы проклятий.

    5) При достижении S “стаков” источник проклятия и объект очищения перестают 
    отображаться визуально в игре. При достижении 2S “стаков” пропадает отображение 
    наличия негативного эффекта. 
    */
    private float m_CurseCoeff = 0.92f;
    private float m_CurseTime = 0f;
    private bool m_IsCurseActive = false;
    private float m_TimeToWait = 2f; 

    public GameObject m_CurseTimeObj;
    //public GameObject m_ParticleSystemObj;
    private Text m_CurseTimeText;
    

    private void Awake()
    {
        m_PlayerMovementScript = this.GetComponent<PlayerMovement>();

        m_CurseTimeText = m_CurseTimeObj.GetComponent<Text>();
        m_CurseTimeObj.SetActive(false);
        //m_ParticleSystemObj.SetActive(false);

        //m_CurseTimeText.text = m_CurseTime.ToString("F1");
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
        m_PlayerSpeedSave = m_PlayerMovementScript.m_Speed;
        
        m_IsCurseActive = true;
        m_CurseTimeObj.SetActive(true);

        m_PlayerMovementScript.m_Speed *= m_CurseCoeff;
    }

    private void IncreaseCurse()
    {
        m_CurseCoeff -= 0.00001f;
        m_PlayerMovementScript.m_Speed *= m_CurseCoeff;
    }

    private IEnumerator DisactivateCurse(float time)
    {
        yield return new WaitForSeconds(time);
        
        m_IsCurseActive = false;
        m_CurseTimeObj.SetActive(false);

        // восстанавливаем некоторые изначальные значения
        m_PlayerMovementScript.m_Speed = m_PlayerSpeedSave; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Curse" && !m_IsCurseActive)
            ActivateCurse();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Curse")
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Curse")
        {
            StartCoroutine(DisactivateCurse(5f));
        }
    }

}
