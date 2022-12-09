using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startGrow : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isGrowing;
    public float maxSize = 1.1f;
    private float size = 1f;
    private Vector3 scale;
    private float delta;

    void Start()
    {
        scale = transform.localScale;
        delta = (maxSize - size) / 10;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrowing) {
            size += delta;
            transform.localScale = new Vector3(scale.x * size, scale.y * size, scale.z);
        }

        if (size > maxSize) {
            Destroy(gameObject);
        }
    }

    public void grow() 
    {
        isGrowing = true;
        
    }
}
