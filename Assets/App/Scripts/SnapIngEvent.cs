using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapIngEvent : MonoBehaviour
{
    public static event System.Action OnSnap;

    void Start()
    {
       
    
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "2DPattern")
        {
            Debug.Log("Snap");
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            OnSnap?.Invoke();
            //other.gameObject.transform.position = Vector3.zero;
        }
    }
}
