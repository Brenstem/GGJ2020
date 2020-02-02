using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool flip;
    float rotationAngle;
    private void Start() {
        rotationAngle = Vector3.Angle(transform.forward, Camera.main.transform.forward);
    }
    void Update() {
        transform.rotation = Quaternion.Euler(rotationAngle, 0, 0);
        if (flip) {
            transform.rotation = Quaternion.Euler(-transform.rotation.eulerAngles.x, -transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
