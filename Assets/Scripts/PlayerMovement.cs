using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    public Action moveEvent;
    public UnityEvent unityMoveEvent, unityHitEvent, unityPickupEvent;
    Fighter fighter;
    Inventory inventory;

    public Vector2 position;
    public float moveDelay;
    float[] tilMove = new float[4];
    public float lerpSpeed;
    MapGenerator mapGen;

    [Header("Dash")]
    public int dashWait;
    int movesLeft;
    public int dashes;
    public TextMeshProUGUI dashText;

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
        //Dash UI
        dashText.color = dashes > 0 ? Color.white : Color.grey;

        //WASD
        //Holding down keys
        if (Input.GetKey(KeyCode.W) && tilMove[0] < 0) { Move(0, 1); tilMove[0] = moveDelay; }
        if (Input.GetKey(KeyCode.S) && tilMove[1] < 0) { Move(0, -1); tilMove[1] = moveDelay; }
        if (Input.GetKey(KeyCode.A) && tilMove[2] < 0) { Move(-1, 0); tilMove[2] = moveDelay; }
        if (Input.GetKey(KeyCode.D) && tilMove[3] < 0) { Move(1, 0); tilMove[3] = moveDelay; }

        //Numpad Keys
        //if (Input.GetKeyDown(KeyCode.Keypad8)) Move(0, 1);
        //if (Input.GetKeyDown(KeyCode.Keypad2)) Move(0, -1);
        //if (Input.GetKeyDown(KeyCode.Keypad4)) Move(-1, 0);
        //if (Input.GetKeyDown(KeyCode.Keypad6)) Move(1, 0);

        //if (Input.GetKeyDown(KeyCode.Keypad7)) Move(-1, -1);
        //if (Input.GetKeyDown(KeyCode.Keypad9)) Move(1, 1);
        //if (Input.GetKeyDown(KeyCode.Keypad1)) Move(-1, 1);
        //if (Input.GetKeyDown(KeyCode.Keypad3)) Move(1, -1);

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(position.x, 0, position.y), lerpSpeed);

        //Move delays
        for (int i = 0; i < tilMove.Length; i++)
        {
            tilMove[i] -= Time.deltaTime;
        }
    }

    void Move(int x, int y)
    {
        if (moveEvent != null) Invoke("MoveEnemies", .25f);

        //Dashing
        movesLeft--;
        if (movesLeft < 1 && dashes == 0) dashes++;

        bool canDash = Input.GetKey(KeyCode.LeftShift) && dashes > 0;
        if (canDash)
        {
            dashes--;
            movesLeft = dashWait;
        }

        Vector2Int newPosition = new Vector2Int((int)(position.x + x * (canDash ? 2 : 1)), (int)(position.y + y * (canDash ? 2 : 1)));

        //Walls
        if (MapGenerator.Instance.GetTile(newPosition.x, newPosition.y) == 1) return;

        //Ladders/Elevators etc
        if (MapGenerator.Instance.GetTile(newPosition.x, newPosition.y) == 2) ClimbLadder();

        //Holes/Doors
        if (MapGenerator.Instance.GetTile(newPosition.x, newPosition.y) == 3) { FallInHole(); return; }

        //Attacking
        if (MapGenerator.Instance.EntityAtPos(newPosition.x, newPosition.y) != null)
        {
            fighter.Attack(MapGenerator.Instance.EntityAtPos(newPosition.x, newPosition.y));
            unityHitEvent.Invoke();
            return;
        }

        //Picking up items
        if (MapGenerator.Instance.ItemAtPos(newPosition.x, newPosition.y) != null)
        {
            inventory.PickUp(MapGenerator.Instance.ItemAtPos(newPosition.x, newPosition.y));
            unityPickupEvent.Invoke();
        }

        //Setting the position
        position = newPosition;

        mapGen.GenerateNew((int)position.x, (int)position.y, x, y);

        unityMoveEvent.Invoke();
    }

    void ClimbLadder()
    {
        //Bunch of map Generation stuff
        MapGenerator.Instance.floor++;
        MapGenerator.Instance.level = MapGenerator.Instance.l1;
        MapGenerator.Instance.MapGeneration((int)position.x, (int)position.y);

        GUIManager.Instance.Print("<color=yellow>You enter a new level, and take a moment of rest...</color>");
        fighter.health += ((int)fighter.health / 2);
    }

    void FallInHole()
    {
        //Bunch of map Generation stuff
        UnityEngine.Random.InitState(MapGenerator.CantorPair((int)position.x, (int)position.y));
        position = new Vector2(UnityEngine.Random.Range(-10000, 10000), UnityEngine.Random.Range(-10000, 10000));
        transform.position = new Vector3(position.x, 0, position.y);
        Camera.main.transform.position = new Vector3(position.x, 14f, position.y);

        MapGenerator.Instance.MapGeneration((int)position.x, (int)position.y);

        GUIManager.Instance.Print("<color=white>You end up in a new place...</color>");
    }

    void MoveEnemies()
    {
        if (moveEvent != null) moveEvent.Invoke();
    }
}
