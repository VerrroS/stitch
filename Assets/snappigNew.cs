using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snappingNew : MonoBehaviour
{
    private Vector3 rayOrigin;
    private Vector3 rayDirection;
    [SerializeField] private GameObject theObjectYouWantToPlace;

    private OVRCameraRig _cameraRig;

    public Transform rightHandTransform;
    public OVRHand rightHand;

    public GameObject test;

    public AnchorIT anchorIT;
    private void Awake()
    {
        _cameraRig = FindObjectOfType<OVRCameraRig>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (!anchorIT.tableInitialized)
            return;

        MRUKAnchor tableAnchor = anchorIT.currentAnchor;


        var ray = GetControllerRay();
        var hit = new RaycastHit();
        MRUKAnchor anchorHit = null;
        MRUK.Instance?.GetCurrentRoom()?.Raycast(ray, Mathf.Infinity, out hit, out anchorHit);

        Debug.Log($"{tableAnchor} and {anchorHit}");
        if(tableAnchor != null && anchorHit != null && tableAnchor == anchorHit)
        {
            ShowHitNormal(hit);
            return;
        }

        if (anchorHit != null)
            Debug.Log($"\nAnchor: {anchorHit.name}\nHit point: {hit.point}\nHit normal: {hit.normal}\n");
    }

    private Ray GetControllerRay()
    {
        Vector3 rayOrigin;
        Vector3 rayDirection;

        rayOrigin = _cameraRig.rightHandAnchor.GetComponentInChildren<OVRHand>().PointerPose.position;
        rayDirection = _cameraRig.rightHandAnchor.GetComponentInChildren<OVRHand>().PointerPose.forward;
        //rayOrigin = rightHand.PointerPose.localPosition;
        //rayDirection = rightHand.PointerPose.forward * -1;


        test.transform.position = rayOrigin;
        test.transform.forward = rayDirection;


        //Quaternion rotation = Quaternion.Euler(0, 0, 90);
        //rayOrigin = rightHandTransform.position;
        //rayDirection = rotation * rightHandTransform.forward;


        Debug.DrawRay(rayOrigin, rayDirection, Color.yellow);
        return new Ray(rayOrigin, rayDirection);
    }

    private void ShowHitNormal(RaycastHit hit)
    {
        if (theObjectYouWantToPlace != null && hit.point != Vector3.zero && hit.distance != 0)
        {
            theObjectYouWantToPlace.SetActive(true);
            theObjectYouWantToPlace.transform.position =
                hit.point + -theObjectYouWantToPlace.transform.up * theObjectYouWantToPlace.transform.localScale.y;
            theObjectYouWantToPlace.transform.rotation = Quaternion.FromToRotation(-Vector3.up, hit.normal);
        }
        else
        {
            theObjectYouWantToPlace.SetActive(false);
        }
    }
}