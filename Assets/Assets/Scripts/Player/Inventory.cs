using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    private Text resin_text;
    private Text shield_text;
    private Text torch_text;
    private Text wood_text;

    public int start_resin_number = 10;
    public int start_shield_number = 0;
    public int start_torch_number = 0;
    public int start_wood_number = 10;

    private int resin_number;
    private int shield_number;
    private int torch_number;
    private int wood_number;

    private int torch_crafting;
    private int shield_crafting;

    public GameObject scaleObject;
    private Craft_progress scale;

    private Color Grey = new Color(0.5f, 0.5f, 0.5f);

    private bool entered = false;

    void Start()
    {
        resin_text = GameObject.Find("Canvas/resin_number").GetComponent<Text>();
        shield_text = GameObject.Find("Canvas/shield_number").GetComponent<Text>();
        torch_text = GameObject.Find("Canvas/torch_number").GetComponent<Text>();
        wood_text = GameObject.Find("Canvas/wood_number").GetComponent<Text>();

        resin_number = start_resin_number;
        shield_number = start_shield_number;
        torch_number = start_torch_number;
        wood_number = start_wood_number;

        shield_crafting = 0;
        torch_crafting = 0;

        scale = scaleObject.GetComponent<Craft_progress>();
    }

    // Update is called once per frame
    void Update()
    {
        Update_labels();
    }

    void FixedUpdate()
    {
        float torch = Input.GetAxisRaw("Craft torch");
        float shield = Input.GetAxisRaw("Craft shield");

        if (torch == 1)
        {
            if (CanCraftShield() && !entered)
            {
                torch_crafting++;
                shield = 0;
                scaleObject.gameObject.SetActive(true);
                scale.UpdateScale(torch_crafting);
            }
            else
            {
                wood_text.color = new Color(255, 0, 0);
                resin_text.color = new Color(255, 0, 0);
            }
        }
        else
        {
            torch_crafting = 0;
        }
        if (shield == 1)
        {
            if (CanCraftShield() && !entered)
            {
                scaleObject.gameObject.SetActive(true);
                shield_crafting++;
                scale.UpdateScale(shield_crafting);
            }
            else
            {
                wood_text.color = new Color(255, 0, 0);
            }

        }
        else
        {
            shield_crafting = 0;
        }

        if (torch == 0 && shield == 0) {
            scaleObject.gameObject.SetActive(false);
            entered = false;
        }

        if (torch_crafting == 50)
        {
            torch_crafting = 0;
            Craft_torch();
            entered = true;
            scaleObject.gameObject.SetActive(false);
        }

        if (shield_crafting == 50)
        {
            shield_crafting = 0;
            Craft_shield();
            entered = true;
            scaleObject.gameObject.SetActive(false);
        }
    }

    void Update_labels()
    {
        wood_text.color = new Color(255, 255, 255);
        resin_text.color = new Color(255, 255, 255);
        torch_text.color = new Color(255, 255, 255);
        shield_text.color = new Color(255, 255, 255);

        if (torch_crafting >= 1)
        {
            wood_text.text = wood_number.ToString() + "-1";
            wood_text.color = Grey;

            torch_text.text = torch_number.ToString() + "+1";
            torch_text.color = Grey;

            resin_text.text = resin_number.ToString() + "-1";
            resin_text.color = Grey;

        }
        else if (shield_crafting >= 1)
        {
            wood_text.text = wood_number.ToString() + "-2";
            wood_text.color = Grey;

            shield_text.text = shield_number.ToString() + "+1";
            shield_text.color = Grey;
        }
        else
        {
            resin_text.text = resin_number.ToString();
            shield_text.text = shield_number.ToString();
            torch_text.text = torch_number.ToString();
            wood_text.text = wood_number.ToString();
        }

        float torch = Input.GetAxisRaw("Craft torch");
        float shield = Input.GetAxisRaw("Craft shield");

        if (shield == 1 && !CanCraftShield()) 
        {
            wood_text.color = new Color(255, 0, 0);
        }
        if (torch == 1 && !CanCraftTorch())
        {
            if (wood_number == 0)
            {
                wood_text.color = new Color(255, 0, 0);
            }

            if (resin_number == 0)
            {
                resin_text.color = new Color(255, 0, 0);
            }
        }
    }

    void Craft_shield()
    {
        wood_number -= 2;
        shield_number++;
    }

    void Craft_torch()
    {
        wood_number --;
        resin_number--;
        torch_number++;
    }

    bool CanCraftShield()
    {
        if (wood_number >= 2)
        {
            return true;
        }
        return false;
    }

    bool CanCraftTorch()
    {
        if (wood_number >= 1 && resin_number >= 1)
        {
            return true;
        }
        return false;
    }
}
