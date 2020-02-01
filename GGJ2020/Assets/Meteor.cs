using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, destination, speed);
        if (transform.position == destination)
            Destroy(this.gameObject);
    }

    private void OnDestroy() {
        print("poof!");
    }
}
