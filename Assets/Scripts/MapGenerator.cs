using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeightedItem
{
    public GameObject item;
    public int weight;
}

[System.Serializable]
public class LevelInfo
{
    public GameObject[] prefabs;
    public int ladderRarity, monsterRarity, itemRarity;
    public WeightedItem[] monsters, items;
    public float lighting = 150f;
}

public class MapGenerator : MonoBehaviour
{
    public static MapGenerator Instance;
    public Light pointLight;

    [Header("Map Info")]
    public Vector2 mapSize;
    public int floor;

    int halfX => Mathf.FloorToInt(mapSize.x / 2);
    int halfY => Mathf.FloorToInt(mapSize.y / 2);

    [HideInInspector]
    public LevelInfo level;

    [Header("Level 0")]
    public LevelInfo l0;
    [Range(0f, 1f)]
    public float l0_wallAmount, l0_pillarAmount, l0_holeAmount;

    [Header("Level 1")]
    public LevelInfo l1;
    [Range(0f, 1f)]
    public float l1_roomAmount;

    [Header("Level 2")]
    public LevelInfo l2;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        level = l1;
        MapGeneration(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        pointLight.intensity = level.lighting;
    }

    int Level0(int x, int y)
    {
        //Walls
        Random.InitState(CantorPair(x, Mathf.FloorToInt((y + 2) / 5)));
        bool randomX = Random.Range(0f, 1f) < l0_wallAmount;

        Random.InitState(CantorPair(x, Mathf.FloorToInt((y + 3) / 5)));
        randomX = randomX || Random.Range(0f, 1f) < l0_wallAmount;

        Random.InitState(CantorPair(Mathf.FloorToInt((x + 2) / 5), y));
        bool randomY = Random.Range(0f, 1f) < l0_wallAmount;

        Random.InitState(CantorPair(Mathf.FloorToInt((x + 3) / 5), y));
        randomY = randomY || Random.Range(0f, 1f) < l0_wallAmount;

        if ((randomX && x % 5 == 0) || (randomY && y % 5 == 0)) return 1;

        //Ladders
        Random.InitState(CantorPair(x, y));
        if (Random.Range(0, l0.ladderRarity) == 0) return 2;

        //Holes
        Random.InitState(CantorPair(Mathf.FloorToInt(x / 7), Mathf.FloorToInt(y / 7)));
        bool hole = Random.Range(0, 5) == 0;
        if (Random.Range(0f, 1f) < l0_holeAmount && (x % 2 == 0 || y % 2 == 0)) return 3;

        //Pillars
        Random.InitState(CantorPair(x, y));
        if (Random.Range(0f, 1f) < l0_pillarAmount) return 1;

        return 0;
    }

    int Level1(int x, int y)
    {
        //Pillars
        if (x % 5 == 0 && y % 4 == 0) return 1;

        //Rooms
        Random.InitState(CantorPair(x, Mathf.FloorToInt((y) / 4)));
        bool randX = Random.Range(0f, 1f) < l1_roomAmount;
        Random.InitState(CantorPair(x + 5, Mathf.FloorToInt((y) / 4)));
        randX = randX || Random.Range(0f, 1f) < l1_roomAmount;

        //Rooms
        Random.InitState(CantorPair(Mathf.FloorToInt((x) / 5), Mathf.FloorToInt((y) / 4)));
        bool randY = Random.Range(0f, 1f) < l1_roomAmount;
        Random.InitState(CantorPair(Mathf.FloorToInt((x) / 5), Mathf.FloorToInt((y - 4) / 4)));
        randY = randY || Random.Range(0f, 1f) < l1_roomAmount;

        if (x % 5 == 0 && randX) return 1;
        if (y % 4 == 0 && randY) return 1;

        return 0;
    }

    int Level2(int x, int y)
    {
        if (x % 10 > 1) return 1;
        if (y % 10 > 1) return 1;

        return 0;
    }

    public void GenerateNew(int x, int y, int a, int b)
    {
        if (a == -1) GenerateLeft(x, y);
        if (a == 1) GenerateRight(x, y);

        if (b == -1) GenerateDown(x, y);
        if (b == 1) GenerateUp(x, y);
    }

