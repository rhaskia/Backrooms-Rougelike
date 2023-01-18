using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    public static Equipment Instance;
    Inventory inventory;
    Fighter fighter;
    public Item armour, weapon, tool;

    public TextMeshProUGUI arText, weText, toText;
    public Image arImage, weImage, toImage;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        inventory = FindObjectOfType<Inventory>();
        fighter = GetComponent<Fighter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (armour != null) arImage.sprite = armour.sprite;
        arImage.enabled = armour != null;

        if (weapon != null) weImage.sprite = weapon.sprite;
        weImage.enabled = weapon != null;

        if (tool != null) toImage.sprite = tool.sprite;
        toImage.enabled = tool != null;

        arText.text = "+" + (armour == true ? armour.strength : "0");
        weText.text = "+" + (weapon == true ? weapon.strength : "0");
        toText.text = "+" + (tool == true ? tool.strength : "0");
    }

    public void EquipArmour(Item _armour)
    {
        int extra = 0;

        if (armour != null)
        {
            inventory.inventory.Add(armour);
            extra = (int)(_armour.strength - armour.strength);
            fighter.defense -= armour.strength;
        }

        GUIManager.Instance.Print("You equipped the <color=#" + ColorUtility.ToHtmlStringRGB(_armour.nameColor) + ">" + _armour.name + "</color>");
        GUIManager.Instance.Print("You gained " + extra + " defense.");

        fighter.defense += _armour.strength;

        armour = _armour;
    }

    public void EquipWeapon(Item _weapon)
    {
        int extra = 0;

        if (weapon != null)
        {
            inventory.inventory.Add(weapon);
            extra = (int)(_weapon.strength - weapon.strength);
            fighter.power -= weapon.strength;
        }

        GUIManager.Instance.Print("You equipped the <color=#" + ColorUtility.ToHtmlStringRGB(_weapon.nameColor) + ">" + _weapon.name + "</color>");
        GUIManager.Instance.Print("You gained " + extra + " attack.");

        fighter.power += _weapon.strength;

        weapon = _weapon;
    }

    public void EquipTool(Item _tool)
    {
        if (tool != null)
        {
            inventory.inventory.Add(tool);
        }

        GUIManager.Instance.Print("You equipped the <color=#" + ColorUtility.ToHtmlStringRGB(_tool.nameColor) + ">" + _tool.name + "</color>");

        tool = _tool;
    }

    public void Unequip(int n)
    {
        if (n == 0)
        {
            if (weapon == null) return;

            GUIManager.Instance.Print("You unequipped the <color=#" + ColorUtility.ToHtmlStringRGB(weapon.nameColor) + ">" + weapon.name + "</color>");
            GUIManager.Instance.Print("You lost " + weapon.strength + " attack.");

            weapon = null;
        }

        if (n == 1)
        {
            if (armour == null) return;

            GUIManager.Instance.Print("You unequipped the <color=#" + ColorUtility.ToHtmlStringRGB(armour.nameColor) + ">" + armour.name + "</color>");
            GUIManager.Instance.Print("You lost " + armour.strength + " defense.");

            armour = null;
        }
    }
}
