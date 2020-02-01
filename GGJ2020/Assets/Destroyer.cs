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
        Destroy(pickup.gameObject);
    }
}
