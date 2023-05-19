using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ItemSlot : MonoBehaviour
{
    public Item item;
    public ItemInfo info;
    Inventory inventory;
    Image sprite;

    public void Start()
    {
        sprite = GetComponent<Image>();
        inventory = FindObjectOfType<Inventory>();
        info = transform.parent.parent.GetChild(1).gameObject.GetComponent<ItemInfo>();
    }

    public void Update()
    {
        if (transform.GetSiblingIndex() < inventory.inventory.Count) item = inventory.inventory[transform.GetSiblingIndex()];
        else item = null;

        if (item != null) sprite.sprite = item.sprite;
        sprite.enabled = item != null;
    }

    public void MouseEnter()
    {
        if (item != null)
        {
            info.gameObject.SetActive(true);
            info.currItem = transform.GetSiblingIndex();
            CancelInvoke("TurnOff");
        }
    }

    public void MouseExit()
    {
        Invoke("TurnOff", 0.1f);
    }

    void TurnOff()
    {
        if (!info.over && info.currItem == transform.GetSiblingIndex())
        {
            info.currItem = -1;
            print("turned offf");
        }
    }
}
