using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Vector2 position;
    public float lerpSpeed = 0.125f;
    public float walkDist = 10;

    public Fighter fighter;
    public GameObject remainsPrefab;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.Instance.moveEvent += Move;

        position = new Vector2((int)transform.position.x, (int)transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3((int)position.x, 0, (int)position.y), lerpSpeed);
    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.moveEvent -= Move;
    }

    public void Die()
    {
        GUIManager.Instance.Print("<color=#" + ColorUtility.ToHtmlStringRGB(fighter.nameColor) + ">" + fighter.gameName + "</color> perished.");
        GUIManager.Instance.Print("You gain " + fighter.xp + " XP.");
        Instantiate(remainsPrefab, transform.position, remainsPrefab.transform.rotation);
        PlayerManager.Instance.xp += fighter.xp;

        Destroy(gameObject);
    }

    void Move()
    {
        if (Vector2.Distance(position, PlayerMovement.Instance.position) > walkDist) return;

        Vector2 newPos = Vector2.zero;

        //Choose to move on x-axis or y-axis
        if (Random.Range(0, 2) == 0)
        {
            //Moving
            if (position.x - PlayerMovement.Instance.position.x != 0)
            {
                if (position.x > PlayerMovement.Instance.position.x) newPos = Vector2.left;
                else newPos = Vector2.right;
            }
            else if (position.y - PlayerMovement.Instance.position.y != 0)
            {
                if (position.y > PlayerMovement.Instance.position.y) newPos = Vector2.down;
                else newPos = Vector2.up;
            }
        }
        else
        {
            if (position.y - PlayerMovement.Instance.position.y != 0)
            {
                if (position.y > PlayerMovement.Instance.position.y) newPos = Vector2.down;
                else newPos = Vector2.up;
            }
            else if (position.x - PlayerMovement.Instance.position.x != 0)
            {
                if (position.x > PlayerMovement.Instance.position.x) newPos = Vector2.left;
                else newPos = Vector2.right;
            }
        }

        if (MapGenerator.Instance.EntityAtPos((int)(position.x + newPos.x), (int)(position.y + newPos.y)) != null) return;

        //Attacking if close to player
        if (newPos + position == PlayerMovement.Instance.position)
        {
            fighter.Attack(PlayerMovement.Instance.GetComponent<Fighter>());
        }

        else if (MapGenerator.Instance.GetTile((int)(position.x + newPos.x), (int)(position.y + newPos.y)) == 0)
            position = position + newPos;
    }
}
