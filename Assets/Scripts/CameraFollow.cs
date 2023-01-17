using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Instance;
    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    void Start()
    {
        Instance = this;
        ResetPosition();
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    public static void ResetPosition()
    {
        Instance.transform.position = new Vector3(Instance.target.position.x, Instance.transform.position.y, Instance.target.position.z);
    }
}
