using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System;

[Serializable]
public class MoveEvent : UnityEvent<float, float> { }
public class button : MonoBehaviour
{
    
    public MoveEvent OnPress=new MoveEvent();
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPress.Invoke(UnityEngine.Random.Range(-10f, 20f), UnityEngine.Random.Range(-10f, -20f));
            print("Pressed");
        }
    }
}