    //Generates new set of tiles to the left, and deletes tiles too far right
    void GenerateLeft(int x, int y)
    {
        foreach (Transform item in transform)
        { if (item.position.x > x + halfX) Destroy(item.gameObject); }

        for (int i = 0; i < mapSize.y; i++) { Generate(x - halfX + 1, y + i - halfY); }
    }

    //Generates new set of tiles to the right, and deletes tiles too far left
    void GenerateRight(int x, int y)
    {
        foreach (Transform item in transform)
        { if (item.position.x < x - halfX) Destroy(item.gameObject); }

        for (int i = 0; i < mapSize.y; i++) { Generate(x + halfX - 1, y + i - halfY); }
    }

    //Generates new set of tiles to the left, and deletes tiles too far right
    void GenerateDown(int x, int y)
    {
        foreach (Transform item in transform)
        { if (item.position.z > y + halfY) Destroy(item.gameObject); }

        for (int i = 0; i < mapSize.x; i++) { Generate(x + i - halfX, y - halfY + 1); }
    }

    //Generates new set of tiles to the right, and deletes tiles too far left
    void GenerateUp(int x, int y)
    {
        foreach (Transform item in transform)
        { if (item.position.z < y - halfY) Destroy(item.gameObject); }

        for (int i = 0; i < mapSize.x; i++) { Generate(x + i - halfX, y + halfY - 1); }
    }

    public void MapGeneration(int pX, int pY)
    {
        foreach (Transform item in transform) { Destroy(item.gameObject); }

        for (int x = 0; x < mapSize.x; x++) { for (int y = 0; y < mapSize.y; y++) { Generate(x + pX - Mathf.FloorToInt(mapSize.y / 2), y + pY - Mathf.FloorToInt(mapSize.y / 2)); } }
    }

    public int GetTile(int x, int y)
    {
        int item = 0;

        switch (floor)
        {
            case 0: item = Level0(x, y); break;
            case 1: item = Level1(x, y); break;
            case 2: item = Level2(x, y); break;
        }

        return item;
    }

    void Generate(int x, int y)
    {
        int tile = GetTile(x, y);

        if (tile == 0)
        {
            GameObject obj = Instantiate(level.prefabs[0], gameObject.transform);
            obj.transform.position = new Vector3(x, 0, y);

            Random.InitState(CantorPair(x, y));

            Random.Range(0, level.monsterRarity);

            //Monster Spawning
            if (Random.Range(0, level.monsterRarity) == 0)
            {
                GameObject monster = WeightedChoice(level.monsters);
                monster = Instantiate(monster, gameObject.transform);
                monster.transform.position = new Vector3(x, 0, y);
            }

            //Item Spawning
            if (Random.Range(0, level.itemRarity) == 0)
            {
                GameObject item = WeightedChoice(level.items);
                item = Instantiate(item, gameObject.transform);
                item.transform.position = new Vector3(x, 0, y);
            }
        }

        else if (tile != 0)
        {
            GameObject obj = Instantiate(level.prefabs[tile], gameObject.transform);

            obj.transform.position = new Vector3(x, 0, y);
        }
    }

    public static int CantorPair(int x, int y)
    {
        return (((x + y) * (x + y + 1)) / 2) + y;
    }

    public Fighter EntityAtPos(int x, int y)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Entity");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.GetComponent<AIMovement>().position == new Vector2(x, y)) return enemy.GetComponent<Fighter>();
        }

        return null;
    }

    public ItemHolder ItemAtPos(int x, int y)
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            if (item.transform.position == new Vector3(x, 0, y)) return item.GetComponent<ItemHolder>();
        }

        return null;
    }

    public GameObject WeightedChoice(WeightedItem[] list)
    {
        int sum = 0;

        foreach (var weight in list) { sum += weight.weight; }

        int randomNum = Random.Range(0, sum);

        foreach (var item in list)
        {
            if (randomNum < item.weight) return item.item;

            randomNum = randomNum - item.weight;
        }

        return null;
    }
}
