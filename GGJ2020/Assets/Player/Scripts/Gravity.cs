using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [SerializeField] private float gravityModifier;
    [SerializeField] private float mass;

    private bool _useGravity;
    private Rigidbody _rb;
    private Vector3 _prevPosition;
    private Vector3 _newPosition;
    private Vector3 _objectVelocity;

    public bool UseGravity { get => _useGravity; set => _useGravity = value; }
    public Vector3 ObjectVelocity { get => _objectVelocity; set => _objectVelocity = value; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _useGravity = true;
    }

    private void Start()
    {
        _prevPosition = transform.position;
        _newPosition = transform.position;
    }

    private void FixedUpdate()
    {
        _newPosition = transform.position; 
        ObjectVelocity = (_newPosition - _prevPosition) / Time.fixedDeltaTime;  
        _prevPosition = _newPosition;  

        if (_useGravity)
        {
            _rb.AddForce(new Vector3(0, -gravityModifier, 0));

            if (ObjectVelocity.y < 0)
            {
                _rb.AddForce(new Vector3(0, -gravityModifier, 0) * (mass * mass));
            }
        }    
    }
}
