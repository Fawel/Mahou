using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class test_event_script : MonoBehaviour {
    private Rigidbody rg;
    public void push(float a, float b)
    {
        rg = GetComponent<Rigidbody>();
        rg.AddForce(a, 0, b);
    }

}
