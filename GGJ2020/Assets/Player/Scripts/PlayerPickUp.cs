﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] Transform _holder;

    Rigidbody _playerRigidBody;

    Holder _holderInView = null;
    Pickup _currentPickupInArms = null;
    Pickup _currentPickupInView = null;

    private void Awake()
    {
        _playerRigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        bool fire1 = Input.GetButtonDown(InputStatics.FIRE_1);

        if (fire1)
        {
            //Currently not holding anything, can pick something up though
            if (_currentPickupInArms == null && _currentPickupInView != null)
                PickupNewItem();
            //Holding item and can let go of item
            else if (_currentPickupInArms != null && _holderInView != null)
                DropItemOnHolder();
            else if (_currentPickupInArms != null && _holderInView == null)
                DropItem();
            else if (_currentPickupInArms == null && _holderInView != null)
                PickUpFromHolder();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickup pickupInView = other.GetComponent<Pickup>();
        Holder holder = other.GetComponent<Holder>();

        //If object in view is a pickup
        if (pickupInView != null)
            _currentPickupInView = pickupInView;
        //If object in view is a holder
        if (holder != null)
            _holderInView = holder;
    }

    private void OnTriggerExit(Collider other)
    {
        Pickup pickupOutOfView = other.GetComponent<Pickup>();
        Holder holder = other.GetComponent<Holder>();


        //If object out view is a pickup
        if (pickupOutOfView == _currentPickupInView)
            _currentPickupInView = null;
        //If object out view is a holder
        if (holder == _holderInView)
            _holderInView = null;
    }

    private void PickupNewItem()
    {
        PickupType type = _currentPickupInView.GetPickupType();

        switch(type)
        {
            case (PickupType.DUCTTAPE):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.MOP):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.WRENCH):
                {
                    BasicPickUp();
                    break;
                }
            default: throw new System.Exception("This item is not implemented correctly: " + type);
        }
    }

    private void BasicPickUp()
    {
        _currentPickupInView.transform.parent = _holder;
        _currentPickupInArms = _currentPickupInView;
        _currentPickupInView = null;

        _currentPickupInArms.transform.localPosition = Vector3.zero;
        _currentPickupInArms.transform.rotation = Quaternion.identity;
        _currentPickupInArms.transform.localScale = Vector3.one;

        _currentPickupInArms.PickedUp();
    }

    private void DropItem()
    {
        _currentPickupInArms.Drop(_playerRigidBody.velocity);
        _currentPickupInArms.transform.parent = null;
        _currentPickupInArms.transform.localScale = Vector3.one;
        _currentPickupInArms = null;
    }

    private void DropItemOnHolder()
    {
        _currentPickupInArms.transform.localScale = Vector3.one;
        _holderInView.Place(_currentPickupInArms);
        _currentPickupInArms = null;
    }

    private void PickUpFromHolder()
    {
        _currentPickupInArms = _holderInView.Pickup();

        _currentPickupInArms.transform.parent = _holder;

        _currentPickupInArms.transform.localPosition = Vector3.zero;
        _currentPickupInArms.transform.rotation = Quaternion.identity;
        _currentPickupInArms.transform.localScale = Vector3.one;

        _currentPickupInArms.PickedUp();
    }
}