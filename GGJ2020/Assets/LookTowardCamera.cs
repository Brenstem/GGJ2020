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
        transform.LookAt(transform.position + _camera.transform.rotation * Vector3.back, _camera.transform.rotation * Vector3.up);
        this.transform.Rotate(0, 180, 0);
    }
}
