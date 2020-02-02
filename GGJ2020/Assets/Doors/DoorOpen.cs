using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] float closedTime;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;

    private Transform _door;
    private bool _doorTimerExpired;
    private float _doorTimer;
    public bool _closeDoors;

    private float _timer;
    private float _spawnTime;

    private void Awake()
    {
        _door = this.transform.GetChild(0);
    }

    private void Start()
    {
        _door.gameObject.SetActive(false);
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > Random.Range(minWaitTime, maxWaitTime))
        {
            _closeDoors = true;
        }

        if (_closeDoors)
        {
            _doorTimer += Time.deltaTime;
            _door.gameObject.SetActive(true);
        }

        if (_doorTimer > closedTime)
        {
            _doorTimerExpired = true;
        }

        if (_doorTimerExpired)
        {
            _door.gameObject.SetActive(false);
            _closeDoors = false;
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        _timer = 0;
        _doorTimer = 0;
        _doorTimerExpired = false;
    }
}
