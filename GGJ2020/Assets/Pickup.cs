using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PickupType
{
    WRENCH,
    DUCTTAPE,
    MOP
    //NYA HÄR SEN
}

public class Pickup : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] PickupType _pickupType;

    public PickupType GetPickupType() { return _pickupType; }
}
