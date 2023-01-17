using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public GameObject deathMenu;
    public TextMeshProUGUI deathText;
    public bool dead;

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
            GUIManager.Instance.Print("<color=#72a5b1>You grow stronger...</color>");
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

        //Equiping Tools
        if (item.use == 4)
        {
            equipment.EquipTool(item);
        }

        //Moth Jelly
        if (item.use == 5)
        {
            PlayerMovement.Instance.dashes += (int)item.strength;
            GUIManager.Instance.Print("You use the <color=#" + ColorUtility.ToHtmlStringRGB(item.nameColor) + ">" + item.name + "</color>");
            GUIManager.Instance.Print("You feel extremely energetic");
        }

        //Harm
        if (item.use == 6)
        {
            if (equipment.tool.name == "Squirt Gun")
            {

            }
            else
            {
                fighter.health -= item.strength;
                GUIManager.Instance.Print("You use the <color=#" + ColorUtility.ToHtmlStringRGB(item.nameColor) + ">" + item.name + "</color>");
                GUIManager.Instance.Print("<color=#cb593a>You take " + item.strength.ToString() + " damage.</color>");
            }
        }

        //Antidotes
        if (item.use == 7)
        {
            fighter.poison.afflictionLeft = 0;
            fighter.bleeding.afflictionLeft = 0;
            GUIManager.Instance.Print("You use the <color=#" + ColorUtility.ToHtmlStringRGB(item.nameColor) + ">" + item.name + "</color>");
            GUIManager.Instance.Print("You feel much better.");
        }

        //Sanity Increase
        if (item.use == 8)
        {
            //sanity
        }

        //Defense Increase
        if (item.use == 9)
        {
            fighter.defense += item.strength;
            GUIManager.Instance.Print("You feel much stronger.");
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

    public void Death()
    {
        dead = true;

        deathMenu.SetActive(true);
        deathText.text = "you made it to level " + MapGenerator.Instance.floor.ToString() + "\n\nyou had " + fighter.maxHealth.ToString() + " health\n" + fighter.power.ToString() + " power, " + fighter.defense.ToString() + " defense\n\n"
            + "you defeated " + fighter.killCount + " enemies\nand gathered " + inventory.totalItems + " items";
    }
}
