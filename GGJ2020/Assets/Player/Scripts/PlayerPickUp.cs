using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    Holder _holderInView = null;
    Pickup _currentPickupInArms = null;
    Pickup _currentPickupInView = null;

    private void Update()
    {
        bool fire1 = Input.GetButton(InputStatics.FIRE_1);

        if (fire1)
        {
            //Currently not holding anything, can pick something up though
            if (_currentPickupInArms == null && _currentPickupInView != null)
                PickupNewItem();
            //Holding item and can let go of item
            else if (_currentPickupInArms != null && _holderInView != null)
                DropItem();
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
        else if (holder != null)
            _holderInView = holder;
    }

    private void OnTriggerExit(Collider other)
    {
        Pickup pickupOutOfView = other.GetComponent<Pickup>();
        Holder holder = other.GetComponent<Holder>();

        if (pickupOutOfView == _currentPickupInView)
            _currentPickupInView = null;
        //If object in view is a holder
        else if (holder != null)
            _holderInView = null;
    }

    private void PickupNewItem()
    {
        PickupType type = _currentPickupInView.GetPickupType();
    }

    private void DropItem()
    {

    }
}
