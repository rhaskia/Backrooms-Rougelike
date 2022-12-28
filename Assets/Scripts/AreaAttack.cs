using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    AIMovement ai;
    public float minDist;
    public Vector2 damageMap;
    public string hurtMessage;
    bool attacked;

    void Start()
    {
        ai = GetComponent<AIMovement>();
    }

    public void OnMove()
    {
        if (Vector2.Distance(ai.position, PlayerMovement.Instance.position) <= minDist)
        {
            if (!attacked) GUIManager.Instance.Print(hurtMessage);
            attacked = true;

            Fighter player = PlayerManager.Instance.GetComponent<Fighter>();

            player.health -= (int)Mathf.Lerp(damageMap.x, damageMap.y, Vector2.Distance(ai.position, PlayerMovement.Instance.position) / minDist);
            player.damageText.text = ((int)Mathf.Lerp(damageMap.x, damageMap.y, Vector2.Distance(ai.position, PlayerMovement.Instance.position) / minDist)).ToString();
            player.lastAttacked = Time.time;
        }
        else attacked = false;
    }
}
