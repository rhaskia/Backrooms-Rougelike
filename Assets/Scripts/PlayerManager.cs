using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    Fighter fighter;
    Inventory inventory;
    Equipment equipment;

    public float xp;
    public int level = 1;
    public int points;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        fighter = GetComponent<Fighter>();
        inventory = GetComponent<Inventory>();
        equipment = GetComponent<Equipment>();
    }

    // Update is called once per frame
    void Update()
    {
        if (xp > 200 + 150 * level)
        {
            GUIManager.Instance.Print("<color=#45ffff>You grow stronger...</color>");
            GUIManager.Instance.OpenLevels();

            points++;
            xp -= 200 + 150 * level;
        }
    }

    public void UseItem(Item item)
    {
        if (item.use == 0) { GUIManager.Instance.Print("Item has no use."); }

        //Healing
        if (item.use == 1)
        {
            fighter.Heal((int)item.strength);
            GUIManager.Instance.Print("You use the <color=#" + ColorUtility.ToHtmlStringRGB(item.nameColor) + ">" + item.name + "</color>");
        }

        //Equiping Armour
        if (item.use == 2)
        {
            equipment.EquipArmour(item);
        }

        //Equiping Weapons
        if (item.use == 3)
        {
            equipment.EquipWeapon(item);
        }
    }

    public void Upgrade(int u)
    {
        if (u == 0) { fighter.maxHealth += 20; fighter.health += 20; }
        if (u == 1) { fighter.power++; }
        if (u == 2) { fighter.defense++; }

        points--;

        if (points < 1) GUIManager.Instance.menuOpen = false;
    }
}
