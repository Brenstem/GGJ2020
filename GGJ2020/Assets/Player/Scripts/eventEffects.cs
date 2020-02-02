using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventEffects : MonoBehaviour
{
    [SerializeField] AudioClip slip1;
    [SerializeField] AudioClip slip2;


    private Rigidbody _rb;
    private AudioSource _audio;
    private float clip;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        clip = Random.Range(0, 100);
        if (_floatTimerStarted)
        {
            _floatTimer += Time.deltaTime;

            if (_floatTimer >= _floatTime)
            {
                ReverseSpacePuddleEffect();
            }
        }
    }


    // Space puddle //
    [SerializeField] private float inertiaDivider;
    private float _floatTimer;
    private float _floatTime;
    private bool _floatTimerStarted;

    public void SpacePuddleEffect(float time)
    {
        print("Slipped on some space");
        if (clip > 50)
        {
            _audio.clip = slip1;
        }
        else
        {
            _audio.clip = slip2;
        }

        _audio.Play();
        _floatTime = time;
        _floatTimerStarted = true;
        GetComponent<PlayerMovement>().enabled = false;
        _rb.velocity = _rb.velocity / inertiaDivider;
       GetComponent<Gravity>().UseGravity = false;
       _rb.constraints = RigidbodyConstraints.None;

    }

    private void ReverseSpacePuddleEffect()
    {
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Gravity>().UseGravity = true;
        _floatTimerStarted = false;
        _floatTimer = 0f;
       _rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }
}
