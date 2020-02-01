using System.Collections;
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
            else if (_currentPickupInArms != null && _holderInView != null && _holderInView.CanDropOff())
                DropItemOnHolder();
            else if (_currentPickupInArms != null && _holderInView == null)
                DropItem();
            else if (_currentPickupInArms == null && _holderInView != null && _holderInView.CanPickUp())
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

    public void RemoveItemIfNotTool()
    {
        PickupType type = _currentPickupInArms.GetPickupType();

        if(type == PickupType.ANTI_FLAMETHROWER || type == PickupType.WRENCH || type == PickupType.MOP) { }
        else
        {
            GameObject.Destroy(_currentPickupInArms.gameObject);
            _currentPickupInArms = null;
        }
    }

    public PickupType CurrentlyHolding()
    {
        if (_currentPickupInArms == null)
            return PickupType.NOTHING;
        else
            return _currentPickupInArms.GetPickupType();
    }

    private void PickupNewItem()
    {
        PickupType type = _currentPickupInView.GetPickupType();

        switch(type)
        {
            case (PickupType.ANTI_FLAMETHROWER):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.CHIP):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.GLUE):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.METAL):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.MOP):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.SCREW):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.TAPE):
                {
                    BasicPickUp();
                    break;
                }
            case (PickupType.WIRE):
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
        _currentPickupInArms.transform.localScale = Vector3.one;
        _currentPickupInArms.transform.localRotation = Quaternion.Euler(0, 0, 0);

        _currentPickupInArms.PickedUp();
    }
}
