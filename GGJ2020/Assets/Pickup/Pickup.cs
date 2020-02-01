using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    WRENCH,
    DUCTTAPE,
    MOP,
    OTHER
    //NYA HÄR SEN
}

public class Pickup : MonoBehaviour
{
    [Header("Drop")]
    Collider _thisCollider;
    Rigidbody _thisRigidBody;

    [Header("Settings")]
    [SerializeField] PickupType _pickupType;

    public PickupType GetPickupType() { return _pickupType; }

    private void Awake()
    {
        _thisCollider = GetComponent<Collider>();
        _thisRigidBody = GetComponent<Rigidbody>();
    }

    public void PickedUp()
    {
        _thisCollider.enabled = false;
        _thisRigidBody.isKinematic = true;
    }

    public void Drop(Vector3 playerVelocity)
    {
        _thisCollider.enabled = true;
        _thisRigidBody.isKinematic = false;
        _thisRigidBody.velocity = playerVelocity;
    }
}
