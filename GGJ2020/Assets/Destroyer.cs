using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : Holder
{
    public override bool CanPickUp()
    {
        return false;
    }

    public override bool CanDropOff()
    {
        return true;
    }

    public override void Place(Pickup pickup)
    {
        if (pickup.GetPickupType() != PickupType.ANTI_FLAMETHROWER && pickup.GetPickupType() != PickupType.WRENCH && pickup.GetPickupType() != PickupType.MOP) 
            Destroy(pickup.gameObject);
    }
}
