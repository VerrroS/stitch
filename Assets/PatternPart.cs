using System;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;

public class PatternPart : MonoBehaviour
{
    public GameObject flatPatternVisual;
    public GameObject spatialPatternVisual;
    public GameObject particleEffect;

    public UnityEvent OnGrabbedEvent;

    public Rigidbody rigigtBody;
    public GameObject StaticSpatialPatternVisual;
    public MeshCollider meshCollider;
    public BoxCollider boxCollider;

    public HandGrabInteractable handGrabInteractable;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        spatialPatternVisual.SetActive(true);
        StaticSpatialPatternVisual.SetActive(false);
        flatPatternVisual.SetActive(false);
        particleEffect.SetActive(false);

        OnGrabbedEvent.AddListener(HandleGrabbed);
        initialPosition = flatPatternVisual.transform.position;
        initialRotation = flatPatternVisual.transform.rotation;
    }

    private void HandleGrabbed()
    {
        StaticSpatialPatternVisual.SetActive(true);
        StartCoroutine(ConvertTo2D());
        StartCoroutine(SpnapBackTo3D());
    }

    private IEnumerator ConvertTo2D()
    {
        yield return new WaitForSeconds(2);
        meshCollider.enabled = false;
        particleEffect.SetActive(true);
        spatialPatternVisual.SetActive(false);
        flatPatternVisual.SetActive(true);
        rigigtBody.isKinematic = false;
        boxCollider.enabled = true;
    }

    private IEnumerator SpnapBackTo3D()
    {
        yield return new WaitForSeconds(15);
        ConvertTo3D();
    }

    private void ConvertTo3D()
    {
        boxCollider.enabled = false;
        rigigtBody.isKinematic = true;
        rigigtBody.gameObject.transform.position = Vector3.zero;
        rigigtBody.gameObject.transform.rotation = Quaternion.identity;
        
        // Reset the position of the spatial pattern visual
        spatialPatternVisual.transform.position = StaticSpatialPatternVisual.transform.position;
        spatialPatternVisual.transform.rotation = StaticSpatialPatternVisual.transform.rotation;
        
        // Reset the position and rotation of flatPatternVisual
        flatPatternVisual.transform.position = initialPosition;
        flatPatternVisual.transform.rotation = initialRotation;
        
        spatialPatternVisual.SetActive(true);
        flatPatternVisual.SetActive(false);
        StaticSpatialPatternVisual.SetActive(false);
        meshCollider.enabled = true;
    }

    public void OnGrabbed()
    {
        OnGrabbedEvent.Invoke();
    }

    public void Update()
    {
        // debug if key g is pressed grab the object
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnGrabbed();
        }
        if (handGrabInteractable.State == InteractableState.Select)
        {
            OnGrabbed();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ConvertTo3D();
        }
    }
}
