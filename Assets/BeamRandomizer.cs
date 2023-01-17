using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamRandomizer : MonoBehaviour
{
    public Vector2 sizeRange;
    public Transform beam;

    // Start is called before the first frame update
    void Start()
    {
        MapGenerator mg = MapGenerator.Instance;
        Vector2Int pos = new Vector2Int((int)transform.position.x, (int)transform.position.z);

        bool seperate = mg.GetTile(pos.x + 1, pos.y) == 0 && mg.GetTile(pos.x - 1, pos.y) == 0 &&
                        mg.GetTile(pos.x, pos.y + 1) == 0 && mg.GetTile(pos.x, pos.y - 1) == 0;

        if (seperate)
        {
            float size = Random.Range(sizeRange.x, sizeRange.y);
            beam.localScale = new Vector3(size, beam.lossyScale.y, size);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
