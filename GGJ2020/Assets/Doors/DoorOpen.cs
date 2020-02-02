using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    [SerializeField] float closedTime;
    [SerializeField] float minWaitTime;
    [SerializeField] float maxWaitTime;
    [SerializeField] AudioClip doorOpen;
    [SerializeField] AudioClip doorClose;

    private Transform _door;
    private Animator _animator;
    private bool _doorTimerExpired;
    private float _doorTimer;
    public bool _closeDoors;

    private AudioSource _audio;
    private float _timer;
    private float _spawnTime;

    private void Awake()
    {
        _door = this.transform.GetChild(0);
        _animator = GetComponentInChildren<Animator>();
        _audio = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _door.GetComponentInChildren<BoxCollider>().enabled = false;
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
            _audio.Stop();
            _audio.clip = doorClose;
            _audio.Play();

            _animator.SetBool("closeDoors", true);
            _doorTimer += Time.deltaTime;
            _door.GetComponentInChildren<BoxCollider>().enabled = true;

        }

        if (_doorTimer > closedTime)
        {
            _doorTimerExpired = true;
        }

        if (_doorTimerExpired)
        {
            _audio.Stop();
            _audio.clip = doorOpen;
            _audio.Play();
            _animator.SetBool("closeDoors", false);
            _door.GetComponentInChildren<BoxCollider>().enabled = false;
            _closeDoors = false;
            ResetTimer();
        }
    }

    void PlayCloseDoorSound()
    {

    }

    void ResetTimer()
    {
        _timer = 0;
        _doorTimer = 0;
        _doorTimerExpired = false;
    }
}
