using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiCollider : MonoBehaviour
{
    public UnityEvent placeWorkspace;

    public void Start()
    {
        
    }

    public void OnCollisionEnter(UnityEngine.Collision collision)
    {
        Debug.Log("Collision detected");
    }
    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        Debug.Log("Trigger detected");
        placeWorkspace?.Invoke();

    }
}
