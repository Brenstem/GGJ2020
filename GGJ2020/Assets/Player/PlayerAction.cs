using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] PlayerPickUp _playerPickUpScript = null;
    Repairable _repairableInView = null;

    private void Update()
    {
        bool pressedAction = Input.GetButton(InputStatics.FIRE_2);

        if (pressedAction)
        {
            PickupType holdingItem = _playerPickUpScript.CurrentlyHolding();

            if (_repairableInView != null && _repairableInView.CanRepair() && _repairableInView.HaveCurrentTool(holdingItem))
            {
                _playerPickUpScript.RemoveItemIfNotTool();

                _repairableInView.Repair(Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Repairable repairable = other.GetComponent<Repairable>();

        if (repairable != null)
            _repairableInView = repairable;

    }

    private void OnTriggerExit(Collider other)
    {
        Repairable repairable = other.GetComponent<Repairable>();

        if (repairable == _repairableInView)
            _repairableInView = null;
    }
}
