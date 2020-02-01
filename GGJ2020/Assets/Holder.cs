using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] Transform _place;

    Pickup _thisPickUp;

    public void Place(Pickup pickup)
    {
        _thisPickUp = pickup;
        _thisPickUp.transform.parent = _place;
        _thisPickUp.transform.localPosition = Vector3.zero;
        _thisPickUp.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public bool CanPickUp()
    {
        if (_thisPickUp != null)
            return true;
        return false;
    }

    public bool CanDropOff()
    {
        if (_thisPickUp != null)
            return false;
        return true;
    }

    public Pickup Pickup()
    {
        Pickup temp = _thisPickUp;
        _thisPickUp = null;

        return temp;
    }
}
