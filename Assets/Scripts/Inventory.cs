using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> inventory = new List<Item>();
    public Transform MapGen;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UseItem(int n)
    {
        PlayerManager.Instance.UseItem(inventory[n]);

        if (inventory[n].use != 0) inventory.RemoveAt(n);
    }

    public void DropItem(int n)
    {
        GameObject item = Instantiate(inventory[n].prefab, MapGen);
        item.transform.position = PlayerMovement.Instance.transform.position;

        inventory.RemoveAt(n);
    }

    public void PickUp(ItemHolder item)
    {
        inventory.Add(item.item);

        if (isVowel(item.item.name.ToLower()[0]))
        {
            GUIManager.Instance.Print("You picked up an <color=#" + ColorUtility.ToHtmlStringRGB(item.item.nameColor) + ">" + item.item.name + "</color>");
        }
        else
        {
            GUIManager.Instance.Print("You picked up a <color=#" + ColorUtility.ToHtmlStringRGB(item.item.nameColor) + ">" + item.item.name + "</color>");
        }

        Destroy(item.gameObject);
    }

    bool isVowel(char c)
    {
        return c == 'a' || c == 'e' || c == 'i' || c == 'o' || c == 'u';
    }
}
