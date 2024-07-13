using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class ChangeGrabbedObject : MonoBehaviour
{

    HandGrabInteractor handGrabInteractor;
    // Start is called before the first frame update
    void Start()
    {
        handGrabInteractor = GetComponent<HandGrabInteractor>();

        PatternPart.OnTransformedGrab += OnGrabbedObjectChanged;

    }

    private void OnGrabbedObjectChanged(HandGrabInteractable obj)
    {
        if(handGrabInteractor.SelectedInteractable != null)
        {        
            handGrabInteractor.ForceRelease();
            handGrabInteractor.ForceSelect(obj, true);
        }

    }

}