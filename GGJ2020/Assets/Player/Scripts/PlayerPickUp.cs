using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickUp : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 _mopHolderPosition = new Vector3(0.00513f, 0.00024f, 0);
    [SerializeField] Vector3 _mopHolderRotation = new Vector3(0, 0, 95.2f);
    [SerializeField] Vector3 _wrenchHolderPosition = new Vector3(0, -0.00054f, 0);
    [SerializeField] Vector3 _wrenchHolderRotation = new Vector3(0, 90, 0);
    [SerializeField] Vector3 _flameHolderPosition = new Vector3(-0.00345f, 0.001f, 0);
    [SerializeField] Vector3 _flameHolderRotation = new Vector3(180, 0, -78.8f);

    [Header("Drop")]
    [SerializeField] public Transform _toolHolder;
    [SerializeField] public Transform _itemHolder;

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
        bool fire1 = Input.GetButtonDown(GetComponent<PlayerMovement>().playerPortOne ? InputStatics.GRAB_1 : InputStatics.GRAB_2);

        if (fire1)
        {
            //Currently not holding anything, can pick something up though
            if (_currentPickupInArms == null && _currentPickupInView != null)
                PickupNewItem(_currentPickupInView);
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
        else {
            GetComponent<PlayerMovement>().animator.SetBool("pickupItem", false);
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

    private void PickupNewItem(Pickup pickup)
    {
        PickupType type = _currentPickupInView.GetPickupType();

        GetComponent<PlayerMovement>().animator.SetBool("pickupItem", false);
        switch (type)
        {
            case (PickupType.ANTI_FLAMETHROWER):
                {
                    _toolHolder.localPosition = _flameHolderPosition;
                    _toolHolder.localRotation = Quaternion.Euler(_flameHolderRotation);
                    BasicPickUp(pickup, _toolHolder);
                    break;
                }
            case (PickupType.MOP):
                {
                    _toolHolder.localPosition = _mopHolderPosition;
                    _toolHolder.localRotation = Quaternion.Euler(_mopHolderRotation);
                    BasicPickUp(pickup, _toolHolder);
                    break;
                }
            case (PickupType.WRENCH):
                {
                    _toolHolder.localPosition = _wrenchHolderPosition;
                    _toolHolder.localRotation = Quaternion.Euler(_wrenchHolderRotation);
                    BasicPickUp(pickup, _toolHolder);
                    break;
                }
            default:
                {
                    BasicPickUp(pickup, _itemHolder);
                    var anim = GetComponent<PlayerMovement>().animator;
                    anim.SetBool("pickupItem", true);
                    anim.Play("anim_char_pickup");
                    break;
                }
        }
    }

    private void BasicPickUp(Pickup pickup, Transform holder)
    {
        pickup.transform.parent = holder;
        _currentPickupInArms = pickup;
        _currentPickupInView = null;

        _currentPickupInArms.PickedUp();
    }

    private void DropItem()
    {
        _currentPickupInArms.Drop(_playerRigidBody.velocity);
        _currentPickupInArms.transform.parent = null;
        _currentPickupInArms.transform.localScale = Vector3.one;
        _currentPickupInArms = null;
        GetComponent<PlayerMovement>().animator.SetBool("pickupItem", false);
    }

    private void DropItemOnHolder()
    {
        _currentPickupInArms.transform.localScale = Vector3.one;
        _holderInView.Place(_currentPickupInArms);
        _currentPickupInArms = null;
        GetComponent<PlayerMovement>().animator.SetBool("pickupItem", false);
    }

    private void PickUpFromHolder()
    {

        _currentPickupInArms = _holderInView.Pickup();

        _currentPickupInArms.PickedUp();

        GetComponent<PlayerMovement>().animator.SetBool("pickupItem", false);
        switch (_currentPickupInArms.GetPickupType())
        {
            case (PickupType.ANTI_FLAMETHROWER):
                {
                    _toolHolder.localPosition = _flameHolderPosition;
                    _toolHolder.localRotation = Quaternion.Euler(_flameHolderRotation);
                    _currentPickupInArms.transform.parent = _toolHolder;
                    break;
                }
            case (PickupType.MOP):
                {
                    _toolHolder.localPosition = _mopHolderPosition;
                    _toolHolder.localRotation = Quaternion.Euler(_mopHolderRotation);
                    _currentPickupInArms.transform.parent = _toolHolder;
                    break;
                }
            case (PickupType.WRENCH):
                {
                    _toolHolder.localPosition = _wrenchHolderPosition;
                    _toolHolder.localRotation = Quaternion.Euler(_wrenchHolderRotation);
                    _currentPickupInArms.transform.parent = _toolHolder;
                    break;
                }
            default:
                {
                    _currentPickupInArms.transform.parent = _itemHolder;
                    var anim = GetComponent<PlayerMovement>().animator;
                    anim.SetBool("pickupItem", true);
                    anim.Play("anim_char_pickup");
                    break;
                }
        }

        _currentPickupInArms.transform.localPosition = Vector3.zero;
        _currentPickupInArms.transform.localScale = Vector3.one;
        _currentPickupInArms.transform.localRotation = Quaternion.Euler(0, 0, 0);

    }
}
