using System;
using System.Collections;
using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using UnityEngine.Events;
using static Oculus.Interaction.OneGrabTranslateTransformer;

public class PatternPart : MonoBehaviour
{
    public GameObject spatialPatternVisual; //3d
    public GameObject particleEffect;

    public HandGrabInteractable handGrabInteractable;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    public bool isSnapped = false;
    private bool isGrabbed = false;

    public static event System.Action OnTransformed;
    public static event System.Action<HandGrabInteractable> OnTransformedGrab;

    [SerializeField] private float transformDuration = 1f;
    [SerializeField] private float snapBackDuration = 15f;

    [SerializeField] private  Material transparentMaterial;
     private GameObject StaticSpatialPatternVisual;
     [SerializeField] private GameObject flatPatternVisualPrefab;
    private GameObject flatPatternVisual;

    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        particleEffect.SetActive(false);
        spatialPatternVisual.SetActive(true);

        // copy spatialPatternVisual and call it StaticSpatialPatternVisual, make it inactive to have a copy of the original part, that will stay in the same position
        StaticSpatialPatternVisual = Instantiate(spatialPatternVisual, spatialPatternVisual.transform.position, spatialPatternVisual.transform.rotation);
        StaticSpatialPatternVisual.SetActive(false);
        StaticSpatialPatternVisual.GetComponent<MeshRenderer>().material = transparentMaterial;

        // instantiate the flatPatternVisual and make it inactive
        flatPatternVisual = Instantiate(flatPatternVisualPrefab, spatialPatternVisual.transform.position, spatialPatternVisual.transform.rotation);
        //flatPatternVisual.transform.parent = spatialPatternVisual.transform.parent;
        // forcegrab the flatPatternVisual
        flatPatternVisual.SetActive(false);
    }

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
        yield return new WaitForSeconds(transformDuration);
        particleEffect.SetActive(true);
        particleEffect.GetComponent<ParticleSystem>().Play();
        spatialPatternVisual.SetActive(false);
        //spatialPatternVisual.GetComponent<MeshRenderer>().enabled = false;
        //spatialPatternVisual.GetComponent<MeshCollider>().enabled = true;
        flatPatternVisual.SetActive(true);
        OnTransformed?.Invoke();
        OnTransformedGrab?.Invoke(flatPatternVisual.GetComponent<HandGrabInteractable>());

    }
    
    private IEnumerator SnapBackTo3D()
    {
        yield return new WaitForSeconds(snapBackDuration);
        if (!isSnapped)
        {
            ConvertTo3D();
        }
    }

    private void ConvertTo3D()
    {
        StopAllCoroutines();
        // Reset the position of the spatial pattern visual
        spatialPatternVisual.transform.position = StaticSpatialPatternVisual.transform.position;
        spatialPatternVisual.transform.rotation = StaticSpatialPatternVisual.transform.rotation;

        // Reset the position and rotation of flatPatternVisual
        flatPatternVisual.transform.position = initialPosition;
        flatPatternVisual.transform.rotation = initialRotation;

        spatialPatternVisual.SetActive(true);
        flatPatternVisual.SetActive(false);
        StaticSpatialPatternVisual.SetActive(false);
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
