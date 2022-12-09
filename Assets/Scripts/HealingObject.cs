using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingObject : MonoBehaviour
{
    public CurseMechanics m_CurseMechanics;
    private SpriteRenderer m_SR;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Damage")
        {
            m_SR = GetComponent<SpriteRenderer>();
            m_SR.color = new Vector4(0f, 0f, 0f, 0f);
            StartCoroutine(m_CurseMechanics.DisactivateCurse());
        }
    }
}
