using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineRandom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).localPosition = new Vector3(Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f), Random.Range(-0.25f, 0.25f));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
