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
        _thisPickUp.transform.rotation = Quaternion.identity;
    }

    public Pickup Pickup()
    {
        Pickup temp = _thisPickUp;
        _thisPickUp = null;

        return temp;
    }
}
