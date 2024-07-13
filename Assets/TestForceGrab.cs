using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.GrabAPI;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class TestForceGrab : MonoBehaviour
{
    public GameObject objectToGrab;
    public GameObject objectToRelease;
    public HandGrabInteractor handGrabInteractor;

    // Start is called before the first frame update
    void Start()
    {
        //handGrabInteractor = GetComponent<HandGrabInteractor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {

                handGrabInteractor.ForceRelease();
                handGrabInteractor.ForceSelect(objectToGrab.GetComponent<HandGrabInteractable>(), true);

        }

        if (Input.GetKeyDown(KeyCode.R))
        {

                handGrabInteractor.ForceRelease();
                handGrabInteractor.ForceSelect(objectToRelease.GetComponent<HandGrabInteractable>(), true);

        }
    }
}
