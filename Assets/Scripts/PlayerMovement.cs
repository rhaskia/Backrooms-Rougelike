using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public Action moveEvent;
    public UnityEvent unityMoveEvent;
    Fighter fighter;
    Inventory inventory;

    public Vector2 position;
    public float lerpSpeed;
    MapGenerator mapGen;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        mapGen = FindObjectOfType<MapGenerator>();
        fighter = GetComponent<Fighter>();
        inventory = GetComponent<Inventory>();
    }

    // Update is called once per frame
    void Update()
    {
        //WASD
        if (Input.GetKeyDown(KeyCode.W)) Move(0, 1);
        if (Input.GetKeyDown(KeyCode.S)) Move(0, -1);
        if (Input.GetKeyDown(KeyCode.A)) Move(-1, 0);
        if (Input.GetKeyDown(KeyCode.D)) Move(1, 0);

        //Numpad Keys
        if (Input.GetKeyDown(KeyCode.Keypad8)) Move(0, 1);
        if (Input.GetKeyDown(KeyCode.Keypad2)) Move(0, -1);
        if (Input.GetKeyDown(KeyCode.Keypad4)) Move(-1, 0);
        if (Input.GetKeyDown(KeyCode.Keypad6)) Move(1, 0);

        if (Input.GetKeyDown(KeyCode.Keypad7)) Move(-1, -1);
        if (Input.GetKeyDown(KeyCode.Keypad9)) Move(1, -1);
        if (Input.GetKeyDown(KeyCode.Keypad1)) Move(-1, 1);
        if (Input.GetKeyDown(KeyCode.Keypad3)) Move(1, 1);

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, 0, position.y), lerpSpeed);
    }

    void Move(int x, int y)
    {
        if (moveEvent != null) Invoke("MoveEnemies", .25f);

        Vector2Int newPosition = new Vector2Int((int)(position.x + x), (int)(position.y + y));

        if (MapGenerator.Instance.GetTile(newPosition.x, newPosition.y) == 1) return;
        if (MapGenerator.Instance.GetTile(newPosition.x, newPosition.y) == 2) ClimbLadder();

        if (MapGenerator.Instance.EntityAtPos(newPosition.x, newPosition.y) != null)
        {
            fighter.Attack(MapGenerator.Instance.EntityAtPos(newPosition.x, newPosition.y));
            return;
        }

        //Picking up items
        if (MapGenerator.Instance.ItemAtPos(newPosition.x, newPosition.y) != null)
        {
            inventory.PickUp(MapGenerator.Instance.ItemAtPos(newPosition.x, newPosition.y));
        }

        position = newPosition;

        mapGen.GenerateNew((int)position.x, (int)position.y, x, y);

        unityMoveEvent.Invoke();
    }

    void ClimbLadder()
    {
        MapGenerator.Instance.floor++;
        MapGenerator.Instance.level = MapGenerator.Instance.l1;
        MapGenerator.Instance.MapGeneration((int)position.x, (int)position.y);

        GUIManager.Instance.Print("<color=yellow>You enter a new level, and take a moment of rest...</color>");
        fighter.health += ((int)fighter.health / 2);
    }

    void MoveEnemies()
    {
        if (moveEvent != null) moveEvent.Invoke();
    }
}
