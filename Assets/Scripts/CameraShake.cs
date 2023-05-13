using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance;
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartShake(float magnitude)
    {
        StartCoroutine("Shake", magnitude);
    }

    public IEnumerator Shake(float magnitude)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float z = Random.Range(-1f, 1f) * magnitude;

            transform.rotation = Quaternion.Euler(90, 0, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.rotation = Quaternion.Euler(90, 0, 0);

    }
}
