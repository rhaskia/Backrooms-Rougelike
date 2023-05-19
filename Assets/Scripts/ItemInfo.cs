using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public int currItem;
    public TextMeshProUGUI nameText, descText;
    Inventory inventory;
    public bool over;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (currItem >= inventory.inventory.Count) Invoke("TurnOff", 0.1f);
        else
        {
            if (currItem != -1)
            {
                nameText.text = "<color=#" + ColorUtility.ToHtmlStringRGB(inventory.inventory[currItem].nameColor) + ">" + inventory.inventory[currItem].name.ToLower() + "</color>";
                descText.text = inventory.inventory[currItem].description;

                if (Input.GetMouseButtonDown(0)) UseItem();
                if (Input.GetMouseButtonDown(1)) DropItem();
            }
            else gameObject.SetActive(false);
        }

        Vector3 newPos = Input.mousePosition + offset;
        newPos.y = Mathf.Clamp(newPos.y, 22, 1000);
        transform.position = newPos;
    }

    public void UseItem()
    {
        inventory.UseItem(currItem);
        Invoke("TurnOff", 0.1f);
    }

    public void DropItem()
    {
        inventory.DropItem(currItem);
        Invoke("TurnOff", 0.1f);
    }
}
