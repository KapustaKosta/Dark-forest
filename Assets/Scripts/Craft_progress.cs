using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft_progress : MonoBehaviour
{
    // Start is called before the first frame update
    private float start_width;
    public GameObject front;

    private RectTransform rectTransfrom;


    void Start()
    {
        rectTransfrom = front.GetComponent<RectTransform>();

        // rectTransfrom.sizeDelta = new Vector2(50, 50); // задаем размер   new Vectro2 (width, height)

        start_width = rectTransfrom.sizeDelta.x;

        Debug.Log(start_width);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateScale(int percent)
    {
        rectTransfrom.sizeDelta = new Vector2(percent * start_width / 50, 25);
    }
}
