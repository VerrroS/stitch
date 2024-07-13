using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TableUiControl : MonoBehaviour
{
    [SerializeField]
    private MeshCollider collider;

    private UnityEvent uIPressed;




    void OnTriggerEnter(Collider other)
    {


        uIPressed.Invoke();
        Debug.Log("UI Pressed");
    }
}
