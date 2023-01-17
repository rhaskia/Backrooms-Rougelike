using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public struct Affliction
{
    public int afflictionDamage, afflictionLeft;
    public bool afflicted => afflictionLeft > 0;
}

public class Fighter : MonoBehaviour
{
    public bool canAttack = true;

    public string gameName;
    public Color nameColor;

    //Stats
    public float health;
    public float maxHealth;

    public float defense;
    public float power;
    public int critRarity = 10;
    public float critMult = 1.5f;

    //Afflictions
    [HideInInspector]
    public Affliction poison, bleeding;

    //Death
    public UnityEvent deathFunction;

    public float xp;

    //UI
    [HideInInspector]
    public float lastAttacked;
    public TextMeshProUGUI damageText;
    public float fadeTimeDMG;
    public Color DMGColor, critColor, poisonCol;

    public int killCount;

    public bool poisons, bleeds;

    // Start is called before the first frame update
    void Start()
    {
        lastAttacked = -fadeTimeDMG;


    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        damageText.color = new Color(damageText.color.r, damageText.color.b, damageText.color.g, (fadeTimeDMG - (Time.time - lastAttacked)) / fadeTimeDMG);

        if (health == 0) deathFunction.Invoke();
    }

    public void Attack(Fighter victim)
    {
        int damage = (int)Mathf.Clamp((power - victim.defense), 0, 10000);

        victim.health = Mathf.Clamp(victim.health - damage, 0, 10000);

        //Critical Hits
        if (Random.Range(0, critRarity) == 0)
        {
            damage = (int)(critMult * damage);
            victim.damageText.color = victim.critColor;
        }
        else victim.damageText.color = victim.DMGColor;

        //Damage Text
        victim.lastAttacked = Time.time;
        victim.damageText.text = damage.ToString();

        //Poisoning
        if (poisons)
        {
            victim.Poison(2, 6, "<color=#" + ColorUtility.ToHtmlStringRGB(poisonCol) + ">poison seeps into your bloodstream</color>");
        }

        if (bleeds)
        {
            victim.Poison(1, 5, "<color=#" + ColorUtility.ToHtmlStringRGB(critColor) + ">you start to bleed</color>");
        }

        //Printing
        if (damage > 0)
        {
            GUIManager.Instance.Print("<color=#" + ColorUtility.ToHtmlStringRGB(nameColor) + ">" + gameName + "</color> attacked <color=#" + ColorUtility.ToHtmlStringRGB(victim.nameColor) + ">" + victim.gameName + "</color>");
        }

        if (victim.health == 0) victim.deathFunction.Invoke();
    }

    public void Heal(int toHeal)
    {
        if (toHeal < 1) return;

        float oldH = health;

        health = Mathf.Clamp(health + toHeal, 0, maxHealth);

        if (health - oldH > 0) GUIManager.Instance.Print("<color=#45ffff>You regain " + (health - oldH).ToString() + " health.</color>");
        else GUIManager.Instance.Print("It has no effect.");
    }

    public void ManageAfflictions()
    {
        if (poison.afflicted)
        {
            health -= poison.afflictionDamage;
            lastAttacked = Time.time;
            damageText.color = poisonCol;
            poison.afflictionLeft--;
        }

        if (bleeding.afflicted)
        {
            health -= bleeding.afflictionDamage;
            lastAttacked = Time.time;
            damageText.color = critColor;
            bleeding.afflictionLeft--;
        }
    }

    //Poisons
    public void Poison(int damage, int time, string message)
    {
        if (!poison.afflicted) GUIManager.Instance.Print(message);

        if (poison.afflictionLeft < time) poison.afflictionLeft = time;
        poison.afflictionDamage = damage;
    }

    //Bleed
    public void Bleed(int damage, int time, string message)
    {
        if (!bleeding.afflicted) GUIManager.Instance.Print(message);

        if (bleeding.afflictionLeft < time) bleeding.afflictionLeft = time;
        bleeding.afflictionDamage = damage;
    }
}
