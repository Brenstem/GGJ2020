using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] protected Transform _place;

    protected Pickup _thisPickUp;

    public virtual void Place(Pickup pickup)
    {
        _thisPickUp = pickup;
        _thisPickUp.transform.parent = _place;
        _thisPickUp.transform.localPosition = Vector3.zero;
        _thisPickUp.transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    public virtual bool CanPickUp()
    {
        if (_thisPickUp != null)
            return true;
        return false;
    }

    public virtual bool CanDropOff()
    {
        if (_thisPickUp != null)
            return false;
        return true;
    }

    public virtual Pickup Pickup()
    {
        Pickup temp = _thisPickUp;
        _thisPickUp = null;

        return temp;
    }
}
