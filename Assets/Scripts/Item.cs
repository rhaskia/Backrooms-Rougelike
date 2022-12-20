using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "Items", order = 1)]
public class Item : ScriptableObject
{
    public string description;
    public Color nameColor;
    public Sprite sprite;
    public int use;
    public float strength;
    public GameObject prefab;
}