using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    WRENCH,
    DUCTTAPE,
    MOP,
    OTHER,
    NOTHING
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

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one;
    }

    public void Drop(Vector3 playerVelocity)
    {
        _thisCollider.enabled = true;
        _thisRigidBody.isKinematic = false;
        _thisRigidBody.velocity = playerVelocity;
    }
}
