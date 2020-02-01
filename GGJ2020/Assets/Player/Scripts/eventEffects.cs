using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventEffects : MonoBehaviour
{
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
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
    private float _floatTimer;
    private float _floatTime;
    private bool _floatTimerStarted;

    public void SpacePuddleEffect(float time)
    {
        print("Slipped on some space");
        _floatTime = time;
        _floatTimerStarted = true;
        GetComponent<PlayerMovement>().enabled = false;
        _rb.velocity = _rb.velocity.normalized * 3;
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
