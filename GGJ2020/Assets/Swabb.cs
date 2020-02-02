using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swabb : Repairable
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
    public override void StartedBreak() {
        animator.Play("SpaceLeakAnim");
    }

    public override void JustGotWholeAgain() {
        animator.Play("SpaceLeakReverseAnim");
    }

    private void OnDrawGizmos() {
        Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    }
}
