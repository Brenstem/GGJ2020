using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacePoolAnimHandler : MonoBehaviour
{
    public void DestroySpacePool() {
        GameObject.Destroy(transform.parent.gameObject);
    }
}