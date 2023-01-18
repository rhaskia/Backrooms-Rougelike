using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowEntity : MonoBehaviour
{
    public bool active;
    public GameObject entity;
    public int rarity = 1000;

    // Start is called before the first frame update
    void Start()
    {
        Random.InitState(MapGenerator.CantorPair((int)transform.position.x, (int)transform.position.z));
        active = Random.Range(0, rarity) == 0;
        entity.SetActive(active);

        PlayerMovement.Instance.moveEvent += CheckPlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        PlayerMovement.Instance.moveEvent -= CheckPlayer;
    }

    void CheckPlayer()
    {
        if (!active) return;

        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), PlayerMovement.Instance.position);

        if (Mathf.Approximately(distance, 1))
        {
            GUIManager.Instance.Print("The shadowy figure pulls you in...");
            PlayerMovement.Instance.fighter.health = 0;
            SoundManager.Instance.PlayHit();
        }
    }
}
