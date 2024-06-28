using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerNewState : MonoBehaviour
{
    private string buttonName;
    private MenuFSM fsm;

    void Start()
    {
        buttonName = gameObject.name;
        fsm = gameObject.GetComponentInParent<MenuFSM>();
    }

    private void OnTriggerEnter(Collider otherCollider)
    {
        fsm.SetButton(buttonName);
    }
}
