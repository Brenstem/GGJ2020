using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookTowardCamera : MonoBehaviour
{
    Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Vector3 newPosition = _camera.transform.position - transform.position;
        transform.forward = newPosition;
        Vector3 thisRotation = transform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(0, thisRotation.y, 0);
    }
}
