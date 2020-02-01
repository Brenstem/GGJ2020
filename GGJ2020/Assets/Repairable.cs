using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repairable : MonoBehaviour
{
    private float _completion;
    public float Completion {
        get { return _completion; }
        set {
            if (value <= 0.0)
                _completion = 0;
            else if (value >= 1.0f) {
                //event att det är klart typ
                print("Done!");
                _completion = 1.0f;
            }
            else
                _completion = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
