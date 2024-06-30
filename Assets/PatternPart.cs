using System;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;
using static Oculus.Interaction.OneGrabTranslateTransformer;

public class PatternPart : MonoBehaviour
{
    public GameObject flatPatternVisual; //2d
    public GameObject spatialPatternVisual; //3d
    public GameObject particleEffect;

    public GameObject StaticSpatialPatternVisual;
    public MeshCollider meshCollider;
    public BoxCollider boxCollider;

    public HandGrabInteractable handGrabInteractable;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool isSnapped = false;
    private bool isGrabbed = false;

    public static event System.Action OnTransformed;


    public void Start()
    {
        //Hide();
        StaticSpatialPatternVisual.SetActive(false);
        flatPatternVisual.SetActive(false);
        particleEffect.SetActive(false);

        initialPosition = flatPatternVisual.transform.position;
        initialRotation = flatPatternVisual.transform.rotation;
        //AnchorIT.TshirtInitialized += Initialize;
        //AnchorIT.TshirtHide += Hide;

    }

    public void Initialize()
    {
        
        spatialPatternVisual.SetActive(true);
    }

    //public void Hide()
    //{
    //    spatialPatternVisual.GetComponent<MeshRenderer>().enabled = true;
    //    //spatialPatternVisual.SetActive(false);
    //}

    private void HandleGrabbed()
    {
        if (isGrabbed)
        {
            return;
        }
        isGrabbed = true;       
        StaticSpatialPatternVisual.SetActive(true);
        StartCoroutine(ConvertTo2D());
        //StartCoroutine(SnapBackTo3D());
    }

    private IEnumerator ConvertTo2D()
    {
        yield return new WaitForSeconds(2);
        particleEffect.SetActive(true);
        particleEffect.GetComponent<ParticleSystem>().Play();
        //spatialPatternVisual.SetActive(false);
        spatialPatternVisual.GetComponent<MeshRenderer>().enabled = false;
        spatialPatternVisual.GetComponent<MeshCollider>().enabled = true;
        flatPatternVisual.SetActive(true);
        boxCollider.enabled = true;
        //meshCollider.enabled = false;
        OnTransformed?.Invoke();
    }
    
    private IEnumerator SnapBackTo3D()
    {
        yield return new WaitForSeconds(15);
        if (!isSnapped)
        {
            ConvertTo3D();
        }
    }

    private void ConvertTo3D()
    {
        StopAllCoroutines();
        boxCollider.enabled = false;

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
        if (!isGrabbed) // Only invoke if not already grabbed
        {
            HandleGrabbed();
        }
    }

    public void OnReleased()
    {
        if (isGrabbed) // Only stop if it was grabbed
        {
            isGrabbed = false;
            StopCoroutine(ConvertTo2D());
        }
    }

    public void Update()
    {
        if (handGrabInteractable.State == InteractableState.Select)
        {
            OnGrabbed();
        }

        // Debugging keys to simulate grabbing and releasing
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnGrabbed();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            ConvertTo3D();
        }
    }
}
