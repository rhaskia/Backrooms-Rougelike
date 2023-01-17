using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance;

    [Header("Stats")]
    public TextMeshProUGUI hp;
    public TextMeshProUGUI defence, power, messagesText, poison, bleeding;

    [Header("Sub Menu")]
    public RectTransform menu;
    public float menuSpeed;
    public bool menuOpen;
    public GameObject levels, inventory, equipment;
    public GameObject itemSlot;

    [Header("XP")]
    public Slider xpSlider;
    public TextMeshProUGUI points;
    public GameObject upgrades;

    PlayerManager p_manager;
    Fighter p_fighter;
    public string messages;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        p_fighter = PlayerMovement.Instance.GetComponent<Fighter>();
        p_manager = PlayerMovement.Instance.GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        hp.text = p_fighter.health.ToString() + "/" + p_fighter.maxHealth.ToString();
        defence.text = p_fighter.defense.ToString();
        power.text = p_fighter.power.ToString();

        points.text = "points - " + p_manager.points.ToString();
        xpSlider.value = p_manager.xp;
        xpSlider.maxValue = p_manager.level * 150 + 200;

        upgrades.SetActive(p_manager.points > 0);

        messagesText.text = messages;

        poison.gameObject.SetActive(p_fighter.poison.afflicted);
        poison.text = "poisoned - " + p_fighter.poison.afflictionLeft;

        bleeding.gameObject.SetActive(p_fighter.bleeding.afflicted);
        bleeding.text = "bleeding - " + p_fighter.bleeding.afflictionLeft;

        //Opening sub menus
        if (Input.GetKeyDown(KeyCode.L))
        {
            menuOpen = !menuOpen;

            if (menuOpen)
            {
                levels.SetActive(true);
                inventory.SetActive(false);
                equipment.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            menuOpen = !menuOpen;

            if (menuOpen)
            {
                levels.SetActive(false);
                inventory.SetActive(true);
                equipment.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            menuOpen = !menuOpen;

            if (menuOpen)
            {
                levels.SetActive(false);
                inventory.SetActive(false);
                equipment.SetActive(true);
            }
        }

        menu.anchoredPosition = Vector3.MoveTowards(menu.anchoredPosition, new Vector2(-97.5f, menuOpen ? 36.85f : -36.85f), menuSpeed);
    }

    public void OpenLevels()
    {
        menuOpen = true;
        levels.SetActive(true);
        inventory.SetActive(false);
    }

    public void Print(string message)
    {
        messages = messages + "\n" + message.ToLower();
    }
}
