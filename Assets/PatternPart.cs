using System;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;
using static Oculus.Interaction.OneGrabTranslateTransformer;

public class PatternPart : MonoBehaviour
{
    public GameObject flatPatternVisual;
    public GameObject flatPatternVisualOnTable;
    public GameObject spatialPatternVisual;
    public GameObject particleEffect;

    public GameObject StaticSpatialPatternVisual;
    public MeshCollider meshCollider;
    public BoxCollider boxCollider;

    public HandGrabInteractable handGrabInteractable;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool isSnapped = false;
    private bool isGrabbed = false;



    private void Awake()
    {
        // Listen to lock event
    }

    void Start()
    {
        spatialPatternVisual.SetActive(true);
        StaticSpatialPatternVisual.SetActive(false);
        flatPatternVisual.SetActive(false);
        particleEffect.SetActive(false);

        initialPosition = flatPatternVisual.transform.position;
        initialRotation = flatPatternVisual.transform.rotation;

        flatPatternVisualOnTable.SetActive(false);
    }

    private void HandleGrabbed()
    {
        isGrabbed = true;
        StaticSpatialPatternVisual.SetActive(true);
        StartCoroutine(ConvertTo2D());
        StartCoroutine(SnapBackTo3D());
    }

    private IEnumerator ConvertTo2D()
    {
        yield return new WaitForSeconds(2);
        particleEffect.SetActive(true);
        particleEffect.GetComponent<ParticleSystem>().Play();
        spatialPatternVisual.SetActive(false);
        flatPatternVisual.SetActive(true);
        boxCollider.enabled = true;
        meshCollider.enabled = false;
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
        if (!isGrabbed && !isSnapped) // Only invoke if not already grabbed
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
        if (handGrabInteractable.State == InteractableState.Select && !isGrabbed)
        {
            OnGrabbed();
        }

        if (handGrabInteractable.State == InteractableState.Normal && isGrabbed)
        {
            OnReleased();
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

    public void Snap(Vector3 newPosition)
    {
        handGrabInteractable.enabled = false;
        flatPatternVisual.SetActive(false);
      

        OneGrabTranslateConstraints constraints = new OneGrabTranslateConstraints();
        constraints.MinY = new FloatConstraint();
        constraints.MaxY = new FloatConstraint();
        constraints.MinY.Constrain = true;
        constraints.MaxY.Constrain = true;
        constraints.MinY.Value = newPosition.y;
        constraints.MaxY.Value = newPosition.y;
        // Set constraints to lock on y axis of the new position
        flatPatternVisualOnTable.GetComponent<OneGrabTranslateTransformer>().InjectOptionalConstraints(constraints);

        flatPatternVisualOnTable.transform.position = newPosition;
        flatPatternVisualOnTable.SetActive(true);
        //flatPatternVisualOnTable.transform.rotation = newRotation;
        isSnapped = true;

        }

    private void Lock()
    {
        gameObject.GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }
}
