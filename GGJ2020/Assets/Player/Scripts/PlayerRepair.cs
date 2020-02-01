using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepair : MonoBehaviour
{
    [SerializeField] readonly string repairTag;
    bool fire = Input.GetButton(InputStatics.FIRE_2);

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == repairTag)
        {
            RepairableItem item = hit.GetComponent<RepairableItem>();

            if (fire)
            {

            }
        }
    }
}
