using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingObject : MonoBehaviour
{
    public CurseMechanics m_CurseMechanics;

    private void OnMouseDown()
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = new Vector4(0f, 0f, 0f, 0f);
        m_CurseMechanics.DisactivateCurse();
    }
}
