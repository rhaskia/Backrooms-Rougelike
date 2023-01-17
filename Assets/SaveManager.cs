using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SaveManager : MonoBehaviour
{
    public bool inGame;

    Inventory inventory;
    public Item[] items;

    private void Awake()
    {
        if (inGame) inventory = FindObjectOfType<Inventory>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (inGame)
        {
            Load();
            InvokeRepeating("Save", 1f, 1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Save()
    {
        //if you see this it's because im lazy and cant be bothered with json
        PlayerPrefs.SetInt("floor", MapGenerator.Instance.floor);

        PlayerPrefs.SetInt("x", (int)PlayerMovement.Instance.position.x);
        PlayerPrefs.SetInt("y", (int)PlayerMovement.Instance.position.y);

        PlayerPrefs.SetInt("level", PlayerManager.Instance.level);
        PlayerPrefs.SetInt("xp", (int)PlayerManager.Instance.xp);
        PlayerPrefs.SetInt("points", PlayerManager.Instance.points);

        for (int i = 0; i < 8; i++)
        {
            if (inventory.inventory.Count > i) PlayerPrefs.SetString("item" + i.ToString(), inventory.inventory[i].name);
            else PlayerPrefs.SetString("item" + i.ToString(), "null");
        }

        PlayerPrefs.SetInt("Titems", inventory.totalItems);
        PlayerPrefs.SetInt("Tkills", PlayerMovement.Instance.fighter.killCount);

        PlayerPrefs.SetInt("health", (int)PlayerMovement.Instance.fighter.health);
        PlayerPrefs.SetInt("maxhealth", (int)PlayerMovement.Instance.fighter.maxHealth);

        PlayerPrefs.SetInt("defense", (int)PlayerMovement.Instance.fighter.defense);
        PlayerPrefs.SetInt("power", (int)PlayerMovement.Instance.fighter.power);

        PlayerPrefs.SetString("chat", GUIManager.Instance.messages);

        PlayerPrefs.SetInt("pleft", PlayerMovement.Instance.fighter.poison.afflictionLeft);
        PlayerPrefs.SetInt("pdmg", PlayerMovement.Instance.fighter.poison.afflictionDamage);

        PlayerPrefs.SetInt("bleft", PlayerMovement.Instance.fighter.bleeding.afflictionLeft);
        PlayerPrefs.SetInt("bdmg", PlayerMovement.Instance.fighter.bleeding.afflictionDamage);

        PlayerPrefs.Save();
    }

    public void Load()
    {
        FindObjectOfType<MapGenerator>().floor = PlayerPrefs.GetInt("floor", 0);

        Random.InitState(DateTime.Now.Second);
        var x = PlayerPrefs.GetInt("x", Random.Range(-10000, 10000));
        var y = PlayerPrefs.GetInt("y", Random.Range(-10000, 10000));
        FindObjectOfType<PlayerMovement>().position = new Vector2(x, y);

        PlayerManager pm = FindObjectOfType<PlayerManager>();
        pm.level = PlayerPrefs.GetInt("level", 0);
        pm.xp = PlayerPrefs.GetInt("xp", 0);
        pm.points = PlayerPrefs.GetInt("points", 0);

        for (int i = 0; i < 8; i++)
        {
            var item = StringToItem(PlayerPrefs.GetString("item" + i.ToString(), "null"));
            if (item != null) inventory.inventory.Add(item);
        }

        inventory.totalItems = PlayerPrefs.GetInt("Titems", 0);
        FindObjectOfType<PlayerMovement>().fighter.killCount = PlayerPrefs.GetInt("Tkills", 0);

        PlayerMovement.Instance.fighter.health = PlayerPrefs.GetInt("health", 100);
        PlayerMovement.Instance.fighter.maxHealth = PlayerPrefs.GetInt("maxhealth", 100);

        PlayerMovement.Instance.fighter.defense = PlayerPrefs.GetInt("defense", (int)PlayerMovement.Instance.fighter.defense);
        PlayerMovement.Instance.fighter.power = PlayerPrefs.GetInt("power", (int)PlayerMovement.Instance.fighter.power);

        PlayerMovement.Instance.fighter.poison.afflictionLeft = PlayerPrefs.GetInt("pleft", 0);
        PlayerMovement.Instance.fighter.poison.afflictionDamage = PlayerPrefs.GetInt("pdmg", 0);

        PlayerMovement.Instance.fighter.bleeding.afflictionLeft = PlayerPrefs.GetInt("bleft", 0);
        PlayerMovement.Instance.fighter.bleeding.afflictionDamage = PlayerPrefs.GetInt("bdmg", 0);

        GUIManager.Instance.messages = PlayerPrefs.GetString("chat", "");
    }

    public void Reset()
    {
        PlayerPrefs.DeleteKey("floor");

        PlayerPrefs.DeleteKey("x");
        PlayerPrefs.DeleteKey("y");

        PlayerPrefs.DeleteKey("level");
        PlayerPrefs.DeleteKey("xp");
        PlayerPrefs.DeleteKey("points");

        for (int i = 0; i < 8; i++)
        {
            PlayerPrefs.DeleteKey("item" + i.ToString());
        }

        PlayerPrefs.DeleteKey("Titems");
        PlayerPrefs.DeleteKey("Tkills");

        PlayerPrefs.DeleteKey("health");
        PlayerPrefs.DeleteKey("maxhealth");

        PlayerPrefs.DeleteKey("defense");
        PlayerPrefs.DeleteKey("power");

        PlayerPrefs.DeleteKey("chat");

        PlayerPrefs.DeleteKey

        PlayerPrefs.Save();
    }

    Item StringToItem(string name)
    {
        foreach (var item in items)
        {
            if (item.name.ToLower() == name.ToLower()) return item;
        }

        return null;
    }
}
