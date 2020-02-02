using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [Header("Drop")]
    [SerializeField] PlayerPickUp _playerPickUpScript = null;
    [SerializeField] AudioClip tapeSound;
    [SerializeField] AudioClip wrenchSound;
    [SerializeField] AudioClip fireExtinguisherSound;
    [SerializeField] AudioClip glueSound;
    [SerializeField] AudioClip mopSound;

    Repairable _repairableInView = null;
    private AudioSource _audio;
    private bool isRepairing;
    private Transform defaultTransform;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        bool pressedAction = Input.GetButton(GetComponent<PlayerMovement>().playerPortOne ? InputStatics.PLACE_1 : InputStatics.PLACE_2);
        var anim = GetComponent<PlayerMovement>().animator;
        isRepairing = false;
        if (pressedAction)
        {
            PickupType holdingItem = _playerPickUpScript.CurrentlyHolding();

            switch (holdingItem)
            {
                case PickupType.WRENCH:
                    _audio.clip = wrenchSound;
                    break;
                case PickupType.ANTI_FLAMETHROWER:
                    _audio.clip = fireExtinguisherSound;
                    break;
                case PickupType.MOP:
                    _audio.clip = mopSound;
                    break;
                case PickupType.METAL:
                    _audio.clip = wrenchSound;
                    break;
                case PickupType.WIRE:
                    _audio.clip = wrenchSound;
                    break;
                case PickupType.SCREW:
                    _audio.clip = wrenchSound;
                    break;
                case PickupType.CHIP:
                    _audio.clip = wrenchSound;
                    break;
                case PickupType.TAPE:
                    _audio.clip = tapeSound;
                    break;
                case PickupType.GLUE:
                    _audio.clip = glueSound;
                    break;
                case PickupType.NOTHING:
                    break;
                default:
                    break;
            }

            if (_repairableInView != null && _repairableInView.CanRepair() && _repairableInView.HaveCurrentTool(holdingItem))
            {
                _audio.Play();
                _playerPickUpScript.RemoveItemIfNotTool();

                _repairableInView.Repair(Time.deltaTime);

                isRepairing = true;
                anim.SetBool("isRepairing", isRepairing);
                var t = GetComponent<PlayerPickUp>()._toolHolder.transform;
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
