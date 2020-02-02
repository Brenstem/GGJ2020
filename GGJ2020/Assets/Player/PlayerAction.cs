using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] PlayerPickUp _playerPickUpScript = null;
    Repairable _repairableInView = null;
    private bool isRepairing;
    private Transform defaultTransform;
    private void Update()
    {
        bool pressedAction = Input.GetButton(InputStatics.FIRE_2);
        var anim = GetComponent<PlayerMovement>().animator;
        isRepairing = false;
        if (pressedAction)
        {
            PickupType holdingItem = _playerPickUpScript.CurrentlyHolding();

            if (_repairableInView != null && _repairableInView.CanRepair() && _repairableInView.HaveCurrentTool(holdingItem))
            {
                _playerPickUpScript.RemoveItemIfNotTool();

                _repairableInView.Repair(Time.deltaTime);

                isRepairing = true;
                anim.SetBool("isRepairing", isRepairing);
                var t = GameObject.Find("ToolHolder").transform;
                defaultTransform = t;
                string animation = "anim_char_fix_generic";
                switch (holdingItem) {
                    case PickupType.ANTI_FLAMETHROWER:
                        animation = "anim_char_antiFlame";
                        break;
                    case PickupType.MOP:
                        //t.position = new Vector3(0.00107f, 0.00005f, -0.00635f);
                        //t.rotation = Quaternion.Euler(0.0f, 81.17f, 95.2f);
                        animation = "anim_char_mopping";
                        break;
                    default:
                        break;
                }
                anim.Play(animation);
            }
        }
        /*
        if (!isRepairing) {
            GameObject.Find("ToolHolder").transform.position = defaultTransform.position;
            GameObject.Find("ToolHolder").transform.rotation = defaultTransform.rotation;
        }
        */   
        anim.SetBool("isRepairing", isRepairing);
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
