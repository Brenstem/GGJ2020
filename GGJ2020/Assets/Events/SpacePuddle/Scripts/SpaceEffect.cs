using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceEffect : MonoBehaviour
{
    [SerializeField] string playerTag;
    [SerializeField] float floatTime;
    [SerializeField] float bounceForce;
    [SerializeField] float torqueForce;


    private void OnTriggerEnter(Collider hit)
    {
        if (hit.tag == playerTag)
        {
            hit.GetComponent<eventEffects>().SpacePuddleEffect(floatTime);
            hit.GetComponent<Rigidbody>().AddForce(Vector3.up * bounceForce);
            Vector3 torqueVector = hit.transform.position - this.transform.position;
            hit.GetComponent<Rigidbody>().AddTorque(torqueVector * torqueForce);
            hit.GetComponent<PlayerMovement>().StateMachine.ChangeState(new NoGravityState());
        }
    }
}
