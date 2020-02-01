using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Holder
{
    [Header("Drop")]
    [SerializeField] GameObject _pickupPrefab;

    Pickup _nextPickUp;

    private void Awake()
    {
         Spawn();
    }

    private void Spawn()
    {
        GameObject newGameObject = GameObject.Instantiate(_pickupPrefab, Vector3.zero, Quaternion.identity, _place) as GameObject;

        _nextPickUp = newGameObject.GetComponent<Pickup>();
        _nextPickUp.gameObject.SetActive(false);
    }

    public override bool CanDropOff()
    {
        return false;
    }

    public override bool CanPickUp()
    {
        return true;
    }

    public override Pickup Pickup()
    {
        Pickup thisPickUp = _nextPickUp;
        thisPickUp.gameObject.SetActive(true);
        Spawn();
        return thisPickUp;
    }
}
