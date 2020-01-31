using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float acceleration = 1.0f;
    [SerializeField] private float deceleration = 1.0f;
    [SerializeField] private float maxSpeed = 200.0f;

    private Vector3 _currentDirectionVector;
    private float _currentSpeed;
    private float CurrentSpeed {
        get { return _currentSpeed; }
        set {
            if (value < maxSpeed && value > 0.0f) {
                _currentSpeed = value;
            }
            else if (value >= maxSpeed ) {
                _currentSpeed = maxSpeed;
            }
            else if (value <= 0.0f) {
                _currentSpeed = 0.0f;
            }
        }
    }

    private Rigidbody Rigidbody {
        get {
            return GetComponent<Rigidbody>();
        }
    } 
    void FixedUpdate()
    {
        Rigidbody.velocity = Vector3.zero;
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        Vector3 directionVector = Vector3.Normalize(new Vector3(x, 0, z));
        if (directionVector != Vector3.zero) {
            CurrentSpeed += acceleration;
            _currentDirectionVector = directionVector;
        }
        else {
            CurrentSpeed -= deceleration;
        }

        Rigidbody.velocity += _currentDirectionVector * CurrentSpeed;
    }
}