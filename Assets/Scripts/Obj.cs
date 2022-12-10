using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obj : MonoBehaviour
{
    // Start is called before the first frame update
    public Inventory inventory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        startGrow s = other.GetComponent<startGrow>();
        if (s != null)
        {
            if(other.tag == "wood") {
                inventory.wood_number++;
                other.tag = "Taken";
                s.grow();
            }
            else if(other.tag == "resin") {
                inventory.resin_number++;
                other.tag = "Taken";
                s.grow();
           }

        }
    }
}
