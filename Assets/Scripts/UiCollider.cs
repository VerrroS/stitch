using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UiCollider : MonoBehaviour
{
    public UnityEvent placeWorkspace;

    public void OnTriggerEnter(UnityEngine.Collider other)
    {
        placeWorkspace?.Invoke();

    }
}
