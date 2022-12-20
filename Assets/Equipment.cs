using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    Inventory inventory;
    Fighter fighter;
    public Item armour, weapon;

    // Start is called before the first frame update
    void Start()
    {
        inventory = FindObjectOfType<Inventory>();
        fighter = GetComponent<Fighter>();
    }

    // Update is called once per frame
    void Update()
    {

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

    public void Unequip(int n)
    {
        if (n == 0)
        {
            GUIManager.Instance.Print("You unequipped the <color=#" + ColorUtility.ToHtmlStringRGB(weapon.nameColor) + ">" + weapon.name + "</color>");
            GUIManager.Instance.Print("You lost " + weapon.strength + " attack.");

            weapon = null;
        }

        if (n == 0)
        {
            GUIManager.Instance.Print("You unequipped the <color=#" + ColorUtility.ToHtmlStringRGB(armour.nameColor) + ">" + armour.name + "</color>");
            GUIManager.Instance.Print("You lost " + armour.strength + " defense.");

            armour = null;
        }
    }
}
